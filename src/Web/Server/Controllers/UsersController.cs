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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var registerResult = await Mediator.Send(command);
        return registerResult.IsSuccess ? CreatedAtAction(nameof(GetById), new {id = registerResult.Value}) : BadRequest(registerResult.Reasons);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Login(LoginUserQuery query)
    {
        var response = await Mediator.Send(query);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Errors);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll(int pageIndex = 0, int pageSize = 20)
    {
        return Ok(await Mediator.Send(new GetAllUsersQuery(pageIndex, pageSize)));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetById(Guid id)
    {
        var response = await Mediator.Send(new GetUserByIdQuery(id));

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Reasons);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> Update(UpdateUserCommand command)
    {
        var response = await Mediator.Send(command);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Reasons);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await Mediator.Send(new DeleteUserCommand(id));

        return response.IsSuccess ? NoContent() : BadRequest(response.Reasons);
    }
}
