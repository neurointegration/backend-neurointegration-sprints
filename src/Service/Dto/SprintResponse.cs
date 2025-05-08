using Data;

namespace Service.Dto
{
    public class SprintResponse
    {
        public long Number { get; set; }
        public long UserId { get; set; }
        public int WeeksCount { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Dictionary<int, SprintWeek> Weeks { get; set; } = default!;
    }
}
