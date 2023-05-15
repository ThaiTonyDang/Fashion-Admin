using Microsoft.AspNetCore.Http;

namespace FashionWeb.Domain.Services
{
    public interface IUrlService
    {
        public string GetBaseUrl();
        public string GetFileApiUrl(string fileName);
    }
}
