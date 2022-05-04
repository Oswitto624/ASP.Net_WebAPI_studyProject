﻿using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services;

public class CartService : ICartService
{
    private readonly ICartStore _CartStore;
    private readonly IProductData _ProductData;

    public CartService(ICartStore CartStore, IProductData ProductData)
    {
        _CartStore = CartStore;
        _ProductData = ProductData;
    }

    public void Add(int Id)
    {
        var cart = _CartStore.Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
        if (item is null)
            cart.Items.Add(new() { ProductId = Id });
        else
            item.Quantity++;

        _CartStore.Cart = cart;
    }
    public void Decrement(int Id)
    {
        var cart = _CartStore.Cart;

        var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
        if (item is null)
            return;

        if (item.Quantity > 0)
            item.Quantity--;

        if (item.Quantity <= 0)
            cart.Items.Remove(item);

        _CartStore.Cart = cart;
    }
    public void Remove(int Id)
    {
        var cart = _CartStore.Cart;
        var item = cart.Items.FirstOrDefault(item => item.ProductId == Id);
        if (item is null)
            return;

        cart.Items.Remove(item);

        _CartStore.Cart = cart;
    }

    public void Clear()
    {
        //_CartStore.Cart = new();

        var cart = _CartStore.Cart;

        cart.Items.Clear();

        _CartStore.Cart = cart;
    }


    public CartViewModel GetViewModel()
    {
        var cart = _CartStore.Cart;

        var products = _ProductData.GetProducts(new()
        {
            Ids = cart.Items.Select(i => i.ProductId).ToArray()
        });

        var products_views = products.Items.ToView().ToDictionary(p => p!.Id);

        return new()
        {
            Items = cart.Items
            .Where(item => products_views.ContainsKey(item.ProductId))
            .Select(item => (products_views[item.ProductId], item.Quantity))!,
        };
    }

}
