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
    
    public async Task<IEnumerable<order>> GetAllAsync()
    {
        return await _context.orders
            .Include(o => o.orderdetails)
            .ThenInclude(od => od.product) // Thêm dòng này để lấy luôn product trong mỗi orderdetail
            .ToListAsync();
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
