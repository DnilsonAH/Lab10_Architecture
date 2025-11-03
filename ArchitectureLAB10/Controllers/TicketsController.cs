using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Application.Features.Tickets.Commands;
using ArchitectureLAB10.Application.Features.Tickets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization; // Importante
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureLAB10.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST api/tickets
    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketDto createTicketDto)
    {
        var command = new CreateTicketCommand { CreateTicketDto = createTicketDto };
        var ticketDto = await _mediator.Send(command);
        return Ok(ticketDto);
    }

    // GET api/tickets/my-tickets
    [HttpGet("my-tickets")]
    public async Task<IActionResult> GetMyTickets()
    {
        var query = new GetMyTicketsQuery();
        var tickets = await _mediator.Send(query);
        return Ok(tickets);
    }

    // GET api/tickets/all
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllTickets()
    {
        var query = new GetAllTicketsQuery();
        var tickets = await _mediator.Send(query);
        return Ok(tickets);
    }

    // PUT api/tickets/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTicket(Guid id, UpdateTicketDto updateTicketDto)
    {
        var command = new UpdateTicketCommand
        {
            TicketId = id,
            UpdateTicketDto = updateTicketDto
        };
        await _mediator.Send(command);
        return NoContent();
    }
    
    // DELETE api/tickets/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var command = new DeleteTicketCommand { TicketId = id };
        await _mediator.Send(command);
        return NoContent();
    }
}