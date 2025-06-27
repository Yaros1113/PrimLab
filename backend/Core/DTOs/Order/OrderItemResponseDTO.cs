public class OrderItemResponseDTO
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtOrder { get; set; }
}