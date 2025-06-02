using cybersoft_final_project.Entities;
using cybersoft_final_project.Infrastructure.UnitOfWork;
using cybersoft_final_project.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Services
{
    public class ProductService
    {
        private readonly UnitOfWork unit;

        public ProductService(UnitOfWork unit)
        {
            this.unit = unit;
        }

        public async Task<(List<product>, int)> GetProductsAsync(int page, int pageSize, string sortBy, string sortOrder)
        {
            var query = unit.ProductRepository.GetAllWithIncludes()
                         .Where(p => p.status == true); // ✅ Chỉ lấy sản phẩm còn hàng và đang active

            query = sortBy?.ToLower() switch
            {
                "price" => sortOrder == "desc" ? query.OrderByDescending(p => p.price) : query.OrderBy(p => p.price),
                "productname" => sortOrder == "desc" ? query.OrderByDescending(p => p.productname) : query.OrderBy(p => p.productname),
                "ratingavg" => sortOrder == "desc" ? query.OrderByDescending(p => p.rating_avg) : query.OrderBy(p => p.rating_avg),
                "soldquantity" => sortOrder == "desc" ? query.OrderByDescending(p => p.sold_quantity) : query.OrderBy(p => p.sold_quantity),
                _ => sortOrder == "desc" ? query.OrderByDescending(p => p.productid) : query.OrderBy(p => p.productid)
            };

            var total = await query.CountAsync();
            var result = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (result, total);
        }

        public async Task<product?> GetProductByIdAsync(int id)
        {
            var product = await unit.ProductRepository.GetByIdAsync(id);
            if (product == null || product.stock_quantity <= 0 || product.status != true) return null; // ✅ Kiểm tra còn hàng & đang active
            return product;
        }

        public async Task<(List<product>, int)> GetProductsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, string sortOrder)
        {
            var query = unit.ProductRepository.GetByCategory(categoryId)
                         .Where(p => p.stock_quantity > 0 && p.status == true); // ✅ Chỉ lấy sản phẩm còn hàng và đang active

            query = sortBy?.ToLower() switch
            {
                "price" => sortOrder == "desc" ? query.OrderByDescending(p => p.price) : query.OrderBy(p => p.price),
                "productname" => sortOrder == "desc" ? query.OrderByDescending(p => p.productname) : query.OrderBy(p => p.productname),
                "ratingavg" => sortOrder == "desc" ? query.OrderByDescending(p => p.rating_avg) : query.OrderBy(p => p.rating_avg),
                "soldquantity" => sortOrder == "desc" ? query.OrderByDescending(p => p.sold_quantity) : query.OrderBy(p => p.sold_quantity),
                _ => sortOrder == "desc" ? query.OrderByDescending(p => p.productid) : query.OrderBy(p => p.productid)
            };

            var total = await query.CountAsync();
            var result = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (result, total);
        }

        public async Task<product?> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            var existingProduct = await unit.ProductRepository.GetByIdAsync(id);
            if (existingProduct == null) return null;

            existingProduct.productname = request.ProductName;
            existingProduct.image = request.Image;
            existingProduct.brand = request.Brand;
            existingProduct.sku = request.Sku;
            existingProduct.price = request.Price;
            existingProduct.discount_price = request.DiscountPrice;
            existingProduct.stock_quantity = request.stock_quantity ?? existingProduct.stock_quantity;


            existingProduct.updated_at = DateTime.Now;
            await unit.SaveAsync();
            return existingProduct;
        }


        public async Task<product?> SoftDeleteProductAsync(int id)
        {
            var product = await unit.ProductRepository.GetByIdAsync(id);
            if (product == null) return null;

            product.status = false;
            product.updated_at = DateTime.Now;
            await unit.SaveAsync();
            return product;
        }

        public List<product> GetRecommendedProducts(string slugName)
        {
            return unit.ProductRepository.GetProductsByRecommendationTypeSlug(slugName)
                       .Where(p => p.stock_quantity > 0 && p.status == true).ToList(); // ✅ Lọc sản phẩm còn hàng và đang active
        }

        public recommendation_type? GetRecommendationType(string slugName)
        {
            return unit.ProductRepository.GetRecommendationTypeBySlug(slugName);
        }
        
        
        public async Task<(List<product>, int)> GetAllProductsOnSaleAsync(int page, int pageSize, string sortBy, string sortOrder)
        {
            var query = unit.ProductRepository.GetAllWithIncludes()
                .Where(p => p.is_sale == true && p.stock_quantity > 0 && p.status == true);

            query = sortBy?.ToLower() switch
            {
                "price" => sortOrder == "desc" ? query.OrderByDescending(p => p.price) : query.OrderBy(p => p.price),
                "productname" => sortOrder == "desc" ? query.OrderByDescending(p => p.productname) : query.OrderBy(p => p.productname),
                "ratingavg" => sortOrder == "desc" ? query.OrderByDescending(p => p.rating_avg) : query.OrderBy(p => p.rating_avg),
                "soldquantity" => sortOrder == "desc" ? query.OrderByDescending(p => p.sold_quantity) : query.OrderBy(p => p.sold_quantity),
                _ => sortOrder == "desc" ? query.OrderByDescending(p => p.productid) : query.OrderBy(p => p.productid)
            };

            var total = await query.CountAsync();
            var result = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (result, total);
        }

    }
}
