namespace ProductsApi.Models;

public class CustomerProductPrice
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal AgreedPrice { get; set; }
}
