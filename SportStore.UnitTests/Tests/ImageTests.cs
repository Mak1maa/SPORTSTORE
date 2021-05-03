using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SportStore.UnitTests.Tests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Products product = new Products
            {
                ProductsID = 2,
                Name = "Вещь2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products> {
                new Products {ProductsID = 1, Name = "Вещь1"},
                product,
                new Products {ProductsID = 3, Name = "Вещь3"}
            }.AsQueryable());

            // Организация - создание контроллера
            ProductController controller = new ProductController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(product.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Products> {
                new Products {ProductsID = 1, Name = "Вещь1"},
                new Products {ProductsID = 2, Name = "Вещь2"}
            }.AsQueryable());

            // Организация - создание контроллера
            ProductController controller = new ProductController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}