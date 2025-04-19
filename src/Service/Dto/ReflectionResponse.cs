using Data;

namespace Service.Dto
{
    public class ReflectionResponse
    {
        public SprintAnswerType AnswerType { get; set; }
        public long SprintNumber { get; set; }
        public DateOnly Date { get; set; }
        public string Answer { get; set; } = default!;
    }
}
