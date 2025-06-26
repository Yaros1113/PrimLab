using Core.Models;
using DAL.Data;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context) => _context = context;

    public async Task<ProductResponseDTO> CreateProductAsync(ProductCreateDTO dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return MapToDTO(product);
    }

    public async Task<PagedList<ProductResponseDTO>> GetAllProductsAsync(
        int page = 1,
        int pageSize = 10,
        string search = "",
        string sortBy = "Name",
        bool ascending = true)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

        // Сортировка
        switch (sortBy.ToLower())
        {
            case "name":
                query = ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                break;
            case "price":
                query = ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                break;
            default:
                query = query.OrderBy(p => p.Name);
                break;
        }

        return await PagedList<ProductResponseDTO>.CreateAsync(
            query.Select(p => MapToDTO(p)),
            page,
            pageSize
        );
    }

    public async Task<ProductResponseDTO?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product == null ? null : MapToDTO(product);
    }

    public async Task<ProductResponseDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return MapToDTO(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductResponseDTO MapToDTO(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price
    };
}