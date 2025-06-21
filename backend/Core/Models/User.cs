namespace Core.Models;

public class User
{
    public int Id { get; set; } // PK
    public string Username { get; set; } // Логин
    public string PasswordHash { get; set; } // Хеш пароля (BCrypt)
    public string Role { get; set; } // "admin" или "user"

    public User()
    {
        Username = string.Empty;
        PasswordHash = string.Empty;
        Role = "user"; // Значение по умолчанию
    }
}