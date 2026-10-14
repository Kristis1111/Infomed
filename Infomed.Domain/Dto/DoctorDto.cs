namespace Infomed.Domain.Dto;

public record DoctorDto
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public int PromotionLevel { get; init; }
    public bool IsActive { get; init; }
    public double AverageRating { get; init; }
    public int TotalRatings { get; init; }

    public IEnumerable<string> LanguageIds { get; init; } = [];
    public IEnumerable<string> Phones { get; init; } = [];
    public IEnumerable<DoctorClinicDto> Clinics { get; init; } = [];
}

public record DoctorClinicDto(string? CityName, string? Street);