namespace Core.DTOs.Client;
public class ClientResponseDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int? UserId { get; set; }
    public List<string> PhoneNumbers { get; set; } = new();
}