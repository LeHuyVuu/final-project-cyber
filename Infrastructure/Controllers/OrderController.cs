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
}
