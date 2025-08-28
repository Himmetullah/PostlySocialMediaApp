using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postly.WebAPI.Context;
using Postly.WebAPI.Dtos;
using Postly.WebAPI.Models;
using TS.Result;

namespace Postly.WebAPI.Controllers;

[ApiController]
[Route("/users")]
public sealed class UsersController(ApplicationDbContext dbContext)
{
    [HttpPost]
    public async Task<Result<string>> Create([FromForm] CreateUserDto request, CancellationToken cancellationToken)
    {
        var fileName = DateTime.Now.ToFileTime() + "_" + request.File.FileName;
        using (var stream = new FileStream($"wwwroot/{fileName}", FileMode.Create))
        {
            request.File.CopyTo(stream);
        }

        User user = request.Adapt<User>();
        user.ImageUrl = fileName;
        user.SifreHash = BCrypt.Net.BCrypt.HashPassword(request.Sifre);
        dbContext.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kullanıcı başarıyla oluşturuldu.");

    }

    [HttpPut]
    public async Task<Result<string>> Update([FromForm] UpdateUserDto request, CancellationToken cancellationToken)
    {

        User? user = await dbContext.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        string? fileName = null;
        if (request.File is not null)
        {
            fileName = DateTime.Now.ToFileTime() + "_" + request.File.FileName;
            using (var stream = new FileStream($"wwwroot/{fileName}", FileMode.Create))
            {
                request.File.CopyTo(stream);
            }

            File.Delete("wwwroot/" + user.ImageUrl);
        }

        request.Adapt(user);

        if (fileName is not null)
        {
            user.ImageUrl = fileName;
        }
        dbContext.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kullanıcı bilgileri başarıyla güncellendi.");

    }

}
