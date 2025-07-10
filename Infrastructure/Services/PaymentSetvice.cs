using PayPalCheckoutSdk.Orders;

public class PaymentService
{
    private readonly PayPalClient _paypal;

    public PaymentService(PayPalClient paypal)
    {
        _paypal = paypal;
    }

    public async Task<string?> CreateOrderAsync(string totalPrice)
    {
        var order = new OrderRequest()
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = "USD",
                        Value = totalPrice
                    }
                }
            },
            ApplicationContext = new ApplicationContext
            {
                // Điều chỉnh returnUrl để bạn có thể tự xử lý sau khi nhận được thông tin từ PayPal
                ReturnUrl = "http://localhost:3000/step/success",
                CancelUrl = "http://localhost:3000/"
            }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);

        var response = await _paypal.Client.Execute(request);
        var result = response.Result<Order>();
        var approveLink = result.Links.FirstOrDefault(l => l.Rel == "approve")?.Href;

        // Thay vì trả về approveLink, bạn có thể làm một redirect trực tiếp tới returnUrl sau khi lấy mã token từ PayPal
        if (approveLink != null)
        {
            // Redirect trực tiếp tới trang returnUrl sau khi xử lý
            // Bạn có thể dùng `HttpContext.Response.Redirect` trong controller hoặc làm như ví dụ dưới
            return "http://localhost:3000/step/success"; // Redirect tới URL chính mà không có tham số.
        }

        return null;
    }
}
