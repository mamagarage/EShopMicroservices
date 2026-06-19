

namespace Catalog.API.Products.CreateProduct;
// This record represents the command to create a product, containing all necessary information about the product to be created.
public record CreateProductCommand
    (string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;


// This record represents the result of the CreateProductCommand, containing the Id of the newly created product.   
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file path is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}


// This handler is responsible for processing the CreateProductCommand and returning a CreateProductResult.
// The actual implementation of the Handle method would include the business logic to create a product, such as validating the input, saving the product to a database, etc.

/*
internal class CreateProductCommandHandler(
    IDocumentSession session, IValidator<CreateProductCommand> validator)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create a new product based on the command object,
        // save it to the database,
        // and return CreateProductResult with the Id of the newly created product.

        var results = await validator.ValidateAsync(command);
        var errors = results.Errors.Select(e => e.ErrorMessage).ToList();

        if (errors.Any())
        {
            throw new ValidationException(errors.FirstOrDefault());
        }

        var product = new Product
        {
            //Id = Guid.NewGuid(),
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(product);
        await session.SaveChangesAsync();

        return new CreateProductResult(product.Id);
    }
}
*/

internal class CreateProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create a new product based on the command object,
        // save it to the database,
        // and return CreateProductResult with the Id of the newly created product.
        
        //logger.LogInformation("Handling CreateProductCommand for product: {ProductName}", command.Name);

        var product = new Product
        {
            //Id = Guid.NewGuid(),
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(product);
        await session.SaveChangesAsync();

        return new CreateProductResult(product.Id);
    }
}