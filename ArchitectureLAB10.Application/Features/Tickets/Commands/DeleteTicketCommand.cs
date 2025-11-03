using ArchitectureLAB10.Domain.Ports;
using MediatR;

namespace ArchitectureLAB10.Application.Features.Tickets.Commands;

public class DeleteTicketCommand : IRequest
{
    public Guid TicketId { get; set; }
}

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.TicketRepository.GetByIdAsync(request.TicketId);
        if (ticket == null)
        {
            throw new Exception("Ticket no encontrado.");
        }
        
        _unitOfWork.TicketRepository.Delete(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}