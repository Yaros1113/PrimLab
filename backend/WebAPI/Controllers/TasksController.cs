using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RequireUserRole")]
public class TasksController : ControllerBase
{
    private readonly OrderTaskService _service;

    public TasksController(OrderTaskService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create(OrderTaskCreateDTO dto)
    {
        var task = await _service.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string search = "",
        [FromQuery] string sortBy = "CreatedDate",
        [FromQuery] bool ascending = false)
    {
        var tasks = await _service.GetAllTasksAsync(page, pageSize, search, sortBy, ascending);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var task = await _service.GetTaskByIdAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderTaskUpdateDTO dto)
    {
        var task = await _service.UpdateTaskAsync(id, dto);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteTaskAsync(id);
        return result ? NoContent() : NotFound();
    }
}