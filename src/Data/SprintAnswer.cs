namespace Data
{
    public class SprintAnswer
    {
        public SprintScenarioType ScenarioType { get; set; }
        public SprintAnswerType AnswerType { get; set; }
        public long SprintNumber { get; set; }
        public DateOnly Date { get; set; }
        public string Answer {  get; set; } = default!;
    }
}
