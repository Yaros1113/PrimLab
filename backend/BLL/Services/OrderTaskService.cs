using Core.Models;
using BLL.Data;
using Microsoft.EntityFrameworkCore;

public class OrderTaskService
{
    private readonly AppDbContext _context;

    public OrderTaskService(AppDbContext context) => _context = context;

    public async Task<OrderTaskResponseDTO> CreateTaskAsync(OrderTaskCreateDTO dto)
    {
        var task = new OrderTask
        {
            OrderId = dto.OrderId,
            Title = dto.Title,
            Description = dto.Description,
            Status = false,
            CreatedDate = DateTime.UtcNow,
            StoreAddress = dto.StoreAddress
        };
        
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return MapToDTO(task);
    }

    public async Task<PagedList<OrderTaskResponseDTO>> GetAllTasksAsync(
        int page = 1,
        int pageSize = 10,
        string search = "",
        string sortBy = "CreatedDate",
        bool ascending = false)
    {
        var query = _context.Tasks
            .Include(t => t.Order)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));

        switch (sortBy.ToLower())
        {
            case "createddate":
                query = ascending ? query.OrderBy(t => t.CreatedDate) : query.OrderByDescending(t => t.CreatedDate);
                break;
            case "status":
                query = ascending ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status);
                break;
            default:
                query = query.OrderByDescending(t => t.CreatedDate);
                break;
        }

        return await PagedList<OrderTaskResponseDTO>.CreateAsync(
            query.Select(t => MapToDTO(t)),
            page,
            pageSize
        );
    }

    public async Task<OrderTaskResponseDTO?> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Order)
            .FirstOrDefaultAsync(t => t.Id == id);
        return task == null ? null : MapToDTO(task);
    }

    public async Task<OrderTaskResponseDTO?> UpdateTaskAsync(int id, OrderTaskUpdateDTO dto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.StoreAddress = dto.StoreAddress;

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return MapToDTO(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<OrderTaskResponseDTO> UpdateTaskStatusAsync(int id, bool status)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return null;

        task.Status = status;
        await _context.SaveChangesAsync();
        
        return MapToDTO(task);
    }

    private static OrderTaskResponseDTO MapToDTO(OrderTask task) => new()
    {
        Id = task.Id,
        OrderId = task.OrderId,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        CreatedDate = task.CreatedDate,
        StoreAddress = task.StoreAddress
    };
}