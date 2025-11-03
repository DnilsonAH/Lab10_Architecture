using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports;
using ArchitectureLAB10.Domain.Ports.IServices;
using AutoMapper;
using MediatR;

namespace ArchitectureLAB10.Application.Features.Users.Commands;

public class RegisterUserCommand : IRequest<AuthResponseDto>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Email { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            throw new Exception("El nombre de usuario ya existe.");
        }
        
        var defaultRole = (await _unitOfWork.RoleRepository.FindAsync(r => r.RoleName == "User"))
            .FirstOrDefault();
        
        if (defaultRole == null)
        {
            throw new Exception("Rol 'User' no encontrado. Por favor,d).");
        }
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = hashedPassword,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };
        
        var userRole = new UserRole
        {
            UserId = user.UserId,
            RoleId = defaultRole.RoleId
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.UserRoleRepository.AddAsync(userRole);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var roles = new List<string> { defaultRole.RoleName };
        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        var userDto = _mapper.Map<UserDto>(user);

        return new AuthResponseDto
        {
            User = userDto,
            Token = token
        };
    }
}