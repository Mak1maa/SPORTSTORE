using System.Collections.Generic;
using System.Linq;

namespace SportStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Products products, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Products.ProductsID == products.ProductsID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Products = products,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Products products)
        {
            lineCollection.RemoveAll(l => l.Products.ProductsID == products.ProductsID);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Products.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Products Products { get; set; }
        public int Quantity { get; set; }
    }
}