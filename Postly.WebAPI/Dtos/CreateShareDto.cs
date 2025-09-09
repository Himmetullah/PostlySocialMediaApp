namespace Postly.WebAPI.Dtos;
public class CreateShareDto
{
    public Guid UserId { get; set; }
    public string Icerik { get; set; } = default!;
    public IFormFile? IcerikResim { get; set; }

}

public record struct ShareUpdateDto
(
    Guid Id,
    string Icerik,
    IFormFile? IcerikResim
);

public record struct ShareResultDto
(
    Guid Id,
    Guid UserId,
    string Icerik,
    string? IcerikResim,
    DateTime PaylasimTarihi,
    string? UserAdSoyad
);
