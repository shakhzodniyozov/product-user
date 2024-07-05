using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using FluentResults;
using MediatR;

namespace Application;

public class GetUserByIdQuery : IRequest<Result<UserDTO>>
{
    public GetUserByIdQuery(Guid id) => Id = id;

    public Guid Id { get; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDTO>>
{
    private readonly IRepository<User> userRepo;
    private readonly IMapper mapper;

    public GetUserByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        userRepo = uow.GetRepository<User>();
        this.mapper = mapper;
    }

    public async Task<Result<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

        return user is not null ? Result.Ok(mapper.Map<UserDTO>(user)) : Result.Fail<UserDTO>($"User with provided Id={request.Id} was not found.");
    }
}
