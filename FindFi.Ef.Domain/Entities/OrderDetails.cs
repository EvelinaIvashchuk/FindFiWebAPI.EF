namespace FindFi.Ef.Domain.Entities;

public class OrderDetails
{
    public long OrderId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string? BillingEmail { get; set; }
    public string? BillingAddress { get; set; }
    public string? Notes { get; set; }

    public Order Order { get; set; } = null!;
}