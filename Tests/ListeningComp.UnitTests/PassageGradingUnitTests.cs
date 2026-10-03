using System.Threading.Tasks;
using Xunit;
using Moq;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.Controllers;
using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.DTOs;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ListeningComp.UnitTests
{
    public class PassageGradingUnitTests
    {
        [Fact]
        public async Task SubmitAnswers_Controller_ComputesCorrectCount()
        {
            // Arrange
            var passage = new Passage
            {
                Id = "p1",
                Questions = new List<Question>
                {
                    new Question { Id = "q1", Position = 0, CorrectChoice = 1 },
                    new Question { Id = "q2", Position = 1, CorrectChoice = 0 }
                }
            };

            var mockRepo = new Mock<IPassageRepository>();
            mockRepo.Setup(r => r.GetPassageEntityAsync("p1")).ReturnsAsync(passage);

            var mockGrading = new Mock<API_ASPNETCoreWebAPI.Services.IGradingService>();
            var expectedFeedback = new SubmissionFeedbackDto
            {
                TotalQuestions = 2,
                CorrectCount = 1,
                Results = new System.Collections.Generic.List<QuestionResultDto>()
            };
            mockGrading.Setup(s => s.GradeAsync(It.IsAny<PassageSubmissionDto>())).ReturnsAsync(expectedFeedback);

            var controller = new PassagesController(mockRepo.Object, mockGrading.Object);

            var submission = new PassageSubmissionDto
            {
                PassageId = "p1",
                Answers = new List<QuestionSubmissionDto>
                {
                    new QuestionSubmissionDto { QuestionId = "q1", SelectedAnswer = 1 },
                    new QuestionSubmissionDto { QuestionId = "q2", SelectedAnswer = 1 }
                }
            };

            // Act
            var actionResult = await controller.SubmitAnswers(submission);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var feedback = Assert.IsType<SubmissionFeedbackDto>(okResult.Value);
            Assert.Equal(2, feedback.TotalQuestions);
            Assert.Equal(1, feedback.CorrectCount);
        }
    }
}
