namespace Data
{
    public class Sprint
    {
        public long Number { get; set; }
        public int? WeeksCount { get; set; }
        public DateOnly BeginDate { get; set; }
        public string SheetId { get; set; } = string.Empty;
        public long UserId { get; set; }
    }
}
