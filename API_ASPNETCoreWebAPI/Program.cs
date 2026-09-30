using API_ASPNETCoreWebAPI.Data;
using API_ASPNETCoreWebAPI.Importing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("LCompDb")));

builder.Services.AddScoped<PassageImporter>();

var app = builder.Build();

// Usage: dotnet run -- --import "C:\QuizContent" ru
if (args.Length > 0 && args[0] == "--import")
{
    if (args.Length != 3)
    {
        throw new ArgumentException(
            "Usage: --import <folder> <language>");
    }

    await using var scope = app.Services.CreateAsyncScope();

    var importer = scope.ServiceProvider
        .GetRequiredService<PassageImporter>();

    var result = await importer.ImportFolderAsync(args[1], args[2]);

    Console.WriteLine(
        $"Imported {result.Imported} passage(s); " +
        $"skipped {result.Skipped} existing passage(s).");

    return;
}
//----------------------------------------------------

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
