namespace WebStore.Domain.ViewModels;

public class CatalogViewModel
{
    public int? BrandId { get; init; }
    public int? SectionId { get; init; }
    public IEnumerable<ProductViewModel> Products { get; init; } = Enumerable.Empty<ProductViewModel>();

    public PageViewModel PageModel { get; init; } = null!;
}
