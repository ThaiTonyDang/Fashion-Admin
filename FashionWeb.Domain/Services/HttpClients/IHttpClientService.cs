using System.Net.Mime;

namespace FashionWeb.Domain.Services.HttpClients
{
    public interface IHttpClientService
    {
        string GetBaseUrl();
        string GetFileApiUrl(string fileName);
        Task<T> GetAsync<T>(string url);
        Task<TResult> PostAsync<TBody, TResult>(TBody body, string url, string contentType = MediaTypeNames.Application.Json);
    }
}
