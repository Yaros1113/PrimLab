using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RequireUserRole")]
public class ClientsController : ControllerBase
{
    private readonly ClientService _clientService;

    public ClientsController(ClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient(ClientCreateDTO dto)
    {
        var client = await _clientService.CreateClientAsync(dto);
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    [HttpGet]
    public async Task<IActionResult> GetClients(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string search = "")
    {
        var clients = await _clientService.GetAllClientsAsync(page, pageSize, search);
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, ClientUpdateDTO dto)
    {
        var client = await _clientService.UpdateClientAsync(id, dto);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await _clientService.DeleteClientAsync(id);
        return result ? NoContent() : NotFound();
    }
}