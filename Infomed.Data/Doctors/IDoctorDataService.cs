using Infomed.Domain.Entities;

namespace Infomed.Data.Doctors;

public interface IDoctorDataService
{
    Task<IEnumerable<DoctorEntity>> GetAllAsync(CancellationToken cancellationToken);
}
