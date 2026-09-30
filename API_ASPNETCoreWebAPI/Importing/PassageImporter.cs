using System.Text.Json;
using API_ASPNETCoreWebAPI.Data;
using API_ASPNETCoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace API_ASPNETCoreWebAPI.Importing;

public sealed class PassageImporter
{
    private readonly AppDbContext _db;

    public PassageImporter(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(int Imported, int Skipped)> ImportFolderAsync(
    string folder,
    string language,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(language))
            throw new ArgumentException("A language is required.", nameof(language));

        folder = Path.GetFullPath(folder);

        if (!Directory.Exists(folder))
            throw new DirectoryNotFoundException(folder);

        // Ensure the database schema exists when running the importer directly.
        // (Will be executed below before querying existing keys.)

        var files = Directory.GetFiles(
            folder, "*.json", SearchOption.AllDirectories);

        Array.Sort(files, StringComparer.Ordinal);

        // Ensure the database schema exists when running the importer
        // directly from the command line (quick local/dev workflow).
        // If you use EF migrations in your workflow, prefer
        // Database.MigrateAsync instead and keep migrations in source.
        await _db.Database.EnsureCreatedAsync(cancellationToken);

        var existingKeys = await _db.Passages
            .Where(p => p.ImportKey != null)
            .Select(p => p.ImportKey!)
            .ToListAsync(cancellationToken);

        var knownKeys = new HashSet<string>(
            existingKeys, StringComparer.Ordinal);

        int imported = 0;
        int skipped = 0;

        foreach (var file in files)
        {
            // Keep folder structure and filenames stable between imports.
            var importKey = Path.GetRelativePath(folder, file)
                .Replace('\\', '/');

            if (knownKeys.Contains(importKey))
            {
                skipped++;
                continue;
            }

            Passage passage;

            try
            {
                await using var stream = File.OpenRead(file);

                var input = await JsonSerializer.DeserializeAsync<PassageJson>(
                    stream,
                    cancellationToken: cancellationToken);

                if (input is null)
                    throw new InvalidDataException("The JSON contains null.");

                passage = Map(input, language, importKey);

                // assign filename to mp3 mediaurl value...
                var mp3Filename = Path.GetFileNameWithoutExtension(file) + ".mp3";
                passage.MediaUrl = "/audio/" + Uri.EscapeDataString(mp3Filename);
            }
            catch (Exception ex) when (
                ex is JsonException ||
                ex is InvalidDataException ||
                ex is IOException)
            {
                throw new InvalidDataException(
                    $"Could not import '{file}': {ex.Message}", ex);
            }

            _db.Passages.Add(passage);
            knownKeys.Add(importKey);
            imported++;
        }

        // Commit all validated passages in one operation.
        await _db.SaveChangesAsync(cancellationToken);

        return (imported, skipped);
    }
        
    private static Passage Map(
        PassageJson input,
        string language,
        string importKey)
    {
        ValidateParagraphs(input.Transcript, "transcript");
        ValidateParagraphs(input.Translation, "translation");

        if (input.Questions is null || input.Questions.Count == 0)
            throw new InvalidDataException(
                "At least one question is required.");

        var sourceUrl = string.Empty;
        if (input.SourceLink.ValueKind == JsonValueKind.String)
        {
            sourceUrl = input.SourceLink.GetString() ?? string.Empty;
        }
        else if (input.SourceLink.ValueKind == JsonValueKind.Array)
        {
            // If array, take first string element if present.
            foreach (var item in input.SourceLink.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    sourceUrl = item.GetString() ?? string.Empty;
                    break;
                }
            }
        }

        var passage = new Passage
        {
            ImportKey = importKey,
            Language = language,
            SourceUrl = sourceUrl,
            Transcription = input.Transcript!,
            Translation = input.Translation!,
            MediaUrl = string.Empty
        };

        for (int position = 0; position < input.Questions.Count; position++)
        {
            var row = input.Questions[position];

            // Your current format: prompt, A, B, C, D, answer letter.
            if (row is null || row.Count != 6 ||
                row.Any(value => string.IsNullOrWhiteSpace(value)))
            {
                throw new InvalidDataException(
                    $"Question {position + 1} must contain a prompt, " +
                    "four nonempty choices, and an answer letter.");
            }

            var answer = row[5].Trim().ToUpperInvariant();

            if (answer.Length != 1 ||
                answer[0] < 'A' || answer[0] > 'D')
            {
                throw new InvalidDataException(
                    $"Question {position + 1} has an invalid answer: '{row[5]}'.");
            }

            passage.Questions.Add(new Question
            {
                PassageId = passage.Id,
                Position = position,
                QuestionText = row[0],
                AnswerChoices = row.Skip(1).Take(4).ToList(),
                CorrectChoice = answer[0] - 'A'
            });
        }

        return passage;
    }

    private static void ValidateParagraphs(
        List<string>? paragraphs,
        string field)
    {
        if (paragraphs is null || paragraphs.Count == 0 ||
            paragraphs.Any(value => string.IsNullOrWhiteSpace(value)))
        {
            throw new InvalidDataException(
                $"'{field}' must contain nonempty paragraph strings.");
        }
    }
}