using Events.Application.Common.Interfaces;
using Events.Application.CQRS.Users.Commands.CreateUser;
using Events.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthController(
        IMediator mediator,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _mediator = mediator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) return BadRequest();
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByEmail(request.Email);
        if (user == null) throw new NotFoundException("User with email", request.Email);

        if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
            throw new ValidationException("Invalid password");

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken(user);

        SetTokenCookies(accessToken, refreshToken);

        return Ok(new { message = "Successfully logged in" });
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken) || !_jwtService.ValidateRefreshToken(refreshToken))
            return Unauthorized(new { message = "Invalid refresh token" });

        var userId = _jwtService.GetUserIdFromToken(refreshToken);
        if (!userId.HasValue)
            return Unauthorized(new { message = "Invalid refresh token" });

        var user = await _userRepository.Get(userId.Value);
        if (user == null)
            return Unauthorized(new { message = "User not found" });

        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken(user);

        SetTokenCookies(newAccessToken, newRefreshToken);

        return Ok(new { message = "Tokens refreshed successfully" });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

        return Ok(new { message = "Successfully logged out" });
    }

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("accessToken", accessToken, accessTokenOptions);
        Response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);
    }
}

public record LoginRequest(string Email, string Password); 