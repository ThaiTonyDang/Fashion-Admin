
using FashionWeb.Infrastructure.FileHelpers;
using FashionWeb.Infrastructure.Models;
using FashionWeb.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionWeb.Domain.Services;
using FashionWeb.Domain.ViewModels;
using Xunit;

namespace FashionWeb.Tests.ServicesTest
{
	public class CategoryServiceTest
	{
		private readonly Mock<ICategoryRepository> _categoryRepoMock;
		public CategoryServiceTest()
		{
			_categoryRepoMock = new Mock<ICategoryRepository>();
			_categoryRepoMock = new Mock<ICategoryRepository>();
		}

		[Fact]
		public async Task Should_GetListCategories_Return_Success()
		{
			// Arrage
			var categoryExpect = new Category()
			{
				Id = Guid.NewGuid(),
				Name = "Mobile",
			};

			var categoriesExpect = new List<Category>() { categoryExpect };
			_categoryRepoMock.Setup(mbox => mbox.Categories()).ReturnsAsync(categoriesExpect);
			var COUNT_EXPECT = 1;

			// Act
			var categoryService = new CategoryService(_categoryRepoMock.Object);
			var categoriesAct = await categoryService.GetListCategoryAsync();

			// Assert
			Assert.Equal(COUNT_EXPECT, categoriesAct.Count);

			var categoryFirtItemAct = categoriesAct.FirstOrDefault();

			Assert.NotNull(categoryFirtItemAct!.Name);
			Assert.Equal(categoryExpect.Name, categoryFirtItemAct!.Name);
			Assert.Equal(categoryExpect.Id, categoryFirtItemAct!.CategoryId);
		}
	}
}