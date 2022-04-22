using System.Net;
using System.Net.Http.Json;

namespace WebStore.WebAPI.Clients.Base;

public abstract class BaseClient : IDisposable
{
    protected HttpClient Http { get; }

    protected string Address { get; }

    protected BaseClient(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }

    protected T? Get<T>(string url) => GetAsync<T>(url).Result;
    protected async Task<T?> GetAsync<T>(string url, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync(url, Cancel).ConfigureAwait(false);
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.NotFound:
                return default;
            default:
                {
                    var result = await response
                        .EnsureSuccessStatusCode()
                        .Content
                        .ReadFromJsonAsync<T>(cancellationToken: Cancel)
                        .ConfigureAwait(false);
                    return result;
                }
        }
    }

    protected HttpResponseMessage Post<T>(string url, T value) => PostAsync<T>(url, value).Result;
    protected async Task<HttpResponseMessage> PostAsync<T>(string url, T value, CancellationToken Cancel = default)
    {
        var response = await Http.PostAsJsonAsync(url, value, Cancel).ConfigureAwait(false);
        return response.EnsureSuccessStatusCode();
    }

    protected HttpResponseMessage Put<T>(string url, T value) => PutAsync<T>(url, value).Result;
    protected async Task<HttpResponseMessage> PutAsync<T>(string url, T value, CancellationToken Cancel = default)
    {
        var response = await Http.PutAsJsonAsync(url, value, Cancel).ConfigureAwait(false);
        return response.EnsureSuccessStatusCode();
    }

    protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
    protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken Cancel = default)
    {
        var response = await Http.DeleteAsync(url, Cancel).ConfigureAwait(false);
        return response;
    }

    //~BaseClient() => Dispose(false);
    public void Dispose()
    {
        if (_Disposed) return;
        Dispose(true);
        _Disposed = true;
        //GC.SuppressFinalize(this);  // Нужно при наличии ~BaseClient()
    }

    private bool _Disposed;
    protected virtual void Dispose(bool Disposing)
    {
        if (_Disposed) return;

        if (Disposing)
        {
            // освободить все управляемые ресурсы -> вызвать .Dispose() везде где нужно - у всего, что было создано в рамках этого экземпляра с помощью "new"
            //Http.Dispose(); !!! - уничтожать нельзя!
        }

        // освобождение неуправляемых ресурсов
    }
}
