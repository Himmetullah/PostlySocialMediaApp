namespace Postly.WebAPI.Dtos;

public class CreateLikeDto
{
    public Guid UserId { get; set; }
    public Guid ShareId { get; set; }
}
public record struct LikeResultDto
(
    Guid Id,
    Guid UserId,
    Guid ShareId,
    DateTime LikedAt,
    string? UserAdSoyad
);
