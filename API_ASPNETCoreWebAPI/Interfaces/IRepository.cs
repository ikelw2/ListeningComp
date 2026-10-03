using SharedDtoClassLibrary.DTOs;

namespace API_ASPNETCoreWebAPI.Interfaces
{
    public interface IRepository
    {
        Task<ListeningDto?> GetEntityForListeningAsync(string id); // for user to listen to a passage

        Task<GradingDto?> GetEntityForGradingAsync(string id); // for grading of answers submission
    }
}
