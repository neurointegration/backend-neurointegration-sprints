using Dapper;
using System.Data;
using System.Text.Json;

namespace Data.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly IDbConnection _con;
        private readonly string _usersInfoTable = "users_info";
        private readonly string _usersAccessTable = "users_access";
        private readonly string _projectsTable = "projects";
        private readonly string _tasksTable = "tasks";

        public TrainerRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<ApplicationUser?> GetClientAsync(long trainerId, long clientId)
        {
            var sql = $@"
SELECT ui.chat_id AS {nameof(ApplicationUser.Id)},
       ui.email AS {nameof(ApplicationUser.Email)},
       ui.username AS {nameof(ApplicationUser.UserName)},
       ui.iam_coach AS {nameof(ApplicationUser.IAmCoach)},
       ui.send_regular_messages AS {nameof(ApplicationUser.SendRegularMessages)},
       ui.name AS {nameof(ApplicationUser.Name)},
       ui.is_onboarding_complete AS {nameof(ApplicationUser.IsOnboardingComplete)},
       ui.about_me AS {nameof(ApplicationUser.AboutMe)},
       ui.sprint_weeks_count AS {nameof(ApplicationUser.SprintWeeksCount)},
       ui.photo_url AS {nameof(ApplicationUser.PhotoUrl)},
       ui.password_hash AS {nameof(ApplicationUser.PasswordHash)}
FROM {_usersInfoTable} ui
JOIN {_usersAccessTable} ua ON ui.chat_id = ua.owner_user_id
WHERE ua.granted_user_id = @TrainerId
  AND ua.owner_user_id = @ClientId";

            var client = await _con.QueryFirstOrDefaultAsync<ApplicationUser>(
                sql,
                new { TrainerId = trainerId, ClientId = clientId });
            return client;
        }

        public async Task<IList<ApplicationUser>> GetClientsAsync(long trainerId)
        {
            var sql = $@"
SELECT DISTINCT ui.chat_id AS {nameof(ApplicationUser.Id)},
       ui.email AS {nameof(ApplicationUser.Email)},
       ui.username AS {nameof(ApplicationUser.UserName)},
       ui.iam_coach AS {nameof(ApplicationUser.IAmCoach)},
       ui.send_regular_messages AS {nameof(ApplicationUser.SendRegularMessages)},
       ui.name AS {nameof(ApplicationUser.Name)},
       ui.is_onboarding_complete AS {nameof(ApplicationUser.IsOnboardingComplete)},
       ui.about_me AS {nameof(ApplicationUser.AboutMe)},
       ui.sprint_weeks_count AS {nameof(ApplicationUser.SprintWeeksCount)},
       ui.photo_url AS {nameof(ApplicationUser.PhotoUrl)},
       ui.password_hash AS {nameof(ApplicationUser.PasswordHash)}
FROM {_usersInfoTable} ui
JOIN {_usersAccessTable} ua ON ui.chat_id = ua.owner_user_id
WHERE ua.granted_user_id = @TrainerId";

            var clients = await _con.QueryAsync<ApplicationUser>(
                sql,
                new { TrainerId = trainerId });
            return clients.ToList();
        }

        public async Task<TrainerComment?> GetCommentAsync(long trainerId, long clientId)
        {
            var sql = $@"
SELECT 
    owner_user_id AS {nameof(TrainerComment.ClientId)},
    granted_user_id AS {nameof(TrainerComment.TrainerId)},
    trainer_comment AS {nameof(TrainerComment.CommentText)}
FROM {_usersAccessTable}
WHERE granted_user_id = @TrainerId
  AND owner_user_id = @ClientId";

            var comment = await _con.QueryFirstOrDefaultAsync<TrainerComment>(
                sql,
                new { TrainerId = trainerId, ClientId = clientId });
            return comment;
        }

        public async Task<ApplicationUser?> GetTrainerAsync(long clientId)
        {
            var sql = $@"
SELECT ui.chat_id AS {nameof(ApplicationUser.Id)},
       ui.email AS {nameof(ApplicationUser.Email)},
       ui.username AS {nameof(ApplicationUser.UserName)},
       ui.iam_coach AS {nameof(ApplicationUser.IAmCoach)},
       ui.send_regular_messages AS {nameof(ApplicationUser.SendRegularMessages)},
       ui.name AS {nameof(ApplicationUser.Name)},
       ui.is_onboarding_complete AS {nameof(ApplicationUser.IsOnboardingComplete)},
       ui.about_me AS {nameof(ApplicationUser.AboutMe)},
       ui.sprint_weeks_count AS {nameof(ApplicationUser.SprintWeeksCount)},
       ui.photo_url AS {nameof(ApplicationUser.PhotoUrl)},
       ui.password_hash AS {nameof(ApplicationUser.PasswordHash)}
FROM {_usersInfoTable} ui
JOIN {_usersAccessTable} ua ON ui.chat_id = ua.granted_user_id
WHERE ua.owner_user_id = @ClientId";

            var trainer = await _con.QueryFirstOrDefaultAsync<ApplicationUser>(
                sql,
                new { ClientId = clientId });
            return trainer;
        }

        public async Task<ApplicationUser?> GetTrainerByUserNameAsync(string userName)
        {
            if (userName.StartsWith("@"))
                userName = userName.Substring(1);
            var sql = $@"
SELECT chat_id AS {nameof(ApplicationUser.Id)},
    email AS {nameof(ApplicationUser.Email)},
    username AS {nameof(ApplicationUser.UserName)},
    iam_coach AS {nameof(ApplicationUser.IAmCoach)},
    send_regular_messages AS {nameof(ApplicationUser.SendRegularMessages)},
    name AS {nameof(ApplicationUser.Name)},
    is_onboarding_complete AS {nameof(ApplicationUser.IsOnboardingComplete)},
    onboarding AS {nameof(ApplicationUser.Onboarding)},
    about_me AS {nameof(ApplicationUser.AboutMe)},
    sprint_weeks_count AS {nameof(ApplicationUser.SprintWeeksCount)},
    photo_url AS {nameof(ApplicationUser.PhotoUrl)},
    password_hash AS {nameof(ApplicationUser.PasswordHash)}
FROM {_usersInfoTable}
WHERE (username = @UserName OR username = @UserNameWithAt) AND iam_coach = true";

            var user = await _con.QueryFirstOrDefaultAsync<ApplicationUser>(
                sql,
                new { UserName = userName, UserNameWithAt = "@" + userName });

            return user;
        }

        public async Task DeleteTrainerAccessAsync(long clientId)
        {
            var deleteTrainerAccessSql = $@"
DELETE FROM {_usersAccessTable}
WHERE owner_user_id = @ClientId";
            await _con.ExecuteAsync(deleteTrainerAccessSql,
                new { ClientId = clientId });
        }

        public async Task AddTrainerAccessAsync(long trainerId, long clientId)
        {
            var sql = $@"
UPSERT INTO {_usersAccessTable}
(
    granted_user_id,
    owner_user_id,
    permission_id,
    sheet_id
)
VALUES
(
    @TrainerId,
    @ClientId,
    '0',
    '0'
)";

            await _con.ExecuteAsync(sql,
                new
                {
                    TrainerId = trainerId,
                    ClientId = clientId
                });
        }

        public async Task<bool> HasAccessAsync(long treinerId, long clientId)
        {
            var sql = $@"
SELECT COUNT(1)
FROM {_usersAccessTable}
WHERE granted_user_id = @treinerId
    AND owner_user_id = @clientId";

            var count = await _con.ExecuteScalarAsync<int>(sql, new { clientId, treinerId});
            return count > 0;
        }

        public async Task<bool> HasProjectAccessAsync(long trainerId, long clientId, Guid clientProjectId)
        {
            var sql = $@"
SELECT COUNT(1)
FROM {_projectsTable} p
JOIN {_usersAccessTable} ua
    ON p.chat_id = ua.owner_user_id
WHERE p.project_id = @ProjectId
    AND ua.owner_user_id = @ClientId
    AND ua.granted_user_id = @TrainerId";

            var count = await _con.ExecuteScalarAsync<int>(
                sql,
                new { TrainerId = trainerId, ClientId = clientId, ProjectId = clientProjectId }
            );

            return count > 0;
        }

        public async Task<bool> HasTaskAccessAsync(long trainerId, long clientId, Guid clientTaskId)
        {
            var sql = $@"
SELECT COUNT(1)
FROM {_tasksTable} t
JOIN {_projectsTable} p
    ON t.project_id = p.project_id
JOIN {_usersAccessTable} ua
    ON p.chat_id = ua.owner_user_id
WHERE t.task_id = @TaskId
    AND ua.owner_user_id = @ClientId
    AND ua.granted_user_id = @TrainerId";

            var count = await _con.ExecuteScalarAsync<int>(
                sql,
                new { TrainerId = trainerId, ClientId = clientId, TaskId = clientTaskId }
            );

            return count > 0;
        }

        public async Task<bool> UpdateCommentAsync(TrainerComment comment)
        {
            var sql = $@"
UPDATE {_usersAccessTable}
SET trainer_comment = @CommentText
WHERE granted_user_id = @TrainerId
    AND owner_user_id = @ClientId
RETURNING owner_user_id";

            var updatedClientId = await _con.ExecuteScalarAsync<string>(
                sql,
                new
                {
                    CommentText = comment.CommentText,
                    TrainerId = comment.TrainerId,
                    ClientId = comment.ClientId
                });

            return !string.IsNullOrEmpty(updatedClientId);
        }
    }
}
