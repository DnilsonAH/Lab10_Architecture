namespace ArchitectureLAB10.Application.DTOs;

public class UpdateTicketDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!; // Admin puede cambiar el estado
}