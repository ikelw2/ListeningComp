using SharedDtoClassLibrary.DTOs;

namespace FrontEnd_BlazorWebAssemblyApp.Services
{
    public interface IPassageService
    {
        Task<List<TitleListDto>> GetTitleListAsync();
        Task<ListeningDto?> GetListeningAsync(string id);
        Task<SubmissionFeedbackDto> SubmitAnswersAsync(SubmissionDto submission);
    }
}