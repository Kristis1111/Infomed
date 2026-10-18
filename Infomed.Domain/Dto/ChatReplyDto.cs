namespace Infomed.Domain.Dto;

public record ChatReplyDto(string Reply, IReadOnlyList<DoctorDto> Doctors);
