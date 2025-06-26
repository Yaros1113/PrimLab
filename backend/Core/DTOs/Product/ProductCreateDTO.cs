using System.ComponentModel.DataAnnotations;

public class ProductCreateDTO
{
    [Required] public string? Name { get; set; }
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue)] public decimal Price { get; set; }
}