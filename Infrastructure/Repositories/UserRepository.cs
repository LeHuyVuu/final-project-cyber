using cybersoft_final_project.Context;
using cybersoft_final_project.Entities;
using Microsoft.EntityFrameworkCore;

public class UserRepository
{
    private readonly MyDbContext _context;

    public UserRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<user>> GetUsers(int page, int pageSize, string? sortBy, string? sortOrder)
    {
        var query = _context.users.Where(u => u.status == true);

        query = sortBy?.ToLower() switch
        {
            "fullname" => sortOrder == "desc"
                ? query.OrderByDescending(u => u.fullname)
                : query.OrderBy(u => u.fullname),
            "phone" => sortOrder == "desc"
                ? query.OrderByDescending(u => u.phone)
                : query.OrderBy(u => u.phone),
            _ => sortOrder == "desc"
                ? query.OrderByDescending(u => u.userid)
                : query.OrderBy(u => u.userid)
        };

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalUsers()
    {
        return await _context.users.CountAsync(u => u.status == true);
    }

    public async Task<user?> GetById(int id)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.userid == id && u.status == true);
    }

    public async Task<bool> PhoneExists(string phone, int excludeId)
    {
        return await _context.users.AnyAsync(u => u.phone == phone && u.userid != excludeId && u.status == true);
    }

    public void Update(user user)
    {
        _context.users.Update(user); // <<== Bổ sung dòng này để entity được tracking chính xác
    }
}