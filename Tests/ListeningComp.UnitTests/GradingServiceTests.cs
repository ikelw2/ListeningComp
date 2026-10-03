using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using API_ASPNETCoreWebAPI.Services;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.Models;
using API_ASPNETCoreWebAPI.DTOs;

namespace ListeningComp.UnitTests
{
    public class GradingServiceTests
    {
        [Fact]
        public async Task GradeAsync_NullSubmission_ThrowsArgumentNullException()
        {
            var mockRepo = new Mock<IPassageRepository>();
            var svc = new GradingService(mockRepo.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => svc.GradeAsync(null!));
        }

        [Fact]
        public async Task GradeAsync_PassageNotFound_ThrowsArgumentException()
        {
            var mockRepo = new Mock<IPassageRepository>();
            mockRepo.Setup(r => r.GetPassageEntityAsync("p1")).ReturnsAsync((Passage?)null);

            var svc = new GradingService(mockRepo.Object);

            var submission = new PassageSubmissionDto { PassageId = "p1", Answers = new List<QuestionSubmissionDto>() };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => svc.GradeAsync(submission));
            // GradingService uses nameof(submission.PassageId) which resolves to "PassageId"
            Assert.Equal("PassageId", ex.ParamName);
        }

        [Fact]
        public async Task GradeAsync_DuplicateAnswers_ThrowsArgumentException()
        {
            var passage = new Passage
            {
                Id = "p1",
                Questions = new List<Question>
                {
                    new Question { Id = "q1", Position = 0, AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 0 },
                    new Question { Id = "q2", Position = 1, AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 1 }
                }
            };

            var mockRepo = new Mock<IPassageRepository>();
            mockRepo.Setup(r => r.GetPassageEntityAsync("p1")).ReturnsAsync(passage);

            var svc = new GradingService(mockRepo.Object);

            var submission = new PassageSubmissionDto
            {
                PassageId = "p1",
                Answers = new List<QuestionSubmissionDto>
                {
                    new QuestionSubmissionDto { QuestionId = "q1", SelectedAnswer = 0 },
                    new QuestionSubmissionDto { QuestionId = "q1", SelectedAnswer = 1 }
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => svc.GradeAsync(submission));
        }

        [Fact]
        public async Task GradeAsync_InvalidQuestionId_ThrowsArgumentException()
        {
            var passage = new Passage
            {
                Id = "p1",
                Questions = new List<Question>
                {
                    new Question { Id = "q1", Position = 0, AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 0 }
                }
            };

            var mockRepo = new Mock<IPassageRepository>();
            mockRepo.Setup(r => r.GetPassageEntityAsync("p1")).ReturnsAsync(passage);

            var svc = new GradingService(mockRepo.Object);

            var submission = new PassageSubmissionDto
            {
                PassageId = "p1",
                Answers = new List<QuestionSubmissionDto>
                {
                    new QuestionSubmissionDto { QuestionId = "qX", SelectedAnswer = 0 }
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => svc.GradeAsync(submission));
        }

        [Fact]
        public async Task GradeAsync_SelectedAnswerOutOfRange_ThrowsArgumentException()
        {
            var passage = new Passage
            {
                Id = "p1",
                Questions = new List<Question>
                {
                    new Question { Id = "q1", Position = 0, AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 0 }
                }
            };

            var mockRepo = new Mock<IPassageRepository>();
            mockRepo.Setup(r => r.GetPassageEntityAsync("p1")).ReturnsAsync(passage);

            var svc = new GradingService(mockRepo.Object);

            var submission = new PassageSubmissionDto
            {
                PassageId = "p1",
                Answers = new List<QuestionSubmissionDto>
                {
                    new QuestionSubmissionDto { QuestionId = "q1", SelectedAnswer = 5 }
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => svc.GradeAsync(submission));
        }

        [Fact]
        public async Task GradeAsync_ValidSubmission_ReturnsCorrectFeedback()
        {
            var passage = new Passage
            {
                Id = "p1",
                Questions = new List<Question>
                {
                    new Question { Id = "q1", Position = 0, AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 1 },
                    new Question { Id = "q2", Position = 1, AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 0 }
                }
            };

            var mockRepo = new Mock<IPassageRepository>();
            mockRepo.Setup(r => r.GetPassageEntityAsync("p1")).ReturnsAsync(passage);

            var svc = new GradingService(mockRepo.Object);

            var submission = new PassageSubmissionDto
            {
                PassageId = "p1",
                Answers = new List<QuestionSubmissionDto>
                {
                    new QuestionSubmissionDto { QuestionId = "q1", SelectedAnswer = 1 }, // correct
                    new QuestionSubmissionDto { QuestionId = "q2", SelectedAnswer = 1 }  // incorrect
                }
            };

            var feedback = await svc.GradeAsync(submission);

            Assert.Equal(2, feedback.TotalQuestions);
            Assert.Equal(1, feedback.CorrectCount);
            Assert.Equal(2, feedback.Results.Count);

            var r1 = feedback.Results.Find(r => r.QuestionId == "q1");
            Assert.True(r1.IsCorrect);
            Assert.Equal(1, r1.UserAnswer);
            Assert.Equal(1, r1.CorrectAnswer);

            var r2 = feedback.Results.Find(r => r.QuestionId == "q2");
            Assert.False(r2.IsCorrect);
            Assert.Equal(1, r2.UserAnswer);
            Assert.Equal(0, r2.CorrectAnswer);
        }
    }
}
