using Microsoft.AspNetCore.Http;

namespace FashionWeb.Domain.Services
{
    public interface IFileService
    {
        public Task<List<string>> UploadFileAsync(IFormFile file, HttpClient httpClient);
    }
}
