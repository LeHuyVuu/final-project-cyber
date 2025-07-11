using System.Security.Claims;
using System.Threading.Tasks;
using cybersoft_final_project.Infrastructure.UnitOfWork;
using cybersoft_final_project.Models;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(UnitOfWork unitOfWork)
    {
        _orderService = new OrderService(unitOfWork);
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return int.TryParse(userIdClaim, out userId);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] OrderRequest request)
    {
        if (!TryGetUserId(out int userId))
            return Unauthorized(HTTPResponse<object>.Response(401, "Token không hợp lệ hoặc thiếu UserId.", null));

        var result = await _orderService.CreateOrderAsync(request, userId);

        if (!result.Success)
            return BadRequest(HTTPResponse<object>.Response(400, result.Message, null));

        return Ok(HTTPResponse<object>.Response(200, result.Message, result.Data));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _orderService.GetAllOrdersAsync();
        return Ok(HTTPResponse<object>.Response(200, "Lấy danh sách đơn hàng thành công.", result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);

        if (result == null)
            return NotFound(HTTPResponse<object>.Response(404, "Không tìm thấy đơn hàng.", null));

        return Ok(HTTPResponse<object>.Response(200, "Lấy đơn hàng thành công.", result));
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> Update(int id, [FromBody] OrderRequest request)
    // {
    //     if (!TryGetUserId(out int userId))
    //         return Unauthorized(HTTPResponse<object>.Response(401, "Token không hợp lệ.", null));
    //
    //     var result = await _orderService.UpdateOrderAsync(id, request, userId);
    //
    //     if (!result.Success)
    //         return BadRequest(HTTPResponse<object>.Response(400, result.Message, null));
    //
    //     return Ok(HTTPResponse<object>.Response(200, result.Message, result.Data));
    // }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _orderService.DeleteOrderAsync(id);

        if (!result.Success)
            return BadRequest(HTTPResponse<object>.Response(400, result.Message, null));

        return Ok(HTTPResponse<object>.Response(200, result.Message, null));
    }
    
    
    [HttpGet("my-orders")]
    public async Task<IActionResult> GetOrdersByCurrentUser()
    {
        // 2. Lấy UserId từ claim chuẩn của token (cách làm đáng tin cậy)
        var userIdClaim = User.FindFirst("UserId")?.Value;

        // 3. Kiểm tra và chuyển đổi UserId
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {   
            // Trả về lỗi 401 nếu token không hợp lệ hoặc không chứa UserId
            return Unauthorized("Token không hợp lệ hoặc không chứa thông tin người dùng.");
        }

        // 4. Gọi service với userId đã được xác thực
        var userOrders = await _orderService.GetOrdersByUserIdAsync(userId);

        // 5. Trả về kết quả thành công
        return Ok(HTTPResponse<object>.Response(200, "Lấy danh sách đơn hàng thành công.", userOrders));
    }
}
