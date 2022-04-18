using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly IProductData _ProductData;
    private readonly ILogger<ProductsApiController> _Logger;

    public ProductsApiController(IProductData ProductData, ILogger<ProductsApiController> Logger)
    {
        _ProductData = ProductData;
        _Logger = Logger;
    }

    [HttpGet("sections")]  //GET -> http://localhost:****/api/products/sections
    public IActionResult GetSections()
    {
        var sections = _ProductData.GetSections();
        return Ok(sections);
    }

    [HttpGet("sections/{Id}")]
    public IActionResult GetSectionById(int Id)
    {
        var section = _ProductData.GetSectionById(Id);
        
        if (section is null)
            return NotFound(); 
       
        return Ok(section);
    }

    [HttpGet("brands")]  //GET -> http://localhost:****/api/products/brands
    public IActionResult GetBrands()
    {
        var brands = _ProductData.GetBrands();
        return Ok(brands);
    }

    [HttpGet("brands/{Id}")]
    public IActionResult GetBrandById(int Id)
    {
        var brand = _ProductData.GetBrandById(Id);
       
        if (brand is null)
            return NotFound(); 
       
        return Ok(brand);
    }

    [HttpPost]
    public IActionResult GetProducts(ProductFilter? Filter = null)
    {
        var products = _ProductData.GetProducts(Filter);
        return Ok(products);
    }

    [HttpGet("{Id}")]
    public IActionResult GetProductsById(int Id)
    {
        var product = _ProductData.GetProductById(Id);
        
        if (product is null)
            return NotFound();
        
        return Ok(product);
    }
}
