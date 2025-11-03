using ArchitectureLAB10.Application.DTOs;
using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports;
using ArchitectureLAB10.Domain.Ports.IServices;
using AutoMapper;
using MediatR;

namespace ArchitectureLAB10.Application.UseCases.Users.Commands;

// 1. El Comando (Input)
public class LoginUserCommand : IRequest<AuthResponseDto>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(
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

    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
        if (user == null)
        {
            throw new Exception("Usuario o contraseña incorrectos.");
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Usuario o contraseña incorrectos.");
        }
        
        var userRoles = await _unitOfWork.UserRoleRepository.GetRolesByUserIdAsync(user.UserId);
        var roles = userRoles.Select(ur => ur.Role.RoleName).ToList();

        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        var userDto = _mapper.Map<UserDto>(user);

        return new AuthResponseDto
        {
            User = userDto,
            Token = token
        };
    }
}