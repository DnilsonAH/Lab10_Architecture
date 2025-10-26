namespace ArchitectureLAB10.Application.DTOs;

// DTO para la respuesta de login/registro
public class AuthResponseDto
{
    public UserDto User { get; set; } = null!;
    public string Token { get; set; } = null!;
}