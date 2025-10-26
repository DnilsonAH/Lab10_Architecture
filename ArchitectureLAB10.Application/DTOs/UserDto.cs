namespace ArchitectureLAB10.Application.DTOs;

// DTO para mostrar información del usuario
public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
}