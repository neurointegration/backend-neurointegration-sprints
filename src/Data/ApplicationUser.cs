namespace Data
{
    public class ApplicationUser
    {
        public long Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool IAmCoach { get; set; }
        public bool SendRegularMessages { get; set; }
        public string? Name { get; set; }
        public bool IsOnboardingComplete { get; set; }
        public Dictionary<string, bool>? Onboarding { get; set; } = default!;
        public string? AboutMe { get; set; }
        public int SprintWeeksCount { get; set; }
        public string? PhotoUrl { get; set; }
        public string? PasswordHash { get; set; }
    }
}
