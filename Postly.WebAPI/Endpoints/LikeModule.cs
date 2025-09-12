using Mapster;
using Microsoft.EntityFrameworkCore;
using Postly.WebAPI.Context;
using Postly.WebAPI.Dtos;
using Postly.WebAPI.Models;
using TS.Endpoints;
using TS.Result;

namespace Postly.WebAPI.Endpoints;

public class LikeModule : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/likes").WithTags("Likes");
        app.MapGet(string.Empty, async (ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var likes = await dbContext.Likes
            .Include(p => p.User)
            .OrderByDescending(p => p.LikedAt)
            .ToListAsync(cancellationToken);

            var resultDtos = likes.Adapt<List<LikeResultDto>>();
            return resultDtos;
        })
         .Produces<List<Like>>();

        app.MapDelete("{id}", async (Guid id, ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var like = await dbContext.Likes.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (like == null)
                return Results.NotFound("Beğeni geri alınamadı!!");
            dbContext.Remove(like);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Results.Ok("Beğeni geri alındı.");
        })
         .Produces<string>();

        app.MapPost(string.Empty, async (CreateLikeDto request, ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var like = request.Adapt<Like>();

            var BegeniVarmi = await dbContext.Likes
                .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.ShareId == request.ShareId, cancellationToken);

            if (BegeniVarmi != null)
                return Results.BadRequest("Bu paylaşımı zaten beğendiniz.");

                dbContext.Add(like);
            await dbContext.SaveChangesAsync(cancellationToken);

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == like.UserId, cancellationToken);
            var resultDto = like.Adapt<LikeResultDto>();
            resultDto = resultDto with { UserAdSoyad = user?.Ad + "" + user?.Soyad };

            return Results.Ok(resultDto);
        })
         .Produces<string>();
    }
}
