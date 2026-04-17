namespace Genovel.Shared.Patterns;

public record Pagination(bool HasNextPage, bool HasPreviousPage, long TotalItems, long PageSize, long PageNumber);

