namespace Core.Models;

public class Product
{
    public int Id { get; set; } // PK
    public string Name { get; set; } // Название
    public string? Description { get; set; } // Описание (необязательное)
    public decimal Price { get; set; } // Цена
    public List<OrderItem> OrderItems { get; set; } = new(); // Связь с заказами

    public Product()
    {
        Name = string.Empty;
    }
}