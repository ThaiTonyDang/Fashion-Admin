using FashionWeb.Admin.Extensions;
using FashionWeb.Domain.HostConfig;
using FashionWeb.Domain.Services;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.Services.Jwts;
using FashionWeb.Domain.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(options =>
					{
						options.LoginPath = "/users/login";
						options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
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

            services.AddSession(options =>
             {
                 options.IdleTimeout = TimeSpan.FromMinutes(720);
                 options.Cookie.HttpOnly = true;
                 options.Cookie.IsEssential = true;
             });

            services.Configure<ApiConfig>(Configuration.GetSection("Api"));
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
