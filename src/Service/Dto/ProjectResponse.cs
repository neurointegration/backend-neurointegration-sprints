using Data;

namespace Service.Dto
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public SectionName SectionName { get; set; }
        public Dictionary<String, PlanningTime>? PlanningTimes { get; set; }
        public Dictionary<String, FactTime>? FactTimes { get; set; }
    }
}
