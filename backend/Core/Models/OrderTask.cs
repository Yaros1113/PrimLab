using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class OrderTask
{
    public int Id { get; set; } // PK
    public int OrderId { get; set; } // FK → Order.Id
    [Required] public required string Title { get; set; } // Заголовок
    public string? Description { get; set; } // Описание
    public bool Status { get; set; } = false; // Статус (false = не выполнено)
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Дата создания
    [Required] public required string StoreAddress { get; set; } // Адрес магазина
    public Order? Order { get; set; } // Навигация
}