namespace Core.Models;

public class Client
{
    public int Id { get; set; } // PK
    public string Name { get; set; } // Имя клиента
    public string Email { get; set; } // Email
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow; // Дата регистрации
    public int? UserId { get; set; } // Ссылка на пользователя (если есть)
    public User? User { get; set; } // Навигационное свойство
    public List<ClientPhone> Phones { get; set; } = new(); // Телефоны (1-ко-многим)
    public List<Order> Orders { get; set; } = new(); // Заказы (1-ко-многим)

    public Client()
    {
        Name = string.Empty;
        Email = string.Empty;
    }
}