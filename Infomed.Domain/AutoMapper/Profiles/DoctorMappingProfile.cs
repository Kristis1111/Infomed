using AutoMapper;
using Infomed.Domain.Dto;
using Infomed.Domain.Entities;

namespace Infomed.Domain.AutoMapper.Profiles;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<DoctorEntity, DoctorDto>()
            .ForMember(destination => destination.AverageRating,
                options => options.MapFrom(source => source.Reviews.AverageRating))
            .ForMember(destination => destination.TotalRatings,
                options => options.MapFrom(source => source.Reviews.TotalRatings))
            .ForMember(destination => destination.Phones,
                options => options.MapFrom(source => source.Phones
                    .Select(phone => FormatPhoneNumber(phone.Number))
                    .Where(number => number.Length > 0)
                    .Distinct()
                    .ToArray()))
            .ForMember(destination => destination.Clinics,
                options => options.MapFrom(source => source.Clinics
                    .Where(clinic => clinic.Address != null)
                    .Select(clinic => new DoctorClinicDto(
                        clinic.Address!.CityName,
                        clinic.Address.Street))
                    .ToArray()));
    }
    private string FormatPhoneNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var digits = string.Concat(number.Where(char.IsDigit));
        var prefixLength = digits.StartsWith("05", StringComparison.Ordinal) ||
                           digits.StartsWith("07", StringComparison.Ordinal)
            ? 3
            : digits.StartsWith('0') ? 2 : 0;

        return prefixLength == 0 || digits.Length <= prefixLength
            ? digits
            : $"{digits[..prefixLength]}-{digits[prefixLength..]}";
    }
}
