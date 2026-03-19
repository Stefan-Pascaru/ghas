namespace ProductsApi.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<CustomerProductPrice> CustomerProductPrices { get; set; } = [];
}
