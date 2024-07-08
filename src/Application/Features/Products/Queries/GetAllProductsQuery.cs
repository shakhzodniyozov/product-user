using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using MediatR;

namespace Application;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
{
    public GetAllProductsQuery(int pageIndex, int pageSize)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    public GetAllProductsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        productRepo = uow.GetRepository<Product>();
        this.mapper = mapper;
    }

    private readonly IRepository<Product> productRepo;
    private readonly IMapper mapper;

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepo.GetPagedListAsync(pageIndex: request.PageIndex, pageSize: request.PageSize);

        return mapper.Map<IEnumerable<ProductDTO>>(products.Items);
    }
}