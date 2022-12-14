using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Commands;
using Orders.Application.Models;
using Orders.Application.Queries;

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
    public async Task<ActionResult> Start([FromBody] OrderStartRequest request)
    {
        try
        {
            Guid id = await _mediator.Send(new StartOrderCommand(request.CustomerId));

            return Ok(id);

        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpPost("{id}/AddItem")]
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

    [HttpPost("{id}/RemoveItem")]
    public async Task<ActionResult> Remove([FromRoute] Guid id, [FromBody] RemoveItemRequest request)
    {
        try
        {
            await _mediator.Send(new RemoveOrderItemCommand(id, request.ProductId));

            return Ok();

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpPost("{id}/Items/{productId}/SetQuantity")]
    public async Task<ActionResult> SetQuantity([FromRoute] Guid id, [FromRoute] Guid productId, [FromBody] SetQuantityRequest request)
    {
        try
        {
            await _mediator.Send(new EditQuantityCommand(id, productId, request.Quantity));

            return Ok();

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> PlacedOrders([FromQuery] ListPlacedOrdersRequest request)
    {
        try
        {
            List<OrderDto> orders = await _mediator.Send(new ListPlacedOrdersQuery(request.DateStart, request.DateEnd));

            return Ok(orders);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

