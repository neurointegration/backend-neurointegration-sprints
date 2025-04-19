namespace Service.Dto
{
    public class UserUpdateRequest
    {
        public string? FirstName { get; set; }
        public string? AboutMe { get; set; }
        public bool? IsOnboardingComplete { get; set; }
        public Dictionary<string, bool>? Onboarding { get; set; }
        public int? SprintWeeksCount { get; set; }
    }
}
