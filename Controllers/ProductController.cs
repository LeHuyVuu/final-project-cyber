using cybersoft_final_project.Context;
using Microsoft.AspNetCore.Mvc;
using cybersoft_final_project.Entities;
using cybersoft_final_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace cybersoft_final_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _context = new MyDbContext();

        // GET: api/products (with pagination and sorting)
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "Productid", // Sửa "ProductId" thành "Productid" để khớp với entity
            [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var query = _context.Tblproducts.AsQueryable();

                // Apply sorting using LINQ with switch expression for common fields
                query = sortBy?.ToLower() switch
                {
                    "price" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),
                    "productname" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Productname)
                        : query.OrderBy(p => p.Productname),
                    "ratingavg" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.RatingAvg)
                        : query.OrderBy(p => p.RatingAvg),
                    "soldquantity" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.SoldQuantity)
                        : query.OrderBy(p => p.SoldQuantity),
                    _ => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Productid) // Default sort by Productid
                        : query.OrderBy(p => p.Productid)
                };

                // Apply pagination
                var totalItems = await query.CountAsync();
                var products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (products == null || !products.Any())
                {
                    return NotFound(HTTPResponse<object>.Response(404, "No products found.", null));
                }

                var paginationInfo = new
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return Ok(HTTPResponse<object>.Response(200, "Products retrieved successfully.",
                    new { Products = products, Pagination = paginationInfo }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _context.Tblproducts.FindAsync(id);

                if (product == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Product with ID {id} not found.", null));
                }

                return Ok(HTTPResponse<Tblproduct>.Response(200, "Product retrieved successfully.", product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }
        
        // GET: api/products/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(
            int categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "Productid",
            [FromQuery] string? sortOrder = "asc")
        {
            try
            {
                var query = _context.Tblproducts
                    .Where(p => p.Categoryid == categoryId && p.Status == true)
                    .AsQueryable();

                // Apply sorting using LINQ with switch expression for common fields
                query = sortBy?.ToLower() switch
                {
                    "price" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),
                    "productname" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Productname)
                        : query.OrderBy(p => p.Productname),
                    "ratingavg" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.RatingAvg)
                        : query.OrderBy(p => p.RatingAvg),
                    "soldquantity" => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.SoldQuantity)
                        : query.OrderBy(p => p.SoldQuantity),
                    _ => sortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Productid)
                        : query.OrderBy(p => p.Productid)
                };

                // Apply pagination
                var totalItems = await query.CountAsync();
                var products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (products == null || !products.Any())
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"No products found for category ID {categoryId}.", null));
                }

                var paginationInfo = new
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };

                return Ok(HTTPResponse<object>.Response(200, "Products retrieved successfully.",
                    new { Products = products, Pagination = paginationInfo }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Tblproduct updatedProduct)
        {
            try
            {
                if (updatedProduct == null || id != updatedProduct.Productid)
                {
                    return BadRequest(HTTPResponse<object>.Response(400, "Invalid product data or ID mismatch.", null));
                }

                var product = await _context.Tblproducts.FindAsync(id);
                if (product == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Product with ID {id} not found.", null));
                }

                // Update properties
                product.Productname = updatedProduct.Productname;
                product.Image = updatedProduct.Image;
                product.Price = updatedProduct.Price;
                product.Quantity = updatedProduct.Quantity;
                product.Importdate = updatedProduct.Importdate;
                product.Usingdate = updatedProduct.Usingdate;
                product.Brand = updatedProduct.Brand;
                product.Sku = updatedProduct.Sku;
                product.DiscountPrice = updatedProduct.DiscountPrice;
                product.DiscountPercent = updatedProduct.DiscountPercent;
                product.StockQuantity = updatedProduct.StockQuantity;
                product.SoldQuantity = updatedProduct.SoldQuantity;
                product.IsAvailable = updatedProduct.IsAvailable;
                product.RatingAvg = updatedProduct.RatingAvg;
                product.RatingCount = updatedProduct.RatingCount;
                product.Categoryid = updatedProduct.Categoryid;
                product.UpdatedAt = DateTime.Now;
                product.Status = updatedProduct.Status;

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<Tblproduct>.Response(200, "Product updated successfully.", product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }

        // DELETE: api/products/{id} (soft delete by setting Status = false)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Tblproducts.FindAsync(id);

                if (product == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, $"Product with ID {id} not found.", null));
                }

                product.Status = false; // Soft delete
                product.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(HTTPResponse<Tblproduct>.Response(200, "Product deleted successfully.", product));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, $"An error occurred: {ex.Message}", null));
            }
        }
    }
}