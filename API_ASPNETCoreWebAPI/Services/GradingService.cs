using SharedDtoClassLibrary.DTOs;
using API_ASPNETCoreWebAPI.Interfaces;

namespace API_ASPNETCoreWebAPI.Services
{

    public class GradingService : IGradingService
    {
        private readonly IRepository _repository;





        public GradingService(IRepository repository)
        {
            _repository = repository;
        }





        public async Task<SubmissionFeedbackDto> ValidateAndGradeAsync(SubmissionDto submission)
        {
            if (submission == null) throw new ArgumentNullException(nameof(submission));

            var passage = await _repository.GetEntityForGradingAsync(submission.PassageId);
            if (passage == null) throw new ArgumentException("Passage not found.", nameof(submission.PassageId));
            // passage now contains a PassageForGradingDto, consisting of one or more QuestionsForGradingDto, consisting of
            // Id, Position, CorrectChoice, and AnswerChoicesCount (to validate given answer is within limits)

            // validate input checking is not a duplicates, is valid questionID

            // collect valid question IDs
            var validQuestionIds = passage.Questions.Select(q => q.QuestionId).ToHashSet();

            // checks for duplicate answers provided to the same question (from hack-engineered API submissions)
            var invalidDuplicateIds = submission.Answers
                .GroupBy(a => a.QuestionId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (invalidDuplicateIds.Any())
                throw new ArgumentException($"Duplicate answers for question(s): {string.Join(',', invalidDuplicateIds)}");

            // checks for any QuestionIds not present in the passage, ie other answers to other questions
            var invalidQuestionIds = submission.Answers
                .Where(a => !validQuestionIds.Contains(a.QuestionId))
                .Select(a => a.QuestionId)
                .ToList();
            if (invalidQuestionIds.Any())
                throw new ArgumentException($"Submission contains invalid question id(s): {string.Join(',', invalidQuestionIds)}");


            // prepare feedback communication Dto

            // build immutable feedback record; collect results in a temp list then create final record
            // similar to creating a string with StringBuilder first
            var tempResults = new List<SubmissionFeedbackAnswersDto>();
            var totalQuestions = passage.Questions.Count;
            var correctCount = 0;

            // check each question included in the passage
            foreach (var dbQuestion in passage.Questions.OrderBy(q => q.Position))
            {
                // checking by QuestionId - whenever it matches the 
            var userAnswerIndex = submission.Answers
                .FirstOrDefault(a => a.QuestionId == dbQuestion.QuestionId)?.SelectedAnswer;

                // Validate bounds if provided
                if (userAnswerIndex.HasValue && (userAnswerIndex.Value < 0 || userAnswerIndex.Value >= dbQuestion.AnswerChoicesCount))
                {
                    throw new ArgumentException($"Selected answer index out of range for question '{dbQuestion.QuestionId}'");
                }

                // assign isCorrect to each question/answer pair included in the passage
                bool isCorrect = userAnswerIndex.HasValue && userAnswerIndex.Value == dbQuestion.CorrectChoice;
                if (isCorrect) correctCount++;

                tempResults.Add(new SubmissionFeedbackAnswersDto
                {
                    QuestionId = dbQuestion.QuestionId,
                    IsCorrect = isCorrect,
                    UserAnswer = userAnswerIndex,
                    CorrectAnswer = dbQuestion.CorrectChoice
                });
            }

            // create immutable feedback DTO from collected mutable tempResults, "projected" into SubmissionFeedbackDto
            var feedback = new SubmissionFeedbackDto
            {
                TotalQuestions = totalQuestions,
                CorrectCount = correctCount,
                Results = tempResults
            };

            return feedback;
        }
    }
}