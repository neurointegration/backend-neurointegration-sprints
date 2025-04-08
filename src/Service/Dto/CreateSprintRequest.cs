using Data;

namespace Service.Dto
{
    public class CreateSprintRequest
    {
        public int WeeksCount { get; set; }
        public DateOnly BeginDate { get; set; }
    }
}
