namespace Data
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string TokenHash { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }   
}
