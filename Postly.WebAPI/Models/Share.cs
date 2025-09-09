namespace Postly.WebAPI.Models;
public sealed class Share
{
    public Guid Id { get; set; }
    public string Icerik { get; set; } = default!;
    public string? IcerikResimUrl { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public DateTime PaylasimTarihi { get; set; } = DateTime.UtcNow;
}
