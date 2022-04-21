using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity;

public class UsersClient : BaseClient
{
    public UsersClient(HttpClient Client) : base(Client, WebAPIAddresses.V1.Identity.Users) { }

    //protected override void Dispose(bool Disposing)
    //{
    //    base.Dispose(Disposing);

    //    if (Disposing)
    //    {

    //    }

    //}


}
