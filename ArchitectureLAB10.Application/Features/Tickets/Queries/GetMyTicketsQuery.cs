using Microsoft.AspNetCore.Authentication;
using System.Security.Claims; // <-- FIX 1: Importar este namespace
using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Ports;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace ArchitectureLAB10.Application.Features.Tickets.Queries;

public class GetMyTicketsQuery : IRequest<IEnumerable<TicketDto>> { }

public class GetMyTicketsQueryHandler : IRequestHandler<GetMyTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMyTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetMyTicketsQuery request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new Exception("Usuario no autenticado.");
        }

        var userId = Guid.Parse(userIdString!);

        var tickets = await _unitOfWork.TicketRepository.FindAsync(t => t.UserId == userId);

        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }
}