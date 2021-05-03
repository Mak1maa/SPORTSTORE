using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Models;
using SportStore.WebUI.HtmlHelpers;

namespace SportStore.UnitTests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1"},
                new Products { ProductsID = 2, Name = "Вещь2"},
                new Products { ProductsID = 3, Name = "Вещь3"},
                new Products { ProductsID = 4, Name = "Вещь4"},
                new Products { ProductsID = 5, Name = "Вещь5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Products> products = result.Products.ToList();
            Assert.IsTrue(products.Count == 2);
            Assert.AreEqual(products[0].Name, "Вещь4");
            Assert.AreEqual(products[1].Name, "Вещь5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1"},
                new Products { ProductsID = 2, Name = "Вещь2"},
                new Products { ProductsID = 3, Name = "Вещь3"},
                new Products { ProductsID = 4, Name = "Вещь4"},
                new Products { ProductsID = 5, Name = "Вещь5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act
            ProductsListViewModel result
                = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1", Category="Cat1"},
                new Products { ProductsID = 2, Name = "Вещь2", Category="Cat2"},
                new Products { ProductsID = 3, Name = "Вещь3", Category="Cat1"},
                new Products { ProductsID = 4, Name = "Вещь4", Category="Cat2"},
                new Products { ProductsID = 5, Name = "Вещь5", Category="Cat3"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Products> result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
                .Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Вещь2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Вещь4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products> {
                new Products { ProductsID = 1, Name = "Вещь1", Category="Футболки"},
                new Products { ProductsID = 2, Name = "Вещь2", Category="Шорты"},
                new Products { ProductsID = 3, Name = "Вещь3", Category="Толстовки"},
                new Products { ProductsID = 4, Name = "Вещь4", Category="Обувь"},
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Футболки");
            Assert.AreEqual(results[1], "Шорты");
            Assert.AreEqual(results[2], "Толстовки");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[] {
                new Products { ProductsID = 1, Name = "Вещь1", Category="Футболки"},
                new Products { ProductsID = 2, Name = "Вещь2", Category="Шорты"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Футболки";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            /// Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1", Category="Cat1"},
                new Products { ProductsID = 2, Name = "Вещь2", Category="Cat2"},
                new Products { ProductsID = 3, Name = "Вещь3", Category="Cat1"},
                new Products { ProductsID = 4, Name = "Вещь4", Category="Cat2"},
                new Products { ProductsID = 5, Name = "Вещь5", Category="Cat3"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}