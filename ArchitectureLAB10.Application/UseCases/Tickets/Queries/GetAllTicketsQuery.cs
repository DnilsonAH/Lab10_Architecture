using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArchitectureLAB10.Application.UseCases.Tickets.Queries;

// Query para obtener TODOS los tickets (Admin)
public class GetAllTicketsQuery : IRequest<IEnumerable<TicketDto>> { }

public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.TicketRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }
}