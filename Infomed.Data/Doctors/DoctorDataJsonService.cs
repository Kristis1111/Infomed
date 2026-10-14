using Infomed.Domain.Entities;
using System.Text.Json;

namespace Infomed.Data.Doctors;

public class DoctorDataJsonService : IDoctorDataService
{
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "doctors.json");
    private readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };


    public async Task<IEnumerable<DoctorEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(_filePath);

        var doctors = await JsonSerializer.DeserializeAsync<List<DoctorEntity>>(
            stream,
            SerializerOptions,
            cancellationToken)
            ?? throw new JsonException("Doctor data must contain a JSON array.");

        return doctors;
    }
}
