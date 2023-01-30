using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Controllers;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{

	[Fact]
    public void CanUseRepository()
    {
        //Arrange
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
        mock.Setup(m => m.Products).Returns((new Product[] {
            new() { ProductID = 1, Name="p1"},
            new() { ProductID = 2, Name="p2"},
        }).AsQueryable<Product>());

        HomeController controller = new(mock.Object);

        //Act
        ProductsListViewModel result = controller.Index(null)?.ViewData.Model as ProductsListViewModel ?? new();

        //Assert
        Product[] prodArray = result.Products.ToArray();

        Assert.True(prodArray.Length == 2);
        Assert.Equal("p1", prodArray[0].Name);
        Assert.Equal("p2", prodArray[1].Name);
    }

    [Fact]
    public void CanPaginate() {
		// Arrange
		Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
		mock.Setup(m => m.Products).Returns((
            new Product[] {
                 new Product {ProductID = 1, Name = "P1"},
                 new Product {ProductID = 2, Name = "P2"},
                 new Product {ProductID = 3, Name = "P3"},
                 new Product {ProductID = 4, Name = "P4"},
                 new Product {ProductID = 5, Name = "P5"}
            })
        .AsQueryable<Product>());

		HomeController controller = new HomeController(mock.Object);
		controller.PageSize = 3;

        //Act 
        ProductsListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P4", prodArray[0].Name);
		Assert.Equal("P5", prodArray[1].Name);
	}

	[Fact]
	public void CanSendPaginationViewModel()
	{
		// Arrange
		Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

		mock.Setup(m => m.Products).Returns((new Product[] {
			 new Product {ProductID = 1, Name = "P1"},
			 new Product {ProductID = 2, Name = "P2"},
			 new Product {ProductID = 3, Name = "P3"},
			 new Product {ProductID = 4, Name = "P4"},
			 new Product {ProductID = 5, Name = "P5"}
			 })
		.AsQueryable<Product>());

		HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

		// Act
		ProductsListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();

		// Assert
		PagingInfo pageInfo = result.PagingInfo;
		Assert.Equal(2, pageInfo.CurrentPage);
		Assert.Equal(3, pageInfo.ItemsPerPage);
		Assert.Equal(5, pageInfo.TotalItems);
		Assert.Equal(2, pageInfo.TotalPages);
	}

    [Fact]
    public void CanFilterProducts() { 
        //Arange
        Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

        mock.Setup(m=>m.Products).Returns(
                (new Product[] {
                     new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                     new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                     new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                     new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                     new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
        }).AsQueryable<Product>());

        HomeController controller = new HomeController(mock.Object);
        controller.PageSize = 3;

        //Action
        Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model as ProductsListViewModel ?? new()).Products.ToArray();

        //Assert
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
		Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");

	}

    [Fact]
    public void GenerateCategorySpecificProductCount() {
        //Arrange
        Mock<IStoreRepository> mock = new();
        mock.Setup(m => m.Products).Returns((new Product[] {
             new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
             new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
             new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
             new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
             new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
        }).AsQueryable<Product>());

        HomeController target = new HomeController(mock.Object);
        target.PageSize = 3;

        Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

		//Action
		int? res1 = GetModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
		int? res2 = GetModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
		int? res3 = GetModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
		int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItems;

		//Assert
		Assert.Equal(2, res1);
		Assert.Equal(2, res2);
		Assert.Equal(1, res3);
		Assert.Equal(5, resAll);
	}

}