using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RequireUserRole")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateDTO dto)
    {
        var order = await _service.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string search = "",
        [FromQuery] string sortBy = "OrderDate",
        [FromQuery] bool ascending = false)
    {
        var orders = await _service.GetAllOrdersAsync(page, pageSize, search, sortBy, ascending);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await _service.GetOrderByIdAsync(id);
        return order == null ? NotFound() : Ok(order);
    }
}