namespace WebStore.Domain.ViewModels;

public class SelectableSectionsViewModel
{
    public IEnumerable<SectionViewModel> Sections { get; init; } = null!;

    public int? SectionId { get; init; }

    public int? ParentSectionId { get; init; }
}
