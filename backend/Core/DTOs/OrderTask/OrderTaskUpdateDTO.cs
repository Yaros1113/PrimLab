using System.ComponentModel.DataAnnotations;

public class OrderTaskUpdateDTO
{
    [Required] public required string Title { get; set; }
    public string? Description { get; set; }
    [Required] public string? StoreAddress { get; set; }
    public bool Status { get; set; }
}