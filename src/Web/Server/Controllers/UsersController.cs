using System.Formats.Asn1;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server;

[Authorize]
public class UsersController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]

    public async Task<ActionResult<string>> Login(LoginUserQuery query)
    {
        var response = await Mediator.Send(query);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(int pageIndex = 0, int pageSize = 20)
    {
        return Ok(await Mediator.Send(new GetAllUsersQuery(pageIndex, pageSize)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetById(Guid id)
    {
        var response = await Mediator.Send(new GetUserByIdQuery(id));

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Reasons);
    }

    [HttpPut]
    public async Task<ActionResult<IEnumerable<UserDTO>>> Update(UpdateUserCommand command)
    {
        var response = await Mediator.Send(command);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Reasons);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await Mediator.Send(new DeleteUserCommand(id));

        return response.IsSuccess ? NoContent() : NotFound(response.Reasons);
    }
}
