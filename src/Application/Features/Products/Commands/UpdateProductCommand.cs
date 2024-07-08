using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application;

public class UpdateProductCommand : IRequest<Result<ProductDTO>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public UpdateProductCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService)
    {
        this.uow = uow;
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
        this.authService = authService;
    }

    public async Task<Result<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id, disableTracking: false);

        if (product is null)
            return Result.Fail($"Product with provided Id={request.Id} was not found.");
        
        mapper.Map(request, product);
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdateUserId = authService.GetUserId();

        await uow.SaveChangesAsync();

        return Result.Ok(mapper.Map<ProductDTO>(product));
    }
}