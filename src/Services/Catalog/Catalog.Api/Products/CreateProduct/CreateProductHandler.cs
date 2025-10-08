

namespace Catalog.Api.Products.CreateProduct;


public record CreateProductCommand(string Name, List<string> Categories, string Description, string ImageFile, decimal Price)
    :ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Categories)
            .NotEmpty().WithMessage("At least one category is required.");

        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("Image file is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
} 

internal class CreateProductCommandHandler(IDocumentSession session )  :
    ICommandHandler<CreateProductCommand , CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //not using it manually as it redundant code
        //we will make it auto by using MediatR  pipeline behavior
        //var validationResult = await validator.ValidateAsync(command, cancellationToken);
        //var errors = validationResult.Errors.Select(x=>x.ErrorMessage).ToList();

        //  if (errors.Any())
        //      throw new ValidationException(errors.FirstOrDefault());

        //Create product entity
        var productEntity = new Product
        {
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        //Save to database (not implemented in this snippet)

        session.Store(productEntity);
        await session.SaveChangesAsync(cancellationToken); 
        
        // Return the result with the new product ID 
        return new CreateProductResult(productEntity.Id);
    }
}