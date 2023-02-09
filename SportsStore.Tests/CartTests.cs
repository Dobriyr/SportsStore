using NuGet.Frameworks;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
	public class CartTests
	{
		[Fact]
		public void CanAddNewLines() {
			//Arrange
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			Cart target = new();

			//Act
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);

			CartLine[] result = target.Lines.ToArray();

			//Assert
			Assert.Equal(2, result.Length);
			Assert.Equal(p1, result[0].Product);
			Assert.Equal(p2, result[1].Product);

		}

		[Fact]
		public void CanAddQuantityToExistingLines() {
			//Arrange
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			Cart target = new();

			//Act
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p1, 10);

			CartLine[] result = (target.Lines ?? new()).OrderBy(c => c.Product.ProductID).ToArray();

			//Assert
			Assert.Equal(2, result.Length);
			Assert.Equal(11, result[0].Quantity);
			Assert.Equal(1, result[1].Quantity);

		}

		[Fact]
		public void CanRemoveLine() {
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };
			Product p3 = new Product { ProductID = 3, Name = "P3" };

			Cart target = new();

			target.AddItem(p1, 1);
			target.AddItem(p2, 3);
			target.AddItem(p3, 5);
			target.AddItem(p2, 1);

			//Act
			target.RemoveLine(p2);

			//Assert
			Assert.Empty(target.Lines.Where(cl => cl.Product.ProductID == 2));
			Assert.Equal(2, target.Lines.Count());
		}

		[Fact]
		public void CalculteCartTotal() {
			//Arrange
			Product p1 = new Product { ProductID = 1, Name = "P1", Price=100M }; 
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

			Cart target = new();

			//Act
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p1, 3);
			decimal result = target.ComputeTotalValue();

			//Assert
			Assert.Equal(450M, result);
		}

		[Fact]
		public void CanClearContents() {
			//Arrage 
			Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

			Cart target = new();

			target.AddItem(p1, 1);
			target.AddItem(p2, 1);

			//Act
			target.Clear();

			//Assert
			Assert.Empty(target.Lines);

		}
		}
	}
