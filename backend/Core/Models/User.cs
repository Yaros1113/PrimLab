namespace Core.Models;

public class User
{
    public int Id { get; set; } // PK
    public string Username { get; set; } = null!; // Логин
    public string PasswordHash { get; set; } = null!; // Хеш пароля (BCrypt)
    public string Role { get; set; } = "user"; // "admin" или "user"
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}