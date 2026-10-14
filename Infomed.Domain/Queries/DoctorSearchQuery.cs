using System.ComponentModel.DataAnnotations;

namespace Infomed.Domain.Queries
{
    public class DoctorSearchQuery
    {
        public string? Name { get; init; }
        public string? Place { get; init; }
        public int? LanguageId { get; init; }
        public bool ActiveOnly { get; init; }
        public bool PaidOnly { get; init; }
        public bool SortByRating { get; init; }

        [Range(1, int.MaxValue)]
        public int Page { get; init; } = 1;

        [Range(1, 100)]
        public int PageSize { get; init; } = 20;
    }

}
