using System.ComponentModel.DataAnnotations;

public class OrderCreateDTO
{
    [Required] public int ClientId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveryDate { get; set; }
    public List<OrderItemDTO> Items { get; set; } = new();
}