using Infomed.Domain.Entities;

namespace Infomed.Data.Doctors;

public interface IContactRequestDataService
{
    Task AddAsync(ContactRequestEntity request, CancellationToken cancellationToken);
}
