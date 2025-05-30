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
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CategoryController(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/category (with pagination and sorting)
        [HttpGet]
        public async Task<IActionResult> GetCategories(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "categoryid",
            [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var query = _context.categories.AsQueryable();

                query = sortBy?.ToLower() switch
                {
                    "categoryname" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(c => c.categoryname)
                        : query.OrderBy(c => c.categoryname),
                    _ => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(c => c.categoryid)
                        : query.OrderBy(c => c.categoryid)
                };

                var totalItems = await query.CountAsync();
                var categories = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (categories == null || !categories.Any())
                {
                    return NotFound(HTTPResponse<object>.Response(404, "No categories found.", null));
                }

                var paginationInfo = new
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return Ok(HTTPResponse<object>.Response(200, "Categories retrieved successfully.",
                    new { Categories = categories, Pagination = paginationInfo }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _context.categories
                    .FirstOrDefaultAsync(c => c.categoryid == id);

                if (category == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Category with ID {id} not found.", null));
                }

                return Ok(HTTPResponse<category>.Response(200, "Category retrieved successfully.", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] category newCategory)
        {
            try
            {
                if (newCategory == null || string.IsNullOrEmpty(newCategory.categoryname))
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Invalid category data.", null));
                }

                var existingCategory = await _context.categories
                    .AnyAsync(c => c.categoryname == newCategory.categoryname);
                if (existingCategory)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Category name already exists.", null));
                }

                newCategory.created_at = DateTime.Now;

                _context.categories.Add(newCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = newCategory.categoryid },
                    HTTPResponse<category>.Response(201, "Category created successfully.", newCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] category updatedCategory)
        {
            try
            {
                if (updatedCategory == null || id != updatedCategory.categoryid || string.IsNullOrEmpty(updatedCategory.categoryname))
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Invalid category data or ID mismatch.", null));
                }

                var category = await _context.categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Category with ID {id} not found.", null));
                }

                var existingCategory = await _context.categories
                    .AnyAsync(c => c.categoryname == updatedCategory.categoryname && c.categoryid != id);
                if (existingCategory)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Category name already exists.", null));
                }

                category.categoryname = updatedCategory.categoryname;

                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<category>.Response(200, "Category updated successfully.", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Category with ID {id} not found.", null));
                }

                var hasProducts = await _context.products.AnyAsync(p => p.categoryid == id);
                if (hasProducts)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Cannot delete category with associated products.", null));
                }

                _context.categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<category>.Response(200, "Category deleted successfully.", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }
    }
}