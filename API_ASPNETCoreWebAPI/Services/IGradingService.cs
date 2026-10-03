using System.Threading.Tasks;
using API_ASPNETCoreWebAPI.DTOs;

namespace API_ASPNETCoreWebAPI.Services;

public interface IGradingService
{
    // Grades the provided submission and returns feedback. Throws ArgumentException for validation errors.
    Task<SubmissionFeedbackDto> ValidateAndGradeAsync(SubmissionDto submission);
}
