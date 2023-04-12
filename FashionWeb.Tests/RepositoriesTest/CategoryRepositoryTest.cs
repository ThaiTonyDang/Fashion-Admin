using FashionWeb.Infrastructure.DataContext;
using FashionWeb.Infrastructure.Models;
using FashionWeb.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChelseaWeb.Tests.RepositoriesTest
{
	public class CategoryRepositoryTest
	{
		private readonly AppDbContext _dbContext;
		private readonly CategoryRepository _categoryRepository;
		public CategoryRepositoryTest()
		{
			var appContextOptions = new DbContextOptionsBuilder<AppDbContext>()
								   .UseInMemoryDatabase(databaseName: $"Ecommercy_{DateTime.Now.ToFileTimeUtc()}")
								   .Options;

			_dbContext = new AppDbContext(appContextOptions);
			_categoryRepository = new CategoryRepository(_dbContext);

			SeddingDataAsync(_dbContext)
							.ConfigureAwait(true)
							.GetAwaiter()
							.GetResult();
		}

		[Fact]
		public async Task Should_GetListCategories_Return_Success()
		{
			// Arrage
			var categoryListExpect = LoadCategorySampleData();

			// Act
			var categoryListAct = await _categoryRepository.Categories();

			// Assert
			Assert.Equal(categoryListExpect.Count, categoryListAct.Count);

			var firtItemExpect = categoryListExpect.FirstOrDefault();
			var firtItemAct = categoryListAct.FirstOrDefault();

			Assert.Equal(firtItemExpect!.Name, firtItemAct!.Name);
			Assert.Equal(firtItemExpect.Id, firtItemAct.Id);
		}
				  
		private async Task SeddingDataAsync(AppDbContext appDbContext)
		{
			var categories = LoadCategorySampleData();
			await appDbContext.Categories.AddRangeAsync(categories);
			await appDbContext.SaveChangesAsync();
		}

		private List<Category> LoadCategorySampleData()
		{
			return new List<Category>()
			{
				new Category
				{
					Id = Guid.Parse("5646576c-199b-456d-a746-1ca64179e60d"),
					Name = "Drink",
					Description = "Drink Type"
				},

				new Category
				{
					Id = Guid.Parse("937798d0-6e17-47a0-a9d8-bb49ffe1f328"),
					Name = "Food",
					Description = "Food Type"
				},

				new Category
				{
					Id = Guid.Parse("210adf63-a836-40b7-ba7d-9c87e9801e69"),
					Name = "Entertainment",
					Description = "Entertainment Type"
				}
			};
		}
	}
}