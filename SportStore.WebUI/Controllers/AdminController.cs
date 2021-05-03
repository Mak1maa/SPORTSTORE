using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;

namespace SportStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productsID)
        {
            Products product = repository.Products
                .FirstOrDefault(p => p.ProductsID == productsID);
            return View(product);
        }

        /*public ViewResult Edit()
        {
            Products product = repository.Products
                .FirstOrDefault();
            return View(product);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Products product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }*/

        [HttpPost]
        public ActionResult Edit(Products product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Изменения в товаре \"{0}\" были сохранены", 
                    product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Products());
        }

        [HttpPost]
        public ActionResult Delete(int ProductsID)
        {
            Products deletedProduct = repository.DeleteProduct(ProductsID);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" был удалён",
                    deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}