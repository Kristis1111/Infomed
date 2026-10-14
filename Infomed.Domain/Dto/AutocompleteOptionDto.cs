namespace Infomed.Domain.Dto;

public record AutocompleteOptionDto
{

    public AutocompleteOptionDto()
    {
    }

    public AutocompleteOptionDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
