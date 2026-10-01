using API_ASPNETCoreWebAPI.DTOs;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API_ASPNETCoreWebAPI.Controllers;

[ApiController]
[Route("api/passages")]
public class PassagesController : ControllerBase
{
    private readonly IPassageRepository _repository;

    // "injecting interface to database" for extra flexibility, which
    // will theoretically allow me to use a different database later on
    public PassagesController(IPassageRepository repository)
    {
        _repository = repository;
    }

    //-----------------------------------------------------------------
    // endpoint 1: GET api/passages/{id}
    // retrieve passage, associated questions, and answer choices to display them to user
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPassageController(string id)
    {
        var response = await _repository.GetPassageFromRepositoryAsync(id); // GetPassageFromRepositoryAsync returns DTO-equivalent type of Passage

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
    public async Task<ActionResult<SubmissionFeedbackDto>> SubmitAnswers([FromBody] PassageSubmissionDto submission)
    {
        if (submission == null) return BadRequest("Invalid submission data.");

        // 1. Fetch the passage and its questions from the repository
        var passage = await _repository.GetPassageEntityAsync(submission.PassageId);

        if (passage == null) return NotFound("Passage not found.");

        var feedback = new SubmissionFeedbackDto
        {
            TotalQuestions = passage.Questions.Count,
            CorrectCount = 0
        };

        // 2. Compare the user's answers against the database records
        foreach (var dbQuestion in passage.Questions.OrderBy(q => q.Position))
        {
            // Find the matching answer submitted by the user for this question
            var userAnswerIndex = submission.Answers
                .FirstOrDefault(a => a.QuestionId == dbQuestion.Id)?.SelectedAnswer;

            // If user did not provide an answer, treat as incorrect
            bool isCorrect = userAnswerIndex.HasValue && userAnswerIndex.Value == dbQuestion.CorrectChoice;

            if (isCorrect)
            {
                feedback.CorrectCount++;
            }

            // 3. Build individual question feedback
            feedback.Results.Add(new QuestionResultDto
            {
                QuestionId = dbQuestion.Id,
                IsCorrect = isCorrect,
                UserAnswer = userAnswerIndex ?? -1,
                CorrectAnswer = dbQuestion.CorrectChoice
            });
        }

        // 4. Return the report immediately. Notice we never call _repository.SaveChanges()!
        return Ok(feedback);
    }





}
