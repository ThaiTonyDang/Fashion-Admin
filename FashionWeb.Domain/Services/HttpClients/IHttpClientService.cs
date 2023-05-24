using FashionWeb.Domain.Model;
using System.Net.Mime;

namespace FashionWeb.Domain.Services.HttpClients
{
    public interface IHttpClientService
    {
        string GetBaseUrl();
        string GetFileApiUrl(string fileName);
        Task<BaseReponseApi> GetAsync<T>(string url);
        Task<BaseReponseApi> PostAsync<TBody, TResult>(TBody body, string url, string contentType = MediaTypeNames.Application.Json);
    }
}
