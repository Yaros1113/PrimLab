using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using Core.DTOs.Auth;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request.Username, request.Password);
        return Ok(new { message = "Registration successful" });
    }

    public class RegisterRequest
    {
        [Required] public required string Username { get; set; }
        [Required] public required string Password { get; set; }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response); // Куки уже установлены в сервисе
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _authService.RefreshTokenAsync(refreshToken);
        return Ok(response);
    }
}