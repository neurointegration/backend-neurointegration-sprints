namespace Service.Dto
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? AboutMe { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsOnboardingComplete { get; set; }
        public Dictionary<string, bool>? Onboarding { get; set; } = default!;
        public int SprintWeeksCount { get; set; }
    }
}
