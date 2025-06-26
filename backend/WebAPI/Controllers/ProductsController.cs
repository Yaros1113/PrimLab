using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RequireUserRole")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDTO dto)
    {
        var product = await _service.CreateProductAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string search = "",
        [FromQuery] string sortBy = "Name",
        [FromQuery] bool ascending = true)
    {
        var products = await _service.GetAllProductsAsync(page, pageSize, search, sortBy, ascending);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDTO dto)
    {
        var product = await _service.UpdateProductAsync(id, dto);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteProductAsync(id);
        return result ? NoContent() : NotFound();
    }
}