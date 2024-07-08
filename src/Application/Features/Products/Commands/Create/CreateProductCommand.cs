using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application;

public class CreateProductCommand : IRequest<Result<ProductDTO>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;
    private readonly IValidator<CreateProductCommand> validator;
    private readonly IAuthService authService;

    public CreateProductCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService, IValidator<CreateProductCommand> validator)
    {
        this.authService = authService;
        this.uow = uow;
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
        this.validator = validator;
    }

    public async Task<Result<ProductDTO>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product.CreatedAt = DateTime.UtcNow;
        product.CreatedUserId = authService.GetUserId();

        await productRepo.InsertAsync(product);
        await uow.SaveChangesAsync();

        return mapper.Map<ProductDTO>(product);
    }
}