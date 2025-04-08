using Dapper;
using System.Data;

namespace Data.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly IDbConnection _con;
        private readonly string _usersInfoTable = "users_info";
        private readonly string _usersAccessTable = "users_access";

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
