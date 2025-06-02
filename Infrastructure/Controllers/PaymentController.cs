using cybersoft_final_project.Models;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace SWD392_backend.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly OrderService _orderService;

    public PaymentController(PaymentService paymentService, OrderService orderService)
    {
        _paymentService = paymentService;
        _orderService = orderService;
    }

    [HttpPost("paypal")]
    public async Task<IActionResult> CreatePaypalOrder([FromBody] string totalPrice)
    {
        var approvalUrl = await _paymentService.CreateOrderAsync(totalPrice);
        return Ok(new { url = approvalUrl });
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
}
