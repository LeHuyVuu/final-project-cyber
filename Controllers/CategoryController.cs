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
        private readonly MyDbContext _context = new MyDbContext();

        // GET: api/category (with pagination and sorting)
        [HttpGet]
        public async Task<IActionResult> GetCategories(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "Categoryid",
            [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var query = _context.Tblcategories.AsQueryable();
                // Note: If Status field exists, add .Where(c => c.Status == true)

                // Apply sorting using LINQ with switch expression
                query = sortBy?.ToLower() switch
                {
                    "categoryname" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(c => c.Categoryname)
                        : query.OrderBy(c => c.Categoryname),
                    _ => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(c => c.Categoryid)
                        : query.OrderBy(c => c.Categoryid)
                };

                // Apply pagination
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
                var category = await _context.Tblcategories
                    .FirstOrDefaultAsync(c => c.Categoryid == id);
                    // Note: If Status field exists, add && c.Status == true

                if (category == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Category with ID {id} not found.", null));
                }

                return Ok(HTTPResponse<Tblcategory>.Response(200, "Category retrieved successfully.", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Tblcategory newCategory)
        {
            try
            {
                if (newCategory == null || string.IsNullOrEmpty(newCategory.Categoryname))
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Invalid category data.", null));
                }

                // Check if category name already exists
                var existingCategory = await _context.Tblcategories
                    .AnyAsync(c => c.Categoryname == newCategory.Categoryname);
                    // Note: If Status field exists, add && c.Status == true
                if (existingCategory)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Category name already exists.", null));
                }

                newCategory.CreatedAt = DateTime.Now;
                // Note: If Status and UpdatedAt fields exist, set:
                // newCategory.Status = true;
                // newCategory.UpdatedAt = DateTime.Now;

                _context.Tblcategories.Add(newCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = newCategory.Categoryid },
                    HTTPResponse<Tblcategory>.Response(201, "Category created successfully.", newCategory));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Tblcategory updatedCategory)
        {
            try
            {
                if (updatedCategory == null || id != updatedCategory.Categoryid || string.IsNullOrEmpty(updatedCategory.Categoryname))
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Invalid category data or ID mismatch.", null));
                }

                var category = await _context.Tblcategories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Category with ID {id} not found.", null));
                }

                // Check if updated category name already exists (excluding current category)
                var existingCategory = await _context.Tblcategories
                    .AnyAsync(c => c.Categoryname == updatedCategory.Categoryname && c.Categoryid != id);
                    // Note: If Status field exists, add && c.Status == true
                if (existingCategory)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Category name already exists.", null));
                }

                // Update properties
                category.Categoryname = updatedCategory.Categoryname;
                // Note: If UpdatedAt field exists, set:
                // category.UpdatedAt = DateTime.Now;

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<Tblcategory>.Response(200, "Category updated successfully.", category));
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
                var category = await _context.Tblcategories.FindAsync(id);

                if (category == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Category with ID {id} not found.", null));
                }

                // Check if category has associated products
                var hasProducts = await _context.Tblproducts.AnyAsync(p => p.Categoryid == id);
                // Note: If Status field exists in Tblproduct, add && p.Status == true
                if (hasProducts)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Cannot delete category with associated products.", null));
                }

                // Note: If Status and UpdatedAt fields exist, use soft delete:
                // category.Status = false;
                // category.UpdatedAt = DateTime.Now;
                // await _context.SaveChangesAsync();
                // For now, perform hard delete since Status is not available
                _context.Tblcategories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<Tblcategory>.Response(200, "Category deleted successfully.", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }
    }
}