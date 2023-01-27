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
	public void Can_Send_Pagination_View_Model()
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
		ProductsListViewModel result = controller.Index(2)?.ViewData.Model as ProductsListViewModel ?? new();
		
        // Assert
		PagingInfo pageInfo = result.PagingInfo;
		Assert.Equal(2, pageInfo.CurrentPage);
		Assert.Equal(3, pageInfo.ItemsPerPage);
		Assert.Equal(5, pageInfo.TotalItems);
		Assert.Equal(2, pageInfo.TotalPages);
	}

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
        ProductsListViewModel result = controller.Index()?.ViewData.Model as ProductsListViewModel ?? new();

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
        ProductsListViewModel result = controller.Index(2)?.ViewData.Model as ProductsListViewModel ?? new();
        //Assert

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P4", prodArray[0].Name);
		Assert.Equal("P5", prodArray[1].Name);
	}

 
}