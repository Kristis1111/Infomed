namespace Infomed.Domain.Entities;


public class DoctorEntity
{
    public int Id { get; init; }
    public int PromotionLevel { get; init; }
    public IEnumerable<DoctorPhoneEntity> Phones { get; init; } = [];
    public IEnumerable<DoctorClinicEntity> Clinics { get; init; } = [];
    public DoctorReviewsEntity Reviews { get; init; } = new();
    public string FullName { get; init; } = string.Empty;
    public IEnumerable<string> LanguageIds { get; init; } = [];
    public bool IsActive { get; init; }
}

public class DoctorPhoneEntity
{
    public string Number { get; init; } = string.Empty;
    public int PhoneType { get; init; }
}

public class DoctorClinicEntity
{
    public DoctorAddressEntity? Address { get; init; }
}

public class DoctorAddressEntity
{
    public string? CityName { get; init; }
    public string? CityId { get; init; }
    public string? Street { get; init; }
}

public class DoctorReviewsEntity
{
    public double AverageRating { get; init; }
    public int TotalRatings { get; init; }
}

