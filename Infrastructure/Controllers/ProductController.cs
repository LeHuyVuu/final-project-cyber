using Microsoft.AspNetCore.Mvc;
using cybersoft_final_project.Models;
using cybersoft_final_project.Services;
using cybersoft_final_project.Entities;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;

namespace cybersoft_final_project.Controllers;

// [Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductService service) : ControllerBase
{
    [HttpGet]
    [OutputCache(Duration = 60, VaryByQueryKeys = ["page", "pageSize", "sortBy", "sortOrder"])]
    public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, string? sortBy = "productid",
        string? sortOrder = "asc")
    {
        var (products, totalItems) = await service.GetProductsAsync(page, pageSize, sortBy!, sortOrder!);
        if (!products.Any())
            return NotFound(HTTPResponse<object>.Response(404, "No products found.", null));

        var pagination = new
        {
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

        return Ok(HTTPResponse<object>.Response(200, "Products retrieved successfully.",
            new { Products = products, Pagination = pagination }));
    }

    [HttpGet("{id}")]
    [OutputCache(Duration = 60, VaryByRouteValueNames = ["id"])]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await service.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(HTTPResponse<object>.Response(404, $"Product with ID {id} not found.", null));

        return Ok(HTTPResponse<product>.Response(200, "Product retrieved successfully.", product));
    }

    [HttpGet("category/{categoryId}")]
    [OutputCache(Duration = 60, VaryByRouteValueNames = ["categoryId"], VaryByQueryKeys = ["page", "pageSize", "sortBy", "sortOrder"])]
    public async Task<IActionResult> GetProductsByCategory(int categoryId, int page = 1, int pageSize = 10,
        string? sortBy = "productid", string? sortOrder = "asc")
    {
        var (products, totalItems) =
            await service.GetProductsByCategoryAsync(categoryId, page, pageSize, sortBy!, sortOrder!);
        if (!products.Any())
            return NotFound(
                HTTPResponse<object>.Response(404, $"No products found for category ID {categoryId}.", null));

        var pagination = new
        {
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

        return Ok(HTTPResponse<object>.Response(200, "Products retrieved successfully.",
            new { Products = products, Pagination = pagination }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        if (request == null || id != request.ProductId)
            return BadRequest(HTTPResponse<object>.Response(400, "Invalid product data or ID mismatch.", null));

        var product = await service.UpdateProductAsync(id, request);
        if (product == null)
            return NotFound(HTTPResponse<object>.Response(404, $"Product with ID {id} not found.", null));

        return Ok(HTTPResponse<product>.Response(200, "Product updated successfully.", product));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await service.SoftDeleteProductAsync(id);
        if (product == null)
            return NotFound(HTTPResponse<object>.Response(404, $"Product with ID {id} not found.", null));

        return Ok(HTTPResponse<product>.Response(200, "Product deleted successfully.", product));
    }

    [HttpGet("/product/{slugName}")]
    [OutputCache(Duration = 60, VaryByRouteValueNames = ["slugName"])]
    public IActionResult GetProductDetailsWithRecommendations(string slugName)
    {
        var recommendationType = service.GetRecommendationType(slugName);
        if (recommendationType == null)
            return NotFound(HTTPResponse<object>.Response(404, "Recommendation type not found.", null));

        var products = service.GetRecommendedProducts(slugName);
        if (!products.Any())
            return NotFound(HTTPResponse<object>.Response(404, "No products found for this recommendation type.",
                null));

        return Ok(HTTPResponse<object>.Response(200, "Products retrieved successfully.", products));
    }


    [HttpGet("sale")]


    public async Task<IActionResult> GetProductsOnSale(
        int page = 1,
        int pageSize = 10,
        string sortBy = "productid",
        string sortOrder = "asc")
    {
        var (products, total) = await service.GetAllProductsOnSaleAsync(page, pageSize, sortBy, sortOrder);

        if (products == null || !products.Any())
        {
            return NotFound(HTTPResponse<object>.Response(404, "No products found on sale.", null));
        }

        var responseData = new
        {
            TotalCount = total,
            Page = page,
            PageSize = pageSize,
            Products = products
        };

        return Ok(HTTPResponse<object>.Response(200, "Sale products retrieved successfully.", responseData));
    }
}