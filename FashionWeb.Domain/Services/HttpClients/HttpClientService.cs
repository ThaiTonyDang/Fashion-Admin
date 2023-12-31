﻿using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.ResponseModel;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace FashionWeb.Domain.Services.HttpClients
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ApiConfig _hostAPIConfig;
        private readonly HttpClient _httpClient;
        public HttpClientService(IOptions<ApiConfig> options, HttpClient httpClient)
        {
            _hostAPIConfig = options.Value;
            _httpClient = httpClient;
        }
        public string GetBaseUrl()
        {
            return _hostAPIConfig.Url;
        }

        public string GetFileApiUrl(string fileName)
        {
            var fileUrl = GetBaseUrl() + "/resource/" + fileName;
            return fileUrl;
        }

        public async Task<BaseReponseApi> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result =  JsonSerializer.Deserialize<ResponseApi<T>>(jsonString);
                return result;
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi<string[]>>(jsonString);
            return error;
        }

        public async Task<BaseReponseApi> GetDataAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseApiData<T>>(jsonString);
                return result;
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi<string[]>>(jsonString);
            return error;
        }

        public async Task<BaseReponseApi> PostAsync<TBody,TResult>(TBody body, string url, string contentType = MediaTypeNames.Application.Json)
        {
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            var jsonString = await response.Content.ReadAsStringAsync();
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ResponseApiData<TResult>>(jsonString);
                return result;
            }

            var error = JsonSerializer.Deserialize<ErrorResponseApi<string[]>>(jsonString);
            return error;
        }
    }
}
