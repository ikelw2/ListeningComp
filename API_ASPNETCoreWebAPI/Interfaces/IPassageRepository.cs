using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.DTOs;

namespace API_ASPNETCoreWebAPI.Interfaces
{
    public interface IPassageRepository
    {
        // Returns a GetPassageResponseDto or null if not found
        Task<GetPassageResponseDto?> GetPassageFromDatabaseAsync(string id);
    }
}
