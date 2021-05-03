using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1"},
                new Products { ProductsID = 2, Name = "Вещь2"},
                new Products { ProductsID = 3, Name = "Вещь3"},
                new Products { ProductsID = 4, Name = "Вещь4"},
                new Products { ProductsID = 5, Name = "Вещь5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Products> result = ((IEnumerable<Products>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Вещь1", result[0].Name);
            Assert.AreEqual("Вещь2", result[1].Name);
            Assert.AreEqual("Вещь3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1"},
                new Products { ProductsID = 2, Name = "Вещь2"},
                new Products { ProductsID = 3, Name = "Вещь3"},
                new Products { ProductsID = 4, Name = "Вещь4"},
                new Products { ProductsID = 5, Name = "Вещь5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Products products1 = controller.Edit(1).ViewData.Model as Products;
            Products products2 = controller.Edit(2).ViewData.Model as Products;
            Products products3 = controller.Edit(3).ViewData.Model as Products;

            // Assert
            Assert.AreEqual(1, products1.ProductsID);
            Assert.AreEqual(2, products2.ProductsID);
            Assert.AreEqual(3, products3.ProductsID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1"},
                new Products { ProductsID = 2, Name = "Вещь2"},
                new Products { ProductsID = 3, Name = "Вещь3"},
                new Products { ProductsID = 4, Name = "Вещь4"},
                new Products { ProductsID = 5, Name = "Вещь5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Products result = controller.Edit(6).ViewData.Model as Products;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Products product = new Products { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(product);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveProduct(product));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Products product = new Products { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(product);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveProduct(It.IsAny<Products>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // Организация - создание объекта Game
            Products product = new Products { ProductsID = 2, Name = "Вещь2" };

            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products>
            {
                new Products { ProductsID = 1, Name = "Вещь1"},
                new Products { ProductsID = 2, Name = "Вещь2"},
                new Products { ProductsID = 3, Name = "Вещь3"},
                new Products { ProductsID = 4, Name = "Вещь4"},
                new Products { ProductsID = 5, Name = "Вещь5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(product.ProductsID);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeleteProduct(product.ProductsID));
        }
    }
}