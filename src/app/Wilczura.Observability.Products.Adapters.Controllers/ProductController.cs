using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Products.Contract.Models;
using Wilczura.Observability.Products.Ports.Models;
using Wilczura.Observability.Products.Ports.Services;

namespace Wilczura.Observability.Products.Adapters.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(
        ILogger<ProductController> logger,
        IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductDto>> GetAsync(long? productId, CancellationToken cancellationToken)
    {
        var result = await _productService.GetAsync(productId);
        return result.Select(x => new ProductDto
        {
            ProductId = x.ProductId,
            Name = x.Name,
        }).ToArray();
    }

    [HttpPost]
    public async Task<IEnumerable<ProductDto>> PostAsync(ProductDto product, CancellationToken cancellationToken)
    {
        var model = new ProductModel()
        {
            ProductId = product.ProductId,
            Name = product.Name,
        };
        var result = await _productService.UpsertAsync(model);
        return result.Select(x => new ProductDto
        {
            ProductId = x.ProductId,
            Name = x.Name,
        }).ToArray();
    }
}
