using cybersoft_final_project.Context;
using cybersoft_final_project.Repositories;

namespace cybersoft_final_project.Infrastructure.UnitOfWork;

public class UnitOfWork
{
    private readonly MyDbContext _context;

    public ProductRepository ProductRepository { get; }
    public UserRepository UserRepository { get; }
    public OrderRepository OrderRepository { get; }

    public UnitOfWork(MyDbContext context,
        ProductRepository productRepository,
        UserRepository userRepository,
        OrderRepository orderRepository)
    {
        _context = context;
        ProductRepository = productRepository;
        UserRepository = userRepository;
        OrderRepository = orderRepository;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}