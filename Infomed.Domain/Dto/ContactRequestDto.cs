using System.ComponentModel.DataAnnotations;

namespace Infomed.Domain.Dto;

public record ContactRequestDto
{
    [Required(ErrorMessage = "יש להזין שם מלא")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "השם חייב להכיל בין 2 ל-100 תווים")]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "יש להזין מספר טלפון")]
    [RegularExpression(@"^0(5\d|[23489]|7\d)-?\d{7}$", ErrorMessage = "מספר הטלפון אינו תקין")]
    public string Phone { get; init; } = string.Empty;

    [Required(ErrorMessage = "יש להזין כתובת אימייל")]
    [EmailAddress(ErrorMessage = "כתובת האימייל אינה תקינה")]
    [StringLength(254, ErrorMessage = "כתובת האימייל ארוכה מדי")]
    public string Email { get; init; } = string.Empty;
}
