using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Domain;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application;

public class UpdateUserCommand : IRequest<Result<UserDTO>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleLevelCascadeMode =  CascadeMode.Stop;
        
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be null.");
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IRepository<User> userRepo;
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public UpdateUserCommandHandler(IUnitOfWork uow, IAuthService authService, IMapper mapper)
    {
        this.uow = uow;
        userRepo = uow.GetRepository<User>();
        this.mapper = mapper;
        this.authService = authService;
    }

    public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = authService.GetUserId();
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Id == userId, disableTracking: false);

        mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;
        await uow.SaveChangesAsync();

        return Result.Ok(mapper.Map<UserDTO>(user));
    }
}