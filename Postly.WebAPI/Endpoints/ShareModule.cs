using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postly.WebAPI.Context;
using Postly.WebAPI.Dtos;
using Postly.WebAPI.Models;
using TS.Endpoints;
using TS.Result;

namespace Postly.WebAPI.Endpoints;
public class ShareModule : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/shares").WithTags("Shares");
        app.MapGet(string.Empty, async (ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var res = await dbContext.Shares
            .Include(p => p.User)
            .OrderByDescending(p => p.PaylasimTarihi)
            .ToListAsync(cancellationToken);
            return res;
        })
         .Produces<List<Share>>();

        app.MapDelete("{id}", async (Guid id, ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var share = await dbContext.Shares.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (share == null)
                return Result<string>.Failure("Paylaşım bulunamadı");

            dbContext.Remove(share);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<string>.Succeed("Paylaşım silindi");
        })
         .Produces<Result<string>>();

        app.MapPost(string.Empty, async ([FromForm] CreateShareDto request, ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
        {
            string? fileName = null; 

            if (request.IcerikResim != null)
            {
                fileName = DateTime.Now.ToFileTime() + "_" + request.IcerikResim.FileName;
                using (var stream = new FileStream($"wwwroot/{fileName}", FileMode.Create))
                {
                    await request.IcerikResim.CopyToAsync(stream);
                }
            }

            Share share = new Share
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Icerik = request.Icerik,
                IcerikResimUrl = fileName, 
                PaylasimTarihi = DateTime.UtcNow
            };

            dbContext.Add(share);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Paylaşım başarıyla oluşturuldu.");
        })
         .Produces<Result<string>>()
         .DisableAntiforgery();
    }
}
