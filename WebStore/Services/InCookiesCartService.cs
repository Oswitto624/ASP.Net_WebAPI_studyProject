using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services;

public class InCookiesCartService : ICartService
{
    private readonly IHttpContextAccessor _HttpContextAccessor;
    private readonly IProductData _ProductData;
    private readonly string _CartName;

    private Cart Cart
    {
        get
        {
            var context = _HttpContextAccessor.HttpContext;
            var cookies = context!.Response.Cookies;

            var cart_cookie = context.Request.Cookies[_CartName];
            if (cart_cookie is null)
            {
                var cart = new Cart();
                cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                return cart;
            }
            ReplaceCart(cookies, cart_cookie);
            return JsonConvert.DeserializeObject<Cart>(cart_cookie);
        }
        set => ReplaceCart(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
    }

    private void ReplaceCart(IResponseCookies cookies, string cart)
    {
        cookies.Delete(_CartName);
        cookies.Append(_CartName, cart);
    }

    public InCookiesCartService(IHttpContextAccessor HttpContextAccessor, IProductData ProductData)
    {
        _HttpContextAccessor = HttpContextAccessor;
        _ProductData = ProductData;

        var user = _HttpContextAccessor.HttpContext!.User;
        var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null ;
        _CartName = $"WebStore.GB.Cart{user_name}";
    }

    public void Add(int Id)
    {
        var cart = Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
        if (item is null)
            cart.Items.Add(new() { ProductId = Id });
        else
            item.Quantity++;

        Cart = cart;
    }
    public void Decrement(int Id)
    {
        var cart = Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
        if (item is null)
            return;

        if (item.Quantity > 0)
            item.Quantity--;

        if (item.Quantity <= 0)
            cart.Items.Remove(item);

        Cart = cart;
    }
    public void Remove(int Id)
    {
        var cart = Cart;
        var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
        if (item is null)
            return;

        cart.Items.Remove(item);

        Cart = cart;
    }

    public void Clear()
    {
        //Cart = new();

        var cart = Cart;

        cart.Items.Clear(); 

        Cart = cart;
    }


    public CartViewModel GetViewModel()
    {
       
    }

}
