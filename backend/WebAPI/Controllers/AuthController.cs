using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using Core.DTOs.Auth;
using Core.Interfaces.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginRequest request)
    {
        var user = await _authService.RegisterAsync(request.Username, request.Password);
        return Ok(new { user.Id, user.Username, user.Role });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response); // Куки уже установлены в сервисе
    }
}