﻿using cybersoft_final_project.Infrastructure.Services;
using cybersoft_final_project.Models;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace cybersoft_final_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Đăng nhập và trả về JWT nếu thành công
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(HTTPResponse<object>.Response(400, "Dữ liệu không hợp lệ", null));
        }

        var result = await _authService.LoginAsync(request.Username, request.Password);

        if (!result.Success)
        {
            return Unauthorized(HTTPResponse<object>.Response(401, result.Message, null));
        }

        return Ok(HTTPResponse<object>.Response(200, result.Message, new
        {
            Token = result.Token
        }));
    }

    /// <summary>
    /// Đăng xuất - chỉ hướng dẫn xóa token phía client
    /// </summary>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(HTTPResponse<object>.Response(200, "Đăng xuất thành công. Hãy xóa token ở phía client.", null));
    }

    /// <summary>
    /// Đăng ký tài khoản mới
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(HTTPResponse<object>.Response(400, "Dữ liệu không hợp lệ", null));
        }

        var (success, message) = await _authService.RegisterAsync(request.Username, request.Password, request.Email);

        if (!success)
        {
            return BadRequest(HTTPResponse<object>.Response(400, message, null));
        }

        return Ok(HTTPResponse<object>.Response(200, message, new
        {
            Username = request.Username,
            Email = request.Email
        }));
    }
}
