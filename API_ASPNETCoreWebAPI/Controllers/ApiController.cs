using SharedDtoClassLibrary.DTOs;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API_ASPNETCoreWebAPI.Controllers;

[ApiController]
[Route("api/passages")]
public class ApiController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly API_ASPNETCoreWebAPI.Services.IGradingService _gradingService;

    // Constructor-based DI
    // "injecting interface to database" for extra flexibility, which
    // will theoretically allow me to use a different database later on
    public ApiController(IRepository repository, API_ASPNETCoreWebAPI.Services.IGradingService gradingService)
    {
        _repository = repository;
        _gradingService = gradingService;
    }


    //-----------------------------------------------------------------
    // endpoint 1: GET api/passages/{id}
    // retrieve passage, associated questions, and answer choices to display them to user for their review
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPassageByIdAsync(string id)
    {
        var response = await _repository.GetEntityForListeningAsync(id); // returns DTO-equivalent type of Passage

        if (response == null)
        {
            return NotFound(new { message = $"Passage ID '{id}' was not found." });
        }

        return Ok(response);
    }


    //-----------------------------------------------------------------
    // endpoint 2: POST api/passages/submit
    // submit passage Id and answer selection for grading/feedback
    [HttpPost("submit")]
    public async Task<ActionResult<SubmissionFeedbackDto>> PostUserAnswersAsync([FromBody] SubmissionDto submission)
    {
        if (submission == null) return BadRequest("Invalid submission data.");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var feedback = await _gradingService.ValidateAndGradeAsync(submission);
            return Ok(feedback);
        }
        catch (ArgumentException ex)
        {
            // GradingService uses ArgumentException for validation and not-found scenarios.
            // If the exception indicates a missing passage, return 404 'not found'; otherwise 400 'bad request'.
            if (string.Equals(ex.ParamName, nameof(submission.PassageId), StringComparison.OrdinalIgnoreCase)
                || ex.Message?.IndexOf("not found", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return NotFound(new { message = ex.Message });
            }

            return BadRequest(new { message = ex.Message });
        }
    }





}
