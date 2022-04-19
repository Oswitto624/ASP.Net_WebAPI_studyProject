using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders;

public class OrdersClient : BaseClient, IOrderService
{
    public OrdersClient(HttpClient Client) : base(Client, "api/orders")
    {
    }

    public Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrderByIdAsync(int Id, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }

    public Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }


}
