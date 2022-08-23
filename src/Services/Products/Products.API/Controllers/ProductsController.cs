using Products.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Products.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    
    private readonly ILogger<ProductsController> _logger;
    private readonly IMediator _mediator;

    public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductRequest request)
    {
        try
        {
            Guid productId = await _mediator.Send(new CreateProductCommand(request.Name, request.Price));
            return Ok(productId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/list")]
    public async Task<ActionResult<Guid>> List(Guid id)
    {
        try
        {
            await _mediator.Send(new ListProductCommand(id));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/delist")]
    public async Task<ActionResult<Guid>> Delist(Guid id)
    {
        try
        {
            await _mediator.Send(new DelistProductCommand(id));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}

