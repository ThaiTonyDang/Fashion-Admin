using FashionWeb.Domain.Services;
using FashionWeb.Infrastructure.Config;
using FashionWeb.Infrastructure.DataContext;
using FashionWeb.Infrastructure.FileHelpers;
using FashionWeb.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			services.AddControllersWithViews();

			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IFileService, FileService>();

			services.Configure<FileConfig>(Configuration.GetSection("FileConfig"));
			services.AddDbContext<AppDbContext>(x =>
											   x.UseSqlServer(Configuration.GetConnectionString("FashionWeb")));
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
			var fileConfig = Configuration.GetSection("FileConfig");

			if (fileConfig.Get<FileConfig>() != null)
			{
				var path = fileConfig.Get<FileConfig>().ImagePath;
				app.UseStaticFiles(new StaticFileOptions
				{
					FileProvider = new PhysicalFileProvider(path),
				});
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
