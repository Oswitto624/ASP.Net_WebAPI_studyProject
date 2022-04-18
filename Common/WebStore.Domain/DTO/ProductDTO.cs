namespace WebStore.Domain.DTO;

public class ProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public SectionDTO Section { get; set; } = null!;

    public BrandDTO? Brand { get; set; }

    public string ImageUrl { get; set; } = null!;

    public decimal Price { get; set; }
}

public class SectionDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public int? ParentId { get; set; }
}

public class BrandDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }
}