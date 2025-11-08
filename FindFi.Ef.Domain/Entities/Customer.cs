namespace FindFi.Ef.Domain.Entities;

public class Customer
{
    public long CustomerId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public byte Role { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    public List<CustomerAddress> Addresses { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}