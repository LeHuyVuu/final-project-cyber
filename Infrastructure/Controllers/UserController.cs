using cybersoft_final_project.Context;
using cybersoft_final_project.Entities;
using cybersoft_final_project.Models;
using cybersoft_final_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(MyDbContext context, UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10, string? sortBy = "userid", string? sortOrder = "asc")
    {
        try
        {
            var (users, totalItems) = await _service.GetUsers(page, pageSize, sortBy, sortOrder);
            if (!users.Any())
                return NotFound(HTTPResponse<object>.Response(404, "No users found.", null));

            var pagination = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            return Ok(HTTPResponse<object>.Response(200, "Users retrieved successfully.", new { Users = users, Pagination = pagination }));
        }
        catch (Exception ex)
        {
            return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] user updatedUser)
    {
        try
        {
            if (updatedUser == null || id != updatedUser.userid)
                return BadRequest(HTTPResponse<object>.Response(400, "Invalid user data or ID mismatch.", null));

            var user = await _service.GetById(id);
            if (user == null)
                return NotFound(HTTPResponse<object>.Response(404, $"User with ID {id} not found.", null));

            if (!string.IsNullOrEmpty(updatedUser.phone))
            {
                if (updatedUser.phone.Length > 20)
                    return BadRequest(HTTPResponse<object>.Response(400, "Phone number exceeds max length.", null));

                if (await _service.PhoneExists(updatedUser.phone, id))
                    return BadRequest(HTTPResponse<object>.Response(400, "Phone number already exists.", null));
            }

            await _service.UpdateUser(user, updatedUser);
            return Ok(HTTPResponse<user>.Response(200, "User updated successfully.", user));
        }
        catch (Exception ex)
        {
            return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
        }
    }
    
    
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(HTTPResponse<object>.Response(401, "Invalid or missing token.", null));

            var user = await _service.GetById(userId);
            if (user == null)
                return NotFound(HTTPResponse<object>.Response(404, "User not found.", null));

            return Ok(HTTPResponse<user>.Response(200, "User profile retrieved successfully.", user));
        }
        catch (Exception ex)
        {
            return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
        }
    }

}
