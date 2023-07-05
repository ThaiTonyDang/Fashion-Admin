using FashionWeb.Admin.Extensions;
using FashionWeb.Domain.Config;
using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.Services;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.Services.Jwts;
using FashionWeb.Domain.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Plugins;
using System;

namespace FashionWeb.Admin
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddIdentityTokenConfig(Configuration);

            var expiredTime = Configuration.GetSection("Token").Get<TokenConfig>().Expired;
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(options =>
                    {
						options.LoginPath = "/users/login";
						options.AccessDeniedPath = "/users/access-denied";
						options.ExpireTimeSpan = TimeSpan.FromMinutes(expiredTime);
                        options.Cookie.Name = "Admin.Authentication";
                    });

            services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IJwtTokenService, JwtTokenService>();
			services.AddScoped<IHttpClientService, HttpClientService>();
			services.AddScoped<IFileService, FileService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddHttpClient();

			services.AddDistributedMemoryCache();

            services.AddSession(cfg =>
			{
                 cfg.Cookie.Name = "Fashion.Admin";
                 cfg.Cookie.IsEssential = true;
                 cfg.Cookie.HttpOnly = true;
                 cfg.Cookie.SameSite = SameSiteMode.Strict;
                 cfg.IdleTimeout = TimeSpan.FromMinutes(expiredTime);
             });

            services.Configure<ApiConfig>(Configuration.GetSection("Api"));
            services.Configure<PageConfig>(Configuration.GetSection("Page"));
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
