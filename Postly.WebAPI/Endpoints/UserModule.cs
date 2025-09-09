using Mapster;
using Microsoft.EntityFrameworkCore;
using Postly.WebAPI.Context;
using Postly.WebAPI.Dtos;
using Postly.WebAPI.Models;
using TS.Endpoints;
using TS.Result;

namespace Postly.WebAPI.Endpoints
{
    public sealed class UserModule : IEndpoint
    {
        public void AddRoutes(IEndpointRouteBuilder builder)
        {
            var app = builder.MapGroup("/users").WithTags("Users");
            app.MapGet(string.Empty, async (ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var res = await dbContext.Users
                .OrderBy(p => p.Ad)
                .ToListAsync(cancellationToken);

                return res;
            })
            .Produces<List<User>>();

            app.MapGet("{id}", async (string id, ApplicationDbContext dbContext) =>
            {
                if (!Guid.TryParse(id, out var guid))
                    return Results.BadRequest("Geçersiz Id");

                var user = await dbContext.Users.FirstOrDefaultAsync(p => p.Id == guid);

                if (user is null)
                    return Results.NotFound();
                return Results.Ok(user);
            })
            .Produces<User>();


            app.MapDelete("{id}", async (Guid id, ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
                if (user == null)
                {
                    return Result<string>.Failure("Kullanıcı bulunamadı");
                }

                dbContext.Remove(user);
                await dbContext.SaveChangesAsync(cancellationToken);
                return Result<string>.Succeed("Kullanıcı silindi");
            })
                .Produces<Result<string>>();

            app.MapPost("register", async (RegisterDto register, ApplicationDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var mevcutUser = await dbContext.Users.FirstOrDefaultAsync(p => p.Email == register.Email, cancellationToken);
                if (mevcutUser is not null)
                    return Result<string>.Failure("Bu email zaten kayıtlı.");

                var user = register.Adapt<User>();
                user.ImageUrl = "profile.png";
                user.SifreHash = BCrypt.Net.BCrypt.HashPassword(register.Sifre);
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Result<string>.Succeed("Kaydınız başarıyla oluşturuldu. Giriş yapabilirsiniz.");
            });

            app.MapPost("login", async (LoginDto login, ApplicationDbContext dbContext, HttpContext httpContext, CancellationToken cancellationToken) =>
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(p => p.Email == login.Email, cancellationToken);

                if (user is null)
                    return Result<string>.Failure("Kullanıcı bulunamadı.");

                bool sifreDogruMu = BCrypt.Net.BCrypt.Verify(login.Sifre, user.SifreHash);
                if (!sifreDogruMu)
                    return Result<string>.Failure("Şifre yanlış.");

                httpContext.Session.SetString("userId", user.Id.ToString());
                httpContext.Session.SetString("userEmail", user.Email);

                return Result<string>.Succeed(user.Id.ToString());
            });
        }
    }
}
