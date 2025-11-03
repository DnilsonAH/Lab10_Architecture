using ArchitectureLAB10.Application.UseCases.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureLAB10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

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
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        try
        {
            var authResponse = await _mediator.Send(command);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}