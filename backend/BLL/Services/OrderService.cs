using Core.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context) => _context = context;

    public async Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var client = await _context.Clients.FindAsync(dto.ClientId);
            if (client == null) throw new ArgumentException("Client not found");

            // Создаем заказ
            var order = new Order
            {
                ClientId = dto.ClientId,
                OrderDate = dto.OrderDate,
                DeliveryDate = dto.DeliveryDate
            };
            
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Добавляем элементы заказа
            foreach (var item in dto.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId)
                    ?? throw new Exception($"Product not found: {item.ProductId}");
                
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtOrder = product.Price
                });
            }
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return await GetOrderByIdAsync(order.Id);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PagedList<OrderResponseDTO>> GetAllOrdersAsync(
        int page = 1,
        int pageSize = 10,
        string search = "",
        string sortBy = "OrderDate",
        bool ascending = false)
    {
        var query = _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .AsQueryable();

        // Фильтрация по клиенту
        if (!string.IsNullOrEmpty(search))
            query = query.Where(o => o.Client.Name.Contains(search));

        // Сортировка
        switch (sortBy.ToLower())
        {
            case "orderdate":
                query = ascending ? query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate);
                break;
            case "deliverydate":
                query = ascending ? query.OrderBy(o => o.DeliveryDate) : query.OrderByDescending(o => o.DeliveryDate);
                break;
            default:
                query = query.OrderByDescending(o => o.OrderDate);
                break;
        }

        return await PagedList<OrderResponseDTO>.CreateAsync(
            query.Select(o => MapToDTO(o)),
            page,
            pageSize
        );
    }

    public async Task<OrderResponseDTO?> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        return order == null ? null : MapToDTO(order);
    }

    private static OrderResponseDTO MapToDTO(Order order) => new()
    {
        Id = order.Id,
        ClientId = order.ClientId,
        ClientName = order.Client.Name,
        OrderDate = order.OrderDate,
        DeliveryDate = order.DeliveryDate,
        Items = order.OrderItems.Select(oi => new OrderItemResponseDTO
        {
            ProductId = oi.ProductId,
            ProductName = oi.Product.Name,
            Quantity = oi.Quantity,
            PriceAtOrder = oi.PriceAtOrder
        }).ToList()
    };
}