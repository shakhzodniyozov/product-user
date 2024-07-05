using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using MediatR;

namespace Application;

public class RegisterUserCommand : IRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<User> userRepo;
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public RegisterUserCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService)
    {
        this.mapper = mapper;
        this.uow = uow;
        this.authService = authService;
        userRepo = uow.GetRepository<User>();
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(request);
        var userId = await authService.Register(user, request.Password);
    }
}