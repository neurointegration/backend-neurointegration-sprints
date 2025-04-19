using Dapper;
using System.Data;
using System.Text.Json;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _con;
        private readonly string _usersTable = "users_info";

        public UserRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(long userId)
        {var sql = $@"
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
FROM {_usersTable}
WHERE chat_id = @UserId";

            var user = await _con.QueryFirstOrDefaultAsync<ApplicationUser>(
                sql,
                new { UserId = userId });

            return user;
        }

        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
        {
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
FROM {_usersTable}
WHERE username = @UserName";

            var user = await _con.QueryFirstOrDefaultAsync<ApplicationUser>(
                sql,
                new { UserName = userName});

            return user;
        }

        public async Task<ApplicationUser> UpsertAsync(ApplicationUser user)
        {
            var sql = $@"
UPSERT INTO {_usersTable}
(
    chat_id,
    email,
    username,
    iam_coach,
    send_regular_messages,
    name,
    is_onboarding_complete,
    onboarding,
    about_me,
    sprint_weeks_count,
    photo_url,
    password_hash
)
VALUES 
(
    @Id,
    @Email,
    @UserName,
    @IAmCoach,
    @SendRegularMessages,
    @Name,
    @IsOnboardingComplete,
    @Onboarding,
    @AboutMe,
    @SprintWeeksCount,
    @PhotoUrl,
    @PasswordHash
)";
            await _con.ExecuteAsync(sql,
                new
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    IAmCoach = user.IAmCoach,
                    SendRegularMessages = user.SendRegularMessages,
                    Name = user.Name,
                    IsOnboardingComplete = user.IsOnboardingComplete,
                    Onboarding = JsonSerializer.Serialize(user.Onboarding),
                    AboutMe = user.AboutMe,
                    SprintWeeksCount = user.SprintWeeksCount,
                    PhotoUrl = user.PhotoUrl,
PasswordHash = user.PasswordHash
                });
            return user;
        }
    }
}
