namespace WebStore.Domain.ViewModels;

public record PageViewModel
{
    public int Page { get; init; }
    
    public int PageSize { get; init; }

    public int TotalPages { get; init; }
}
