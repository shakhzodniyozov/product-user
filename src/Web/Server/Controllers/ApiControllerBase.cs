using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Server;

[ApiController, Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    private IMediator? mediator;
    public IMediator Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
