using Dapper;
using System.Data;

namespace Data.Repositories
{
    public class SprintRepository : ISprintRepository
    {
        private readonly IDbConnection _con;
        private readonly string _sprintsTable = "sprints_info";

        public SprintRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<Sprint?> GetSprintAsync(long userId, long sprintNumber)
        {
            var sql = $@"
SELECT 
    sprint_number AS {nameof(Sprint.Number)},
    weeks_count AS {nameof(Sprint.WeeksCount)},
    sprint_start_date AS {nameof(Sprint.BeginDate)},
    sheet_id AS {nameof(Sprint.SheetId)},
    user_id AS {nameof(Sprint.UserId)}
FROM {_sprintsTable}
WHERE user_id = @UserId AND sprint_number = @SprintNumber";

            var sprint = await _con.QueryFirstOrDefaultAsync<Sprint>(sql, new { UserId = userId, SprintNumber = sprintNumber });
            return sprint;
        }

        public async Task CreateSprintAsync(Sprint sprint)
        {
            var sql = $@"
INSERT INTO {_sprintsTable}
(
    sprint_number,
    weeks_count,
    sprint_start_date,
    sheet_id,
    user_id
)
VALUES
(
    @Number,
    @WeeksCount,
    @BeginDate,
    @SheetId,
    @UserId
)";
            await _con.ExecuteAsync(sql, sprint);
        }

        public async Task UpdateSprintAsync(Sprint sprint)
        {
            var sql = $@"
UPDATE {_sprintsTable}
SET 
    weeks_count = @WeeksCount,
    sprint_start_date = @BeginDate,
    sheet_id = @SheetId
WHERE user_id = @UserId AND sprint_number = @Number";
            await _con.ExecuteAsync(sql, sprint);
        }

        public async Task<IList<Sprint>> GetSprintsByUserIdAsync(long userId)
        {
            var sql = $@"
SELECT 
    sprint_number AS {nameof(Sprint.Number)},
    weeks_count AS {nameof(Sprint.WeeksCount)},
    sprint_start_date AS {nameof(Sprint.BeginDate)},
    sheet_id AS {nameof(Sprint.SheetId)},
    user_id AS {nameof(Sprint.UserId)}
FROM {_sprintsTable}
WHERE user_id = @UserId
ORDER BY sprint_number DESC";

            var sprints = await _con.QueryAsync<Sprint>(sql, new { UserId = userId });
            return sprints.ToList();
        }
    }
}
