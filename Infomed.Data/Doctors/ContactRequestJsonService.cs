using Infomed.Domain.Entities;
using System.Text.Json;

namespace Infomed.Data.Doctors;

public class ContactRequestJsonService : IContactRequestDataService
{
    private readonly SemaphoreSlim FileLock = new(1, 1);
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "contact-requests.json");
    private readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };


    public async Task AddAsync(ContactRequestEntity request, CancellationToken cancellationToken)
    {
        await FileLock.WaitAsync(cancellationToken);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            var requests = new List<ContactRequestEntity>();
            if (File.Exists(_filePath))
            {
                await using var readStream = File.OpenRead(_filePath);
                if (readStream.Length > 0)
                {
                    requests = await JsonSerializer.DeserializeAsync<List<ContactRequestEntity>>(
                        readStream, SerializerOptions, cancellationToken) ?? new List<ContactRequestEntity>();
                }
            }

            requests.Add(request);

            var tempPath = _filePath + ".tmp";
            await using (var writeStream = File.Create(tempPath))
            {
                await JsonSerializer.SerializeAsync(writeStream, requests, SerializerOptions, cancellationToken);
            }

            File.Move(tempPath, _filePath, overwrite: true);
        }
        finally
        {
            FileLock.Release();
        }
    }
}
