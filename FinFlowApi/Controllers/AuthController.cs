
using Microsoft.AspNetCore.Mvc;

using FinFlowApi.DTOs;
using FinFlowApi.Middleware;
using FinFlowApi.Services;


namespace FinFlowApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserService _userService;

    public AuthController(IUserService userService, JwtSettings jwtSettings)
    {
        _userService = userService;
        _jwtSettings = jwtSettings;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        bool isValidUser = await _userService.CheckUserExists(request.Username, request.Password);

        if (!isValidUser)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = JwtTokenGenerator.GenerateToken(request.Username, _jwtSettings);

        return Ok(new { token });
    }

}