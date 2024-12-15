using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public ApplicationUser(string userName) : this()
        {
            UserName = userName;
        }

        public string? FirstName { get; set; }
        public bool IsOnboardingComplete { get; set; }
        public string? LastName { get; set; }
        public string? AboutMe { get; set; }
        public int SprintWeeksCount { get; set; }
        public string? TelegramId { get; set; }
        public string? PhotoUrl { get; set; }

        public Guid? TrainerId { get; set; }
        public ApplicationUser? Trainer { get; set; }

        public IList<ApplicationUser> Clients { get; set; } = default!;
    }
}
