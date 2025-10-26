using AutoMapper;
using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Entities;

namespace ArchitectureLAB10.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de User a UserDto
        CreateMap<User, UserDto>();
        
    }
}