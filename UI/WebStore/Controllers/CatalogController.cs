using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers;

public class CatalogController : Controller
{
    private readonly IProductData _ProductData;
    private readonly IConfiguration _Configuration;

    public CatalogController(IProductData ProductData, IConfiguration Configuration)
    {
        _ProductData = ProductData;
        _Configuration = Configuration;
    }

    public IActionResult Index(int? SectionId, int? BrandId, int PageNumber = 1, int? PageSize = null)
    {
        var page_size = PageSize
            ?? (int.TryParse(_Configuration["CatalogPageSize"], out var value) ? value : null);

        var filter = new ProductFilter
        {
            BrandId = BrandId,
            SectionId = SectionId,
            PageNumber = PageNumber,
            PageSize = page_size,
        };

        var (products, _, _, total_count) = _ProductData.GetProducts(filter);

        return View(new CatalogViewModel
        {
            SectionId = SectionId,
            BrandId = BrandId,
            Products = products
                .OrderBy(p => p.Order)
                .ToView()!,
        });
    }

    public IActionResult Details(int Id)
    {
        var product = _ProductData.GetProductById(Id);

        if(product is null)
            return NotFound();

        return View(product.ToView());
    }
}
