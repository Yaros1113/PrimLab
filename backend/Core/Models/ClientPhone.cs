namespace Core.Models;

public class ClientPhone
{
    public int Id { get; set; } // PK
    public string? PhoneNumber { get; set; } // Номер телефона
    public int ClientId { get; set; } // FK → Client.Id
    public Client? Client { get; set; } // Навигационное свойство
}