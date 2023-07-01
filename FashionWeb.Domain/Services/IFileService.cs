using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.ResponseModel;
using Microsoft.AspNetCore.Http;

namespace FashionWeb.Domain.Services
{
    public interface IFileService
    {
        Task<ResultDto> UploadFileAsync(MultipartFormDataContent file, string token);
    }
}
