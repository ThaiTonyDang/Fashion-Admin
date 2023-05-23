using FashionWeb.Domain.ResponseModel;
using Microsoft.AspNetCore.Http;

namespace FashionWeb.Domain.Services
{
    public interface IFileService
    {
        public Task<Tuple<ResponseApi<List<string>>, string>> GetResponeUploadFileAsync(IFormFile file, HttpClient httpClient);
    }
}
