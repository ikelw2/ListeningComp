using Microsoft.EntityFrameworkCore;
using API_ASPNETCoreWebAPI.Data;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.DTOs;

namespace API_ASPNETCoreWebAPI.Repository
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _dbContext;





        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }





        public async Task<ListeningDto?> GetEntityForListeningAsync(string id)
        {
            // queries the Passages table for the row with the given Id and
            // uses "projection" to return a ListeningDto & ListeningQuestionDto data type (or null if not found)
            return await _dbContext.Passages

                .Where(p => p.Id == id) // select first passage that matches Id string

                .Select(p => new ListeningDto // returns ListeningDto
                {
                    SourceUrl = p.SourceUrl,
                    MediaUrl = p.MediaUrl,
                    Transcription = p.Transcription,
                    Translation = p.Translation,
                    Questions = p.Questions
                        .OrderBy(q => q.Position)
                        .Select(q => new ListeningQuestionDto // with Questions
                        {
                            QuestionText = q.QuestionText,
                            AnswerChoices = q.AnswerChoices
                        }).ToList()
                })

                .FirstOrDefaultAsync(); // return first (and only) match, because Id is unique identifier of Passages
        }

        public async Task<GradingDto?> GetEntityForGradingAsync(string id)
        {
            // this is called in ValidateAndGradeAsync method of GradingServices class

            // queries the Passages table for the row with the given Passage Id and
            // uses "projection" to return a GradingDto & GradingAnswersDto
            return await _dbContext.Passages
                .Where(p => p.Id == id)
                .Select(p => new GradingDto
                {
                    Questions = p.Questions
                        .OrderBy(q => q.Position)
                        .Select(q => new GradingAnswersDto
                        {
                            QuestionId = q.Id,
                            Position = q.Position,
                            CorrectChoice = q.CorrectChoice,
                            AnswerChoicesCount = q.AnswerChoices.Count
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }

    }
}
