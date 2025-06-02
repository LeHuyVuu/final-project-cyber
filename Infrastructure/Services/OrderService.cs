using cybersoft_final_project.Entities;
using cybersoft_final_project.Models.Request;
using cybersoft_final_project.Infrastructure.UnitOfWork;

public class OrderService
{
    private readonly UnitOfWork _unitOfWork;

    public OrderService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool Success, string Message, object? Data)> CreateOrderAsync(OrderRequest request, int userId)
    {
        if (request.OrderDetails == null || !request.OrderDetails.Any())
            return (false, "Đơn hàng phải có ít nhất một sản phẩm.", null);

        var total = request.OrderDetails.Sum(od => od.Price * od.Quantity);

        var newOrder = new order
        {
            orderdate = DateTime.Now,
            userid = userId,
            total = total,
            status = "Processing"
        };

        await _unitOfWork.OrderRepository.AddAsync(newOrder);
        await _unitOfWork.SaveAsync(); // sinh orderid

        foreach (var item in request.OrderDetails)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                return (false, $"Sản phẩm với ID {item.ProductId} không tồn tại.", null);

            if (product.stock_quantity < item.Quantity)
                return (false, $"Sản phẩm '{product.productname}' không đủ hàng trong kho.", null);

            // Trừ kho và tăng số lượng đã bán
            product.stock_quantity -= item.Quantity;
            product.sold_quantity = (product.sold_quantity ?? 0) + item.Quantity;

            // Tạo chi tiết đơn hàng
            var detail = new orderdetail
            {
                orderid = newOrder.orderid,
                productid = item.ProductId,
                quantity = item.Quantity,
                price = item.Price,
                status = "Processing"
            };

            await _unitOfWork.OrderRepository.AddDetailAsync(detail);
        }

        await _unitOfWork.SaveAsync();

        return (true, "Đặt hàng thành công", new { OrderId = newOrder.orderid });
    }
    
    public async Task<IEnumerable<order>> GetAllOrdersAsync()
    {
        return await _unitOfWork.OrderRepository.GetAllAsync(
            q => q.OrderByDescending(o => o.orderdate));
    }

    
    public async Task<order?> GetOrderByIdAsync(int id)
    {
        return await _unitOfWork.OrderRepository.GetByIdAsync(id);
    }

    // public async Task<(bool Success, string Message, object? Data)> UpdateOrderAsync(int id, OrderRequest request, int userId)
    // {
    //     var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(id);
    //     if (existingOrder == null)
    //         return (false, "Không tìm thấy đơn hàng.", null);
    //
    //     if (request.OrderDetails == null || !request.OrderDetails.Any())
    //         return (false, "Đơn hàng cần ít nhất một sản phẩm.", null);
    //
    //     // Xử lý cập nhật: đơn giản chỉ cập nhật status và total
    //     existingOrder.total = request.OrderDetails.Sum(d => d.Price * d.Quantity);
    //     existingOrder.status = request. ?? existingOrder.status;
    //     existingOrder.userid = userId;
    //
    //     _unitOfWork.OrderRepository.Update(existingOrder);
    //     await _unitOfWork.SaveAsync();
    //
    //     return (true, "Cập nhật đơn hàng thành công.", existingOrder);
    // }

    public async Task<(bool Success, string Message)> DeleteOrderAsync(int id)
    {
        var existingOrder = await _unitOfWork.OrderRepository.GetByIdAsync(id);
        if (existingOrder == null)
            return (false, "Không tìm thấy đơn hàng để xoá.");

        _unitOfWork.OrderRepository.Delete(existingOrder);
        await _unitOfWork.SaveAsync();
        return (true, "Xoá đơn hàng thành công.");
    }


}