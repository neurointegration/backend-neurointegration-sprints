namespace Data
{
    public class Sprint
    {
        public Guid Id { get; set; }
        public int WeeksCount { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Weeks { get; set; } = default!;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public IList<Project> Projects { get; set; } = default!;
    }
}
