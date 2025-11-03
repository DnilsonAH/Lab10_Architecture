using AutoMapper;
using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Entities;

namespace ArchitectureLAB10.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // entity -> dto
        CreateMap<User, UserDto>();
        CreateMap<Ticket, TicketDto>();
        
        // dto -> entity
        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<UpdateTicketDto, Ticket>();
    }
}