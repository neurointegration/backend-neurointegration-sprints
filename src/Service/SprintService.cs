using Data.Repositories;
using Data;
using Service.Dto;

namespace Service
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;

        public SprintService(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<SprintResponse> GetSprintByIdAsync(long userId, long sprintNumber)
        {
            var sprint = await _sprintRepository.GetSprintAsync(userId, sprintNumber);

            if (sprint == null)
                throw new UnauthorizedAccessException("Access denied to this sprint.");

            return MapToSprintResponse(sprint);
        }

        public async Task<IList<SprintResponse>> GetSprintsByUserIdAsync(long userId)
        {
            var sprints = await _sprintRepository.GetSprintsByUserIdAsync(userId);
            return sprints.Select(MapToSprintResponse).ToList();
        }

        public async Task CreateSprintAsync(long userId, CreateSprintRequest request)
        {
            var sprints = await _sprintRepository.GetSprintsByUserIdAsync(userId);
            var sprintNumber = 0L;
            if (sprints.Count > 0)
            {
                sprintNumber = sprints[0].Number + 1;
            }
            var sprint = new Sprint
            {
                Number = sprintNumber,
                UserId = userId,
                WeeksCount = request.WeeksCount,
                BeginDate = request.BeginDate
            };

            await _sprintRepository.CreateSprintAsync(sprint);
        }

        private SprintResponse MapToSprintResponse(Sprint sprint)
        {
            var weeksCount = sprint.WeeksCount == 3 ? 3 : 4;
            var weeks = new Dictionary<int, SprintWeek>();
            for (var i = 0; i < weeksCount; i++)
            {
                weeks.Add(
                    i + 1,
                    new SprintWeek
                    {
                        Begin = sprint.BeginDate.AddDays(i * 7),
                        End = sprint.BeginDate.AddDays(i * 7 + 6)
                    }
                );
            }
            return new SprintResponse
            {
                id = sprint.Number,
                Number = sprint.Number,
                UserId = sprint.UserId,
                WeeksCount = weeksCount,
                BeginDate = sprint.BeginDate,
                EndDate = sprint.BeginDate.AddDays(sprint.WeeksCount ?? 3 * 7 - 1),
                Weeks = weeks
            };
        }
    }
}
