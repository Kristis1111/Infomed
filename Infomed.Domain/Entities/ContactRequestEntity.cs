namespace Infomed.Domain.Entities;

public record ContactRequestEntity
{
    public Guid Id { get; init; }
    public int DoctorId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
}
