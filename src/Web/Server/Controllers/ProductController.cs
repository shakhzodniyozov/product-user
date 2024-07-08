using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server;

[Authorize]
public class ProductController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDTO>> Create(CreateProductCommand command)
    {
        var createResult = await Mediator.Send(command);

        return createResult.IsSuccess ? CreatedAtAction(nameof(GetById), new {id = createResult.Value.Id}, createResult.Value) 
                                            : BadRequest(createResult.Reasons);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll(int pageIndex = 0, int pageSize =  20)
    {
        return Ok(await Mediator.Send(new GetAllProductsQuery(pageIndex, pageSize)));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetById(Guid id)
    {
        var response = await Mediator.Send(new GetProductByIdQuery(id));

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Reasons);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDTO>> Update(UpdateProductCommand command)
    {
        var response = await Mediator.Send(command);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Reasons);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await Mediator.Send(new DeleteProductCommand(id));

        return response.IsSuccess ? NoContent() : BadRequest(response.Reasons);
    }
}
