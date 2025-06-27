public class OrderResponseDTO
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public List<OrderItemResponseDTO> Items { get; set; } = new();
}