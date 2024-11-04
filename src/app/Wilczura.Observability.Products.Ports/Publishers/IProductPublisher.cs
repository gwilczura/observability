using Wilczura.Observability.Products.Ports.Models;

namespace Wilczura.Observability.Products.Ports.Publishers;

public interface IProductPublisher
{
    Task PublishProductChangedAsync(ProductModel product);
}
