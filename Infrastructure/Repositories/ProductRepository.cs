using cybersoft_final_project.Context;
using cybersoft_final_project.Entities;
using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Repositories
{
    public class ProductRepository(MyDbContext context)
    {
        public IQueryable<product> GetAllWithIncludes()
        {
            return context.products
                .Include(p => p.recommendation_products)
                    .ThenInclude(rp => rp.recommendation_type)
                .Include(p => p.productattributes)
                .AsNoTracking();
        }
        public async Task<product?> GetByIdAsync(int id)
        {
            var product = await context.products
                .Include(p => p.product_images) // Bao gồm các ảnh của sản phẩm
                .FirstOrDefaultAsync(p => p.productid == id); // Tìm sản phẩm theo id

            return product;
        }



        public IQueryable<product> GetByCategory(int categoryId)
        {
            return context.products
                .Where(p => p.categoryid == categoryId && p.status == true);
        }

       

        public List<product> GetProductsByRecommendationTypeSlug(string slug)
        {
            var products = context.recommendation_products
                .Where(rp => rp.recommendation_type.slug_name.Replace(" ", "-").ToLower() == slug.ToLower())
                .Include(rp => rp.product)
                    .ThenInclude(p => p.productattributes)
                .Include(rp => rp.product)
                    .ThenInclude(p => p.recommendation_products)
                    .ThenInclude(rp => rp.recommendation_type)
                .Select(rp => rp.product)
                .Where(p => p.status == true)
                .AsNoTracking()
                .ToList();

            return products;
        }

        public recommendation_type? GetRecommendationTypeBySlug(string slug)
        {
            return context.recommendation_types
                .FirstOrDefault(p => p.slug_name.Replace(" ", "-").ToLower() == slug.ToLower());
        }
    }
}
