using ArchitectureLAB10.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureLAB10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator; // Solo inyectamos MediatR

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        try
        {
            var authResponse = await _mediator.Send(command);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Manejo simple de errores
        }
    }
    
    // Aquí crearías el endpoint [HttpPost("login")]
    // que enviaría un `LoginUserCommand`
}