namespace FindFi.Ef.Domain.Entities;

public class Order
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public byte Status { get; set; }
    public string Currency { get; set; } = "USD"; // char(3)
    public decimal TotalAmount { get; set; }
    public DateTime? PlacedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = new();
    public OrderDetails? Details { get; set; }
}