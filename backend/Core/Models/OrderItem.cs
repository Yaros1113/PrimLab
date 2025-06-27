using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class OrderItem
{
    public int Id { get; set; } // PK
    public int OrderId { get; set; } // FK → Order.Id
    public int ProductId { get; set; } // FK → Product.Id
    public int Quantity { get; set; } // Количество
    public decimal PriceAtOrder { get; set; } // Цена на момент заказа
    [ForeignKey("OrderId")] public Order? Order { get; set; } // Навигация
    [ForeignKey("ProductId")] public Product? Product { get; set; } // Навигация
}