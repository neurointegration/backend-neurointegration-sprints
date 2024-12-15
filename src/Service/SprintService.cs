using Data.Repositories;
using Data;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;

        public SprintService(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<SprintResponse> GetSprintByIdAsync(Guid userId, Guid sprintId)
        {
            var sprint = await _sprintRepository.GetSprintByIdAsync(sprintId);

            if (sprint == null || sprint.UserId != userId)
                throw new UnauthorizedAccessException("Access denied to this sprint.");

            return MapToSprintResponse(sprint);
        }

        public async Task<IList<SprintResponse>> GetSprintsByUserIdAsync(Guid userId)
        {
            var sprints = await _sprintRepository.GetSprintsByUserIdAsync(userId);
            return sprints.Select(MapToSprintResponse).ToList();
        }

        public async Task CreateSprintAsync(Guid userId, CreateSprintRequest request)
        {
            var sprint = new Sprint
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                WeeksCount = request.WeeksCount,
                BeginDate = request.BeginDate,
                EndDate = request.EndDate,
                Weeks = JsonSerializer.Serialize(request.Weeks)
            };

            await _sprintRepository.CreateSprintAsync(sprint);
        }

        private SprintResponse MapToSprintResponse(Sprint sprint)
        {
            var weeks = JsonSerializer.Deserialize<Dictionary<int, SprintWeekDto>>(sprint.Weeks);

            return new SprintResponse
            {
                id = sprint.Id,
                WeeksCount = sprint.WeeksCount,
                BeginDate = sprint.BeginDate,
                EndDate = sprint.EndDate,
                Weeks = weeks ?? new Dictionary<int, SprintWeekDto>()
            };
        }
    }
}
