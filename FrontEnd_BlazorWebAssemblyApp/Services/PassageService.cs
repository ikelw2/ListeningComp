using System.Net.Http.Json;
using SharedDtoClassLibrary.DTOs;

namespace FrontEnd_BlazorWebAssemblyApp.Services
{
    public class PassageService : IPassageService
    {
        private readonly HttpClient _http;

        public PassageService(HttpClient http) => _http = http;

        public Task<List<TitleListDto>> GetTitleListAsync()
            => _http.GetFromJsonAsync<List<TitleListDto>>("api/passages")!;

        public Task<ListeningDto?> GetListeningAsync(string id)
            => _http.GetFromJsonAsync<ListeningDto>($"api/passages/{id}");

        public async Task<SubmissionFeedbackDto> SubmitAnswersAsync(SubmissionDto submission)
        {
            var res = await _http.PostAsJsonAsync("api/passages/submit", submission);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<SubmissionFeedbackDto>()!;
        }
    }
}