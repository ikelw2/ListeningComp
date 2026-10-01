using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.DTOs;

namespace API_ASPNETCoreWebAPI.Interfaces
{
    public interface IPassageRepository
    {
        // Returns a GetPassageResponseDto or null if not found
        // GetPassageResponseDto is DTO-format equivalent of Passage type
        Task<GetPassageResponseDto?> GetPassageFromRepositoryAsync(string id);

        // Returns the Passage entity (including Questions) for server-side grading/processing
        Task<Passage?> GetPassageEntityAsync(string id);
    }
}
