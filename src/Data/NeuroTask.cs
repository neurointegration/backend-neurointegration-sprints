namespace Data
{
    public class NeuroTask
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public SectionName SectionName { get; set; }
        public string Title { get; set; } = default!;
        public Dictionary<string, PlanningTime>? PlanningTimes { get; set; }
        public Dictionary<string, FactTime>? FactTimes { get; set; }
    }
}
