using System.Data;
using Dapper;

namespace Data.Repositories
{
    public class SprintAnswerRepository : ISprintAnswerRepository
    {
        private readonly IDbConnection _con;
        private const string Table = "user_sprint_answers2";

        public SprintAnswerRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<IList<SprintAnswer>> GetStandupAsync(long userId, long sprintNumber)
        {
            var sql = $@"
SELECT
    scenario_type   AS {nameof(SprintAnswer.ScenarioType)},
    answer_type     AS {nameof(SprintAnswer.AnswerType)},
    sprint_number   AS {nameof(SprintAnswer.SprintNumber)},
    date            AS {nameof(SprintAnswer.Date)},
    answer          AS {nameof(SprintAnswer.Answer)}
FROM {Table}
WHERE user_id = @UserId
  AND sprint_number = @SprintNumber
  AND scenario_type != 'Reflection'";

            var list = await _con.QueryAsync<SprintAnswer>(sql, new { UserId = userId, SprintNumber = sprintNumber });
            return list.AsList();
        }

        public async Task<IList<SprintAnswer>> GetAllStandupsAsync(long userId)
        {
            var sql = $@"
SELECT
    scenario_type   AS {nameof(SprintAnswer.ScenarioType)},
    answer_type     AS {nameof(SprintAnswer.AnswerType)},
    sprint_number   AS {nameof(SprintAnswer.SprintNumber)},
    date            AS {nameof(SprintAnswer.Date)},
    answer          AS {nameof(SprintAnswer.Answer)}
FROM {Table}
WHERE user_id = @UserId
  AND scenario_type != 'Reflection'";

            var list = await _con.QueryAsync<SprintAnswer>(sql, new { UserId = userId });
            return list.AsList();
        }

        public async Task<IList<SprintAnswer>> GetReflectionAsync(long userId, long sprintNumber)
        {
            var sql = $@"
SELECT
    scenario_type   AS {nameof(SprintAnswer.ScenarioType)},
    answer_type     AS {nameof(SprintAnswer.AnswerType)},
    sprint_number   AS {nameof(SprintAnswer.SprintNumber)},
    date            AS {nameof(SprintAnswer.Date)},
    answer          AS {nameof(SprintAnswer.Answer)}
FROM {Table}
WHERE user_id = @UserId
  AND sprint_number = @SprintNumber
  AND scenario_type = 'Reflection'";

            var list = await _con.QueryAsync<SprintAnswer>(sql, new { UserId = userId, SprintNumber = sprintNumber });
            return list.AsList();
        }

        public async Task<IList<SprintAnswer>> GetAllReflectionsAsync(long userId)
        {
            var sql = $@"
SELECT
    scenario_type   AS {nameof(SprintAnswer.ScenarioType)},
    answer_type     AS {nameof(SprintAnswer.AnswerType)},
    sprint_number   AS {nameof(SprintAnswer.SprintNumber)},
    date            AS {nameof(SprintAnswer.Date)},
    answer          AS {nameof(SprintAnswer.Answer)}
FROM {Table}
WHERE user_id = @UserId
  AND scenario_type = 'Reflection'";

            var list = await _con.QueryAsync<SprintAnswer>(sql, new { UserId = userId });
            return list.AsList();
        }
    }
}
