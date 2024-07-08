using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using FluentResults;
using MediatR;

namespace Application;

public class GetProductByIdQuery : IRequest<Result<ProductDTO>>
{
    public GetProductByIdQuery(Guid id) => Id = id;

    public Guid Id { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
{
    public GetProductByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
    }

    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;

    public async Task<Result<ProductDTO>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

        return product is not null ? Result.Ok(mapper.Map<ProductDTO>(product)) : Result.Fail($"Product with provided Id={request.Id} was not found.");
    }
}