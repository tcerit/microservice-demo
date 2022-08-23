using Customers.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers;

[ApiController]
[Route("api/Customers/")]
public class CustomersController : ControllerBase
{

    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "Register")]
    public async Task<ActionResult> Register([FromBody] CustomersRegisterRequest request)
    {
        try
        {
            Guid registeredCustomerId = await _mediator.Send(new RegisterCustomerCommand(request.Name, request.LastName));
            return Ok(registeredCustomerId);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
