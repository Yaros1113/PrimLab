namespace Core.Models;

public class Order
{
    public int Id { get; set; } // PK
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Дата заказа
    public DateTime? DeliveryDate { get; set; } // Дата доставки (необязательное)
    public int ClientId { get; set; } // FK → Client.Id
    public Client? Client { get; set; } // Клиент
    public List<OrderItem> OrderItems { get; set; } = new(); // Товары в заказе
    public List<OrderTask> Tasks { get; set; } = new(); // Задачи по заказу
}