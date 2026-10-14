using Infomed.Core.Caching;
using Infomed.Domain.Dto;
using System.Globalization;
using System.Text.Json;

namespace Infomed.Data.DropDowns;

public class DropDownDataService(IApplicationCache applicationCache) : IDropDownDataService
{
    private readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    private readonly string _languagesFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "language.json");
    private readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Task<IEnumerable<AutocompleteOptionDto>> GetLanguagesAsync(CancellationToken cancellationToken)
    {
        return applicationCache.GetOrCreateAsync(
            "dropdown:languages",
            async token =>
            {
                await using var stream = File.OpenRead(_languagesFilePath);
                var data = await JsonSerializer.DeserializeAsync<Dictionary<string, Dictionary<string, string>>>(
                    stream,
                    SerializerOptions,
                    token)
                    ?? throw new JsonException("Language data must contain a language object.");

                if (!data.TryGetValue("language", out var languages))
                {
                    throw new JsonException("Language data must contain a 'language' object.");
                }

                return (IEnumerable<AutocompleteOptionDto>)languages
                    .Select(language => int.TryParse(language.Key, NumberStyles.None, CultureInfo.InvariantCulture, out var id)
                        ? new AutocompleteOptionDto(id, language.Value)
                        : throw new JsonException($"Language ID '{language.Key}' must be an integer."))
                    .OrderBy(language => language.Id)
                    .ToArray();
            },
            CacheDuration,
            cancellationToken);
    }
}
