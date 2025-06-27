public class OrderTaskResponseDTO
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? StoreAddress { get; set; }
}