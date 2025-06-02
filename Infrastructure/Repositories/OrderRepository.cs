using Microsoft.EntityFrameworkCore;

namespace cybersoft_final_project.Repositories;

using cybersoft_final_project.Context;
using cybersoft_final_project.Entities;

public class OrderRepository
{
    private readonly MyDbContext _context;

    public OrderRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(order order)
    {
        await _context.orders.AddAsync(order);
    }

    public async Task AddDetailAsync(orderdetail detail)
    {
        await _context.orderdetails.AddAsync(detail);
    }
    
    public async Task<IEnumerable<order>> GetAllAsync(Func<IQueryable<order>, IQueryable<order>>? queryShaper = null)
    {
        IQueryable<order> query = _context.orders
            .Include(o => o.orderdetails)
            .ThenInclude(od => od.product);

        if (queryShaper != null)
        {
            query = queryShaper(query);
        }

        return await query.ToListAsync();
    }



    public async Task<order?> GetByIdAsync(int id)
    {
        return await _context.orders
            .Include(o => o.orderdetails)
            .FirstOrDefaultAsync(o => o.orderid == id);
    }

    public void Update(order order)
    {
        _context.orders.Update(order);
    }

    public void Delete(order order)
    {
        _context.orders.Remove(order);
    }

}
