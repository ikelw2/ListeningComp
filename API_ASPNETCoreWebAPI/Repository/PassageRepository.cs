using Microsoft.EntityFrameworkCore;
using API_ASPNETCoreWebAPI.Data;
using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.DTOs;
using System.Linq;

namespace API_ASPNETCoreWebAPI.Repository
{
    public class PassageRepository : IPassageRepository
    {
        private readonly MyDbContext _dbContext;

        public PassageRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetPassageResponseDto?> GetPassageFromDatabaseAsync(string id)
        {
            // use "(repository) projection" to select one of 'Passage' type, but send 'GetPassageResponseDto' (DTO) type to the controller
            return await _dbContext.Passages
                
                .Where(p => p.Id == id) // select first passage that matches Id string
                
                .Select(p => new GetPassageResponseDto // return DTO-equivalent of Passage type
                {
                    SourceUrl = p.SourceUrl,
                    MediaUrl = p.MediaUrl,
                    Transcription = p.Transcription,
                    Translation = p.Translation,
                    Questions = p.Questions
                        .OrderBy(q => q.Position) 
                        .Select(q => new GetQuestionResponseDto // along with corresponding DTO-equivalent of Question type
                        {
                            QuestionText = q.QuestionText,
                            AnswerChoices = q.AnswerChoices
                        }).ToList()
                })
                
                .FirstOrDefaultAsync(); // return first (and only) match, because Id is unique identifier of Passages
        }
    }
}
