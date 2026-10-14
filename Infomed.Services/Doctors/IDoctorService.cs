using Infomed.Domain.Dto;
using Infomed.Domain.Queries;
using Infomed.Domain.Responses;

namespace Infomed.Services.Doctors;

public interface IDoctorService
{
    Task<PagedResult<DoctorDto>> GetDoctorsAsync(DoctorSearchQuery query, CancellationToken cancellationToken);
    Task<bool> SaveContactRequestAsync(int doctorId, ContactRequestDto request, CancellationToken cancellationToken);
}
