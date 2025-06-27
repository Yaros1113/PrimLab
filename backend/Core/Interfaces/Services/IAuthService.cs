using Core.DTOs.Auth;
using Core.Models;

namespace Core.Interfaces.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(string username, string password);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string? refreshToken);
}