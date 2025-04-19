using Data.Repositories;
using Service.Dto;
using Data;

namespace Service
{
    public class SprintAnswerService : ISprintAnswerService
    {
        private readonly ISprintAnswerRepository _answerRepository;

        public SprintAnswerService(ISprintAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<IList<StandupResponse>> GetStandupAsync(long userId, long sprintNumber)
        {
            var answers = await _answerRepository.GetStandupAsync(userId, sprintNumber);
            return MapToStandupResponses(answers);
        }

        public async Task<IList<StandupResponse>> GetAllStandupsAsync(long userId)
        {
            var answers = await _answerRepository.GetAllStandupsAsync(userId);
            return MapToStandupResponses(answers);
        }

        public async Task<IList<ReflectionResponse>> GetReflectionAsync(long userId, long sprintNumber)
        {
            var answers = await _answerRepository.GetReflectionAsync(userId, sprintNumber);
            return MapToReflectionResponses(answers);
        }

        public async Task<IList<ReflectionResponse>> GetAllReflectionsAsync(long userId)
        {
            var answers = await _answerRepository.GetAllReflectionsAsync(userId);
            return MapToReflectionResponses(answers);
        }

        private static IList<StandupResponse> MapToStandupResponses(IList<SprintAnswer> answers)
        {
            var list = new List<StandupResponse>(answers.Count);
            foreach (var ans in answers)
            {
                list.Add(new StandupResponse
                {
                    AnswerType = ans.AnswerType,
                    SprintNumber = ans.SprintNumber,
                    Date = ans.Date,
                    Answer = ans.Answer
                });
            }
            return list;
        }

        private static IList<ReflectionResponse> MapToReflectionResponses(IList<SprintAnswer> answers)
        {
            var list = new List<ReflectionResponse>(answers.Count);
            foreach (var ans in answers)
            {
                list.Add(new ReflectionResponse
                {
                    AnswerType = ans.AnswerType,
                    SprintNumber = ans.SprintNumber,
                    Date = ans.Date,
                    Answer = ans.Answer
                });
            }
            return list;
        }
    }
}
