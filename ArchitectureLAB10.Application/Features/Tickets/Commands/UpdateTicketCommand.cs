using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArchitectureLAB10.Application.Features.Tickets.Commands;

public class UpdateTicketCommand : IRequest
{
    public Guid TicketId { get; set; }
    public UpdateTicketDto UpdateTicketDto { get; set; } = null!;
}

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.TicketRepository.GetByIdAsync(request.TicketId);
        if (ticket == null)
        {
            throw new Exception("Ticket no encontrado.");
        }

        _mapper.Map(request.UpdateTicketDto, ticket);
        
        _unitOfWork.TicketRepository.Update(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}