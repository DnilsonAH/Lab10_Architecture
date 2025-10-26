using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports;
using ArchitectureLAB10.Domain.Ports.IServices;
using AutoMapper;
using MediatR;

namespace ArchitectureLAB10.Application.Features.Users.Commands;

// Define el "comando" (los datos de entrada) y el tipo de respuesta (AuthResponseDto)
public class RegisterUserCommand : IRequest<AuthResponseDto>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Email { get; set; }
}

// Define el "manejador" (la lógica del caso de uso)
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
        // 1. Validar si el usuario ya existe
        var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            throw new Exception("El nombre de usuario ya existe."); // Deberías usar excepciones customizadas
        }

        // 2. Encriptar la contraseña
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        // 3. Crear la nueva entidad User
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = hashedPassword,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Agregar al repositorio y guardar
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Generar el token JWT
        var token = _jwtTokenGenerator.GenerateToken(user);

        // 6. Mapear a DTOs y devolver la respuesta
        var userDto = _mapper.Map<UserDto>(user);

        return new AuthResponseDto
        {
            User = userDto,
            Token = token
        };
    }
}