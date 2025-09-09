namespace Postly.WebAPI.Models
{
    public sealed class User
    {
        public User()
        {
            Id = Guid.CreateVersion7();
        }
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string Ad { get; set; } = default!;
        public string Soyad { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string SifreHash {get; set; } = default!;
        public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;
    }
}
