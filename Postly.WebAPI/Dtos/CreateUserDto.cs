namespace Postly.WebAPI.Dtos;
public record class CreateUserDto
(
    IFormFile File,
    string Ad,
    string Soyad,
    string Email,
    string Sifre
);

public record class UpdateUserDto
(
    Guid Id,
    IFormFile? File,
    string Ad,
    string Soyad,
    string Email,
    string Sifre
); 

public record class RegisterDto
(
    string Ad,
    string Soyad,
    string Email,
    string Sifre
);

public record class LoginDto
(
    string Email,
    string Sifre
);