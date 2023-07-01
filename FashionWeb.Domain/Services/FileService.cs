using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model.Files;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FashionWeb.Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiResource = "file";
        public FileService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public async Task<ResultDto> UploadFileAsync(MultipartFormDataContent file, string token)
        {
            var url = $"{_apiResource}/upload";
            var response = await _httpClientService.UploadAsync<FileUpload>(file, url, token);
            return response;
        }
    }
}
