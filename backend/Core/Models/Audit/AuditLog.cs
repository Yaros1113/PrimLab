using System.ComponentModel.DataAnnotations;

namespace Core.Models.Audit;

public class AuditLog
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public required string EntityType { get; set; }  // Тип сущности (например, "Client")
    
    [Required]
    public required string ActionType { get; set; }  // "Create", "Update", "Delete"
    
    public string? EntityId { get; set; }    // ID изменяемой сущности
    
    [Required]
    public required string AuditData { get; set; }   // JSON с деталями изменений
    
    [Required]
    public DateTime AuditDate { get; set; } = DateTime.UtcNow;
    
    public string? UserName { get; set; }    // Имя пользователя, выполнившего действие
}