using System.ComponentModel.DataAnnotations;

public class UpdateTaskStatusDTO
{
    [Required] public bool Status { get; set; }
}