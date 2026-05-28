using MediatR;
using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct
{
    // This record represents the command to create a product, containing all necessary information about the product to be created.
    public record CreateProductCommand
        (string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;


    // This record represents the result of the CreateProductCommand, containing the Id of the newly created product.   
    public record CreateProductResult(Guid Id);


    // This handler is responsible for processing the CreateProductCommand and returning a CreateProductResult.
    // The actual implementation of the Handle method would include the business logic to create a product, such as validating the input, saving the product to a database, etc.
    internal class CreateProductCommandHandler
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create a new product based on the command object,
            // save it to the database,
            // and return CreateProductResult with the Id of the newly created product.

            var product = new Product
            {
                //Id = Guid.NewGuid(),
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            
            return new CreateProductResult(Guid.NewGuid());
        }
    }
}