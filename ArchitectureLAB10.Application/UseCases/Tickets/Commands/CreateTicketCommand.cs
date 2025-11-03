using System.Security.Claims; // <-- FIX 1: Importar este namespace
using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http; 

namespace ArchitectureLAB10.Application.UseCases.Tickets.Commands;

public class CreateTicketCommand : IRequest<TicketDto>
{
    public CreateTicketDto CreateTicketDto { get; set; } = null!;
}

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new Exception("Usuario no autenticado.");
        }
        var userId = Guid.Parse(userIdString!);
        var ticket = _mapper.Map<Ticket>(request.CreateTicketDto);
        
        ticket.TicketId = Guid.NewGuid();
        ticket.UserId = userId; 
        ticket.Status = "abierto"; 
        ticket.CreatedAt = DateTime.UtcNow;
        
        await _unitOfWork.TicketRepository.AddAsync(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<TicketDto>(ticket);
    }
}