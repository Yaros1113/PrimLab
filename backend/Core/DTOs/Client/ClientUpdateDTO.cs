using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Client;

public class ClientUpdateDTO
{
    [Required]
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    public List<string> PhoneNumbers { get; set; } = new();
    public int? UserId { get; set; }
}