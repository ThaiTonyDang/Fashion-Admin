using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace FashionWeb.Domain.Services.HttpClients
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public HttpClientService(IOptions<ApiConfig> options, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiUrl = $"{options.Value.Url}";
        }

        public async Task<ResultDto> GetAsync(string pathApiUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var response = await _httpClient.GetAsync(url);
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> GetDataAsync<TResult>(string pathApiUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var response = await _httpClient.GetAsync(url);
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> PostAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }
            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> PostDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> PatchAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PatchAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> PatchDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PatchAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> PutAsync<TBody>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PutAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseAsync(response);
        }

        public async Task<ResultDto> PutDataAsync<TBody, TResult>(TBody body, string pathApiUrl, string token = "", string contentType = MediaTypeNames.Application.Json)
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PutAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public async Task<ResultDto> UploadAsync<TResult>(MultipartFormDataContent file, string pathApiUrl, string token = "")
        {
            if (!string.IsNullOrEmpty(token))
            {
                this.SetAuthenticationHeader(token);
            }

            var url = $"{_apiUrl}/api/{pathApiUrl}";
            var response = await _httpClient.PostAsync(url, file);
            return await this.GetResponseDataAsync<TResult>(response);
        }

        public string GetFileApiUrl(string fileName)
        {
            var fileUrl = $"{_apiUrl}/{HTTP.SLUG}/" + fileName;
            return fileUrl;
        }

        private async Task<ResultDto> GetResponseAsync(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseApi>(jsonString);
                return new SuccessResult(result.Message);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }

        private async Task<ResultDto> GetResponseDataAsync<TResult>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseDataApi<TResult>>(jsonString);
                return new SuccessDataResult<TResult>(result.Message, result.Data);
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi>(jsonString);
            return new ErrorResult(string.Join(", ", error.Errors));
        }

        private void SetAuthenticationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        }
    }
}
