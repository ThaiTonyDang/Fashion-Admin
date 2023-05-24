using FashionWeb.Domain.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FashionWeb.Admin.Extensions
{
    public static class TokenServiceExtensions
    {
        public static void AddIdentityTokenConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfig = configuration.GetSection("Token");
            services.Configure<TokenConfig>(tokenConfig);
        }
    }
}
