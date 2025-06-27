using System.ComponentModel.DataAnnotations;

public class OrderTaskCreateDTO
{
    [Required] public int OrderId { get; set; }
    [Required] public required string Title { get; set; }
    public string? Description { get; set; }
    [Required] public string? StoreAddress { get; set; }
}