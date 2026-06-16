using BuildingBlocks.Exceptions;
namespace Catalog.API.Exceptions;
public class ProductNotFoundExceptions : NotFoundException
{
    public ProductNotFoundExceptions(Guid id) : base("Product", id)
    {
            
    }
}