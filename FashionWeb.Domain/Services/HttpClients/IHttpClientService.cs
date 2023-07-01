using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;
using System.Net.Mime;

namespace FashionWeb.Domain.Services.HttpClients
{
    public interface IHttpClientService
    {
        Task<ResultDto> GetAsync(string pathApiUrl, string token = "");
        Task<ResultDto> GetDataAsync<TResult>(string pathApiUrl, string token = "");
        Task<ResultDto> PostAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PostDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PatchAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PatchDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PutAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> PutDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json);
        Task<ResultDto> UploadAsync<TResult>(MultipartFormDataContent file, string pathApiUrl, string token = "");
        string GetFileApiUrl(string fileName);
    }
}
