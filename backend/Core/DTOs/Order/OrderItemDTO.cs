using System.ComponentModel.DataAnnotations;

public class OrderItemDTO
{
    [Required] public int ProductId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}