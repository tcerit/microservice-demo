using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands;

namespace Orders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("Start")]
    public async Task<ActionResult> Start([FromBody] Guid customerId)
    {
        try
        {
            Guid id = await _mediator.Send(new StartOrderCommand(customerId));

            return Ok(id);

        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpPost("{id}/Add")]
    public async Task<ActionResult> Add([FromRoute] Guid id, [FromBody] AddOrderItemRequest request)
    {
        try
        {
            await _mediator.Send(new AddOrderItemCommand(id, request.ProductId, request.Quantity));

            return Ok();

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
}

