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
                ReturnUrl = "http://localhost:3000/step/success",
                CancelUrl = "http://localhost:3000/"
            }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);

        var response = await _paypal.Client.Execute(request);
        var result = response.Result<Order>();
        return result.Links.FirstOrDefault(l => l.Rel == "approve")?.Href;
    }
}