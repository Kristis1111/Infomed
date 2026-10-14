using Infomed.Core.Caching;
using Infomed.Data.Doctors;
using Infomed.Domain.Entities;
using System.Globalization;

namespace Infomed.Data.Autocomplete;

public class AutocompleteDataJsonService(IDoctorDataService doctorRepository, IApplicationCache applicationCache) : IAutocompleteDataService
{
    private readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public Task<IEnumerable<DoctorEntity>> GetExpertsAsync(CancellationToken cancellationToken)
    {
        return applicationCache.GetOrCreateAsync(
            "autocomplete:experts",
            async token =>
            {
                var doctors = await GetDoctorsAsync(token);
                return (IEnumerable<DoctorEntity>)doctors
                    .Where(doctor => !string.IsNullOrWhiteSpace(doctor.FullName))
                    .OrderBy(doctor => doctor.FullName, StringComparer.Ordinal)
                    .ThenBy(doctor => doctor.Id)
                    .ToArray();
            },
            CacheDuration,
            cancellationToken);
    }

    public Task<IEnumerable<CityEntity>> GetAreasAndCitiesAsync(CancellationToken cancellationToken)
    {
        return applicationCache.GetOrCreateAsync(
            "autocomplete:cities",
            async token =>
            {
                var doctors = await GetDoctorsAsync(token);
                var cityRecords = doctors
                    .SelectMany(doctor => doctor.Clinics)
                    .Select(clinic => clinic.Address)
                    .Where(address => address is not null && !string.IsNullOrWhiteSpace(address.CityName))
                    .Select(address => new { Name = address!.CityName!.Trim(), address.CityId })
                    .GroupBy(city => city.Name, StringComparer.OrdinalIgnoreCase)
                    .Select(group => new
                    {
                        Name = group.First().Name,
                        Id = group.Select(city => int.TryParse(city.CityId, NumberStyles.None, CultureInfo.InvariantCulture, out var id) ? id : (int?)null)
                            .FirstOrDefault(id => id.HasValue)
                    })
                    .OrderBy(city => city.Name, StringComparer.Ordinal)
                    .ToArray();

                var nextId = cityRecords
                    .Where(city => city.Id.HasValue)
                    .Select(city => city.Id!.Value)
                    .DefaultIfEmpty(0)
                    .Max();

                return (IEnumerable<CityEntity>)cityRecords
                    .Select(city => new CityEntity { Id = city.Id ?? ++nextId, Name = city.Name })
                    .ToArray();
            },
            CacheDuration,
            cancellationToken);
    }

    private Task<IEnumerable<DoctorEntity>> GetDoctorsAsync(CancellationToken cancellationToken)
    {
        return applicationCache.GetOrCreateAsync(
            "autocomplete:doctor-source",
            async token => (IEnumerable<DoctorEntity>)(await doctorRepository.GetAllAsync(token)).ToArray(),
            CacheDuration,
            cancellationToken);
    }
}
