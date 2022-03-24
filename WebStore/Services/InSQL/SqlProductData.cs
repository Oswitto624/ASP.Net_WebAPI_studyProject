using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL;

public class SqlProductData : IProductData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<SqlProductData> _Logger;

    public SqlProductData(WebStoreDB db, ILogger<SqlProductData> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public IEnumerable<Section> GetSections() => _db.Sections; 

    public IEnumerable<Brand> GetBrands() => _db.Brands;

    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IQueryable<Product> query = _db.Products;

        //if (Filter != null && Filter.SectionId != null)
        //    query = query.Where(p => p.SectionId == Filter.SectionId);

        if (Filter?.SectionId is { } section_id)
            query = query.Where(p => p.SectionId == Filter.SectionId);

        if (Filter?.BrandId is { } brand_id)
            query = query.Where(p => p.BrandId == Filter.BrandId);

        return query;
    }

}
