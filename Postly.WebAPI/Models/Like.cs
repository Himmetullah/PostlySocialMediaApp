namespace Postly.WebAPI.Models;
    public sealed class Like
    {
        public Like()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShareId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = default!;
        public Share Share { get; set; } = default!;
}
