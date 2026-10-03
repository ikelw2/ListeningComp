using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using API_ASPNETCoreWebAPI;
using SharedDtoClassLibrary.DTOs;
using API_ASPNETCoreWebAPI.Data;
using API_ASPNETCoreWebAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace ListeningComp.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registration(s)
                var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<MyDbContext>) || d.ServiceType == typeof(MyDbContext)).ToList();
                foreach (var d in descriptors) services.Remove(d);

                // Use in-memory database for testing by registering the context directly
                services.AddScoped<MyDbContext>(sp =>
                {
                    var options = new DbContextOptionsBuilder<MyDbContext>()
                        .UseInMemoryDatabase("TestDb")
                        .Options;
                    return new MyDbContext(options);
                });
            });
        }
    }

    public class PassageGradingTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PassageGradingTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SubmitAnswers_ReturnsCorrectCount()
        {
            // Seed database through the factory's services before making requests
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                db.Database.EnsureCreated();

                var passage = new Passage
                {
                    Id = "test-p1",
                    SourceUrl = "src",
                    MediaUrl = "media",
                    Transcription = new List<string> { "t1" },
                    Translation = new List<string> { "tr1" },
                    Questions = new List<Question>
                    {
                        new Question { Id = "q1", Position = 0, QuestionText = "Q1", AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 1, PassageId = "test-p1" },
                        new Question { Id = "q2", Position = 1, QuestionText = "Q2", AnswerChoices = new List<string>{"A","B"}, CorrectChoice = 0, PassageId = "test-p1" }
                    }
                };

                db.Passages.Add(passage);
                db.SaveChanges();
            }

            var client = _factory.CreateClient();

            var submission = new PassageSubmissionDto
            {
                PassageId = "test-p1",
                Answers = new List<QuestionSubmissionDto>
                {
                    new QuestionSubmissionDto { QuestionId = "q1", SelectedAnswer = 1 }, // correct
                    new QuestionSubmissionDto { QuestionId = "q2", SelectedAnswer = 1 }  // incorrect
                }
            };

            var res = await client.PostAsJsonAsync("/api/passages/submit", submission);
            res.EnsureSuccessStatusCode();

            var feedback = await res.Content.ReadFromJsonAsync<SubmissionFeedbackDto>();

            Assert.NotNull(feedback);
            Assert.Equal(2, feedback.TotalQuestions);
            Assert.Equal(1, feedback.CorrectCount);
            Assert.Equal(2, feedback.Results.Count);

            var r1 = feedback.Results.Single(r => r.QuestionId == "q1");
            Assert.True(r1.IsCorrect);

            var r2 = feedback.Results.Single(r => r.QuestionId == "q2");
            Assert.False(r2.IsCorrect);
        }
    }
}
