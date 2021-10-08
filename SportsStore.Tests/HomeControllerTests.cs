using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId=1, Name="P1"},
                new Product {ProductId=2, Name="P2"}
            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            IEnumerable<Product> result = (controller.Index() as ViewResult).ViewData.Model as IEnumerable<Product>;

            Product[] productsArray = result.ToArray();
            Assert.True(productsArray.Length == 2);
            Assert.Equal("P1", productsArray[0].Name);
            Assert.Equal("P2", productsArray[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductId=1, Name="P1"},
                new Product { ProductId=2, Name="P2"},
                new Product { ProductId=3, Name="P3"},
                new Product { ProductId=4, Name="P4"},
                new Product { ProductId=5, Name="P5"}
            }).AsQueryable<Product>());
            HomeController controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            IEnumerable<Product> result = (controller.Index(2) as ViewResult).ViewData.Model as IEnumerable<Product>;

            Product[] prodArray = result.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
    }
}
