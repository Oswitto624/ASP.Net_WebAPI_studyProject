using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Domain.DTO;

namespace WebStore.Tests.Controllers;

[TestClass]
public class HomeControllerIntegrationTests
{
    [TestMethod]
    public async Task Home_Index_HTML_Test()
    {
        var products = Enumerable.Range(1, 10)
           .Select(i => new Product
            {
                Id = i,
                Name = $"Product-{i}",
                Order = i,
                Price = i * 1000 + i,
                Section = new Section { Id = i, Name = $"Section-{i}" },
                ImageUrl = $"Image-{i}",
            })
           .ToArray();

        var product_data_mock = new Mock<IProductData>();
        product_data_mock
           .Setup(s => s.GetProducts(It.IsAny<ProductFilter>()))
           .Returns(new Page<Product>(products, 1, products.Length, products.Length));

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var product_data_http = services.First(s => s.ServiceType == typeof(IProductData));
                    services.Remove(product_data_http);
                    services.AddSingleton(product_data_mock.Object);
                });
            });

        var client = application.CreateClient();

        var resposnse = await client.GetAsync("/");

        var html_str = await resposnse.EnsureSuccessStatusCode()
            .Content
            .ReadAsStringAsync();

        var parser = new HtmlParser();
        var html = parser.ParseDocument(html_str);

        var product_cards = html.QuerySelectorAll(".single-products");
    }
}
