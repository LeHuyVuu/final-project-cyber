using cybersoft_final_project.Context;
using Microsoft.AspNetCore.Mvc;
using cybersoft_final_project.Entities;
using cybersoft_final_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace cybersoft_final_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context = new MyDbContext();

        // GET: api/user (with pagination and sorting)
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "Userid",
            [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var query = _context.Tblusers
                    .Where(u => u.Status == true)
                    .AsQueryable();

                // Apply sorting using LINQ with switch expression
                query = sortBy?.ToLower() switch
                {
                    "fullname" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Fullname)
                        : query.OrderBy(u => u.Fullname),
                    "phone" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Phone)
                        : query.OrderBy(u => u.Phone),
                    _ => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Userid)
                        : query.OrderBy(u => u.Userid)
                };

                // Apply pagination
                var totalItems = await query.CountAsync();
                var users = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    return NotFound(HTTPResponse<object>.Response(404, "No users found.", null));
                }

                var paginationInfo = new
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return Ok(HTTPResponse<object>.Response(200, "Users retrieved successfully.",
                    new { Users = users, Pagination = paginationInfo }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Tbluser updatedUser)
        {
            try
            {
                if (updatedUser ==  null || id != updatedUser.Userid)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Invalid user data or ID mismatch.", null));
                }

                var user = await _context.Tblusers
                    .Where(u => u.Userid == id && u.Status == true)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"User with ID {id} not found.", null));
                }

                // Check if updated phone already exists (excluding current user)
                if (!string.IsNullOrEmpty(updatedUser.Phone))
                {
                    var existingPhone = await _context.Tblusers
                        .AnyAsync(u => u.Phone == updatedUser.Phone && u.Userid != id && u.Status == true);
                    if (existingPhone)
                    {
                        return BadRequest(HTTPResponse<object>.Response(400, "Phone number already exists.", null));
                    }
                }

                // Update properties
                user.Fullname = updatedUser.Fullname;
                user.Address = updatedUser.Address;
                user.Birthday = updatedUser.Birthday;
                user.Phone = updatedUser.Phone;
                user.Roleid = updatedUser.Roleid;
                // Note: Password updates should involve hashing; not implemented here
                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    user.Password = updatedUser.Password; // In practice, hash the password
                }
                // Note: If UpdatedAt field exists, set: user.UpdatedAt = DateTime.Now;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<Tbluser>.Response(200, "User updated successfully.", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }
    }
}