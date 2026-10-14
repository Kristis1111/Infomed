using AutoMapper;
using Infomed.Core.Caching;
using Infomed.Data.Doctors;
using Infomed.Domain.Dto;
using Infomed.Domain.Entities;
using Infomed.Domain.Queries;
using Infomed.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Infomed.Services.Doctors;

public class DoctorService(IDoctorDataService doctorDataService, IMapper mapper, IApplicationCache applicationCache,
    IContactRequestDataService contactRequestDataService, ILogger<DoctorService> logger) : IDoctorService
{
    public async Task<PagedResult<DoctorDto>> GetDoctorsAsync(DoctorSearchQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = $"doctors:{System.Text.Json.JsonSerializer.Serialize(query)}";

        return await applicationCache.GetOrCreateAsync(
            cacheKey,
            token => LoadDoctorsAsync(query, token),
            TimeSpan.FromMinutes(5),
            cancellationToken);
    }

    public async Task<bool> SaveContactRequestAsync(int doctorId, ContactRequestDto request, CancellationToken cancellationToken)
    {
        var doctors = await doctorDataService.GetAllAsync(cancellationToken);
        if (!doctors.Any(doctor => doctor.Id == doctorId))
        {
            return false;
        }

        var contactRequest = mapper.Map<ContactRequestEntity>(request, options =>
            options.Items["DoctorId"] = doctorId);
        await contactRequestDataService.AddAsync(contactRequest, cancellationToken);

        logger.LogInformation("Saved contact request for doctor {DoctorId}.", doctorId);
        return true;
    }

    private async Task<PagedResult<DoctorDto>> LoadDoctorsAsync(DoctorSearchQuery query, CancellationToken cancellationToken)
    {
        var doctors = await doctorDataService.GetAllAsync(cancellationToken);
        var filteredDoctors = FilterDoctors(doctors, query).ToArray();
        var orderedDoctors = OrderDoctors(filteredDoctors, query.SortByRating);
        logger.LogDebug(
            "Loaded doctor page {Page} with page size {PageSize}; {TotalCount} doctors matched.",
            query.Page,
            query.PageSize,
            filteredDoctors.Length);
        var pageOffset = ((long)query.Page - 1) * query.PageSize;
        var pagedDoctors = pageOffset >= filteredDoctors.Length
            ? Array.Empty<DoctorEntity>()
            : orderedDoctors.Skip((int)pageOffset).Take(query.PageSize);

        var items = mapper.Map<DoctorDto[]>(pagedDoctors);

        return new PagedResult<DoctorDto>(items, filteredDoctors.Length, query.Page, query.PageSize);
    }

    private IEnumerable<DoctorEntity> FilterDoctors(IEnumerable<DoctorEntity> doctors, DoctorSearchQuery query)
    {
        return doctors
            .Where(doctor => string.IsNullOrWhiteSpace(query.Name) ||
                             doctor.FullName.Contains(query.Name.Trim(), StringComparison.OrdinalIgnoreCase))
            .Where(doctor => string.IsNullOrWhiteSpace(query.Place) ||
                             doctor.Clinics.Any(clinic =>
                                 clinic.Address is not null &&
                                 ((clinic.Address.CityName?.Contains(query.Place.Trim(), StringComparison.OrdinalIgnoreCase) ?? false) ||
                                  (clinic.Address.Street?.Contains(query.Place.Trim(), StringComparison.OrdinalIgnoreCase) ?? false))))
            .Where(doctor => !query.LanguageId.HasValue ||
                             doctor.LanguageIds.Contains(query.LanguageId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)))
            .Where(doctor => !query.ActiveOnly || doctor.IsActive)
            .Where(doctor => !query.PaidOnly || doctor.PromotionLevel <= 5);
    }

    private IOrderedEnumerable<DoctorEntity> OrderDoctors(IEnumerable<DoctorEntity> doctors, bool sortByRating)
    {
        if (sortByRating)
        {
            return doctors
                .OrderByDescending(doctor => doctor.Reviews.AverageRating)
                .ThenByDescending(doctor => doctor.Reviews.TotalRatings)
                .ThenBy(doctor => doctor.PromotionLevel)
                .ThenBy(doctor => doctor.Id);
        }

        return doctors
            .OrderBy(doctor => doctor.PromotionLevel)
            .ThenBy(doctor => doctor.Id);
    }
}
