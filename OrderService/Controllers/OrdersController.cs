using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

public record CreateOrderDto(int ProductId, int Quantity);
public record ProductDto(int Id, string Name, decimal Price);

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _db;
    private readonly IHttpClientFactory _http;

    public OrdersController(OrdersDbContext db, IHttpClientFactory http)
    {
        _db = db;
        _http = http;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _db.Orders.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var client = _http.CreateClient("product");
        var resp = await client.GetAsync($"/api/products/{dto.ProductId}");
        if (!resp.IsSuccessStatusCode) return BadRequest("Product not found");

        var product = await resp.Content.ReadFromJsonAsync<ProductDto>();
        if (product is null) return BadRequest("Invalid product data");

        var order = new Order
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = product.Price,
            TotalPrice = product.Price * dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var o = await _db.Orders.FindAsync(id);
        return o is null ? NotFound() : Ok(o);
    }
}
