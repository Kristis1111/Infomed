using System.ComponentModel.DataAnnotations;

namespace Infomed.Domain.Dto;

public class ChatRequestDto
{
    [Required]
    [StringLength(2000, MinimumLength = 1)]
    public string Message { get; init; } = string.Empty;
}