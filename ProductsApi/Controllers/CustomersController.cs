using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Models;

namespace ProductsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET /api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return Ok(await _context.Customers.ToListAsync());
    }

    // GET /api/customers/{name}/products
    [HttpGet("{name}/products")]
    public async Task<ActionResult<IEnumerable<CustomerProductResponse>>> GetProductsForCustomer(string name)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerProductPrices)
            .FirstOrDefaultAsync(c => c.Name == name);

        if (customer == null)
            return NotFound($"Customer '{name}' not found.");

        var agreedPrices = customer.CustomerProductPrices
            .ToDictionary(cpp => cpp.ProductId, cpp => cpp.AgreedPrice);

        var allProducts = await _context.Products.ToListAsync();

        var products = allProducts.Select(p => new CustomerProductResponse
        {
            ProductId = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = agreedPrices.TryGetValue(p.Id, out var agreed) ? agreed : p.Price,
            HasAgreedPrice = agreedPrices.ContainsKey(p.Id),
            Inventory = p.Inventory
        });

        return Ok(products);
    }
}

public class CustomerProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool HasAgreedPrice { get; set; }
    public int Inventory { get; set; }
}
