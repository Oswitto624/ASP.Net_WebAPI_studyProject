﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class CatalogController : Controller
{
    private readonly IProductData _ProductData;
    public CatalogController(IProductData ProductData) => _ProductData = ProductData; 

    public IActionResult Index(int? SectionId, int? BrandId)
    {
        var filter = new ProductFilter
        {
            BrandId = BrandId,
            SectionId = SectionId,
        };
        var products = _ProductData.GetProducts(filter);

        return View(new CatalogViewModel
        {
            SectionId = SectionId,
            BrandId = BrandId,
            Products = products
                .OrderBy(p => p.Order)
                .Select(p => new ProductViewModel{
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                }),
        });
    }

    public IActionResult Details(int Id)
    {
        var product = _ProductData.GetProductById(Id);

        if(product is null)
            return NotFound();

        return View(new ProductViewModel
        {
            Id = product.Id,
            Name= product.Name,
            Price= product.Price,
            ImageUrl= product.ImageUrl,
        });
    }
}
