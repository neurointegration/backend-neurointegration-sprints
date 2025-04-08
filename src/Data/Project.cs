namespace Data
{
    public class Project
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public long SprintNumber { get; set; }
        public SectionName SectionName { get; set; }
        public string Title { get; set; } = default!;
        public Dictionary<string, PlanningTime>? PlanningTimes { get; set; }
        public Dictionary<string, FactTime>? FactTimes { get; set; }
    }
}
