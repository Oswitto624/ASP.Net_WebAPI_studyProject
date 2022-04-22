using System.Net.Http.Json;
using WebStore.Interfaces;
using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values;

public class ValuesClient : BaseClient, IValuesService
{
    public ValuesClient(HttpClient Client) : base(Client, WebAPIAddresses.V1.Values) { }

    public IEnumerable<string> GetValues()
    {
        var response = Http.GetAsync(Address).Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result!;

        return Enumerable.Empty<string>();
    }
    
    public int Count()
    {
        var response = Http.GetAsync($"{Address}/count").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<int>().Result;
        return -1;
    }

    public string GetById(int id)
    {
        var response = Http.GetAsync($"{Address}/{id}").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<string>().Result;
        return null;
    }

    public void Add(string Value)
    {
        var response = Http.PostAsJsonAsync(Address, Value).Result;
        response.EnsureSuccessStatusCode();
    }

    public void Edit(int id, string value)
    {
        var response = Http.PostAsJsonAsync($"{Address}/{id}", value).Result;
        response.EnsureSuccessStatusCode();
    }

    public bool Delete(int id)
    {
        var response = Http.DeleteAsync($"{Address}/{id}").Result;
        return response.IsSuccessStatusCode;
    }



}
