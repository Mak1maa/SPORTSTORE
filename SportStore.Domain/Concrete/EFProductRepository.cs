using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportStore.Domain.Entities;
using SportStore.Domain.Abstract;
using System.Data.SqlClient;

namespace SportStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Products> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Products product)
        {
            if (product.ProductsID == 0)
                context.Products.Add(product);
            else
            {
                Products dbEntry = context.Products.Find(product.ProductsID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Quantity = product.Quantity;
                    dbEntry.Category = product.Category; 
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Products DeleteProduct(int ProductsID)
        {
            Products dbEntry = context.Products.Find(ProductsID);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}