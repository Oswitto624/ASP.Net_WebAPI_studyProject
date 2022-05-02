using WebStore.Interfaces;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Console;

public class ConsoleClient : BaseClient
{
    public ConsoleClient(HttpClient Client) : base(Client, WebAPIAddresses.V1.Console) { }

    public void Clear()
    {
        Http.GetAsync("clear").Wait();
    }

    public void WriteLine(string Str)
    {
        Http.GetAsync($"write?str={Str}").Wait();
    }

    public void SetTitle(string Str)
    {
        Http.GetAsync($"title?str={Str}").Wait();
    }
}
