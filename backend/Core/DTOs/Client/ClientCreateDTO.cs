using System.ComponentModel.DataAnnotations;

public class ClientCreateDTO
{
    [Required] public string? Name { get; set; }
    [EmailAddress] public string? Email { get; set; }
    public int? UserId { get; set; } // Для связи с пользователем
    public List<string> PhoneNumbers { get; set; } = new();
}