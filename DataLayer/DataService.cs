using Microsoft.EntityFrameworkCore;
using DataLayer.Model;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System;

namespace DataLayer
{
    public class DataService
    {


        public IList<Category> GetCategories()
        {
            using var db = new NorthwindContext();

            return db.Categories.ToList();
        }

        public Category? GetCategory(int id)
        {
            using var db = new NorthwindContext();

            return db.Categories.Find(id);
        }

        public Category CreateCategory(string name, string desription)
        {
            using var db = new NorthwindContext();

            int id;
            int allIds = db.Categories.Count();
            id = allIds + 1;

            Category newCategory = new Category{ Id = id, Name = name, Description = desription};
     
            db.Categories.Add(newCategory);

            db.SaveChanges();

            return newCategory;        
        }

        public bool DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return false;
            }
            else
            {
                using var db = new NorthwindContext();
                var toDelete = db.Categories.Find(categoryId);
                if (toDelete != null)
                {
                    db.Categories.Remove(toDelete);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateCategory(int id, string name, string description)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                using var db = new NorthwindContext();
                var categoryToUpdate = db.Categories.Find(id);


                if (categoryToUpdate != null)
                {
                    //set new category values
                    categoryToUpdate.Name = name;
                    categoryToUpdate.Description = description;
                    //upate category 
                    db.Categories.Update(categoryToUpdate);
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public Product? GetProduct(int id)
        {
            using var db = new NorthwindContext();
            var product = db.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
                     
            return product;
        }


      

        public IList<ProductModel> GetProductByCategory(int categoryId)
        {
            using var db = new NorthwindContext();

            var productList = db.Products.Include(x => x.Category).Where(x => x.CategoryId == categoryId).ToList();
            List<ProductModel> products = new List<ProductModel>();

            foreach(var product in productList)
            {
                ProductModel model = new ProductModel();
                model.Id = product.Id;
                model.Name = product.Name;
                model.CategoryName = product.Category.Name;
                products.Add(model);
            }

            return products;
        }

        public IList<ProductModel> GetProductByName(string name)
        {
            using var db = new NorthwindContext();
            List<ProductModel> products = new List<ProductModel>();
        
            foreach (var product in db
                .Products
                .Include(x => x.Category)
                .Where(x => x.Name.Contains(name))
                )
            {
                ProductModel productFound = new ProductModel();
                productFound.Id = product.Id;
                productFound.ProductName = product.Name;
                productFound.CategoryName = product.Category.Name;
                products.Add(productFound);
            }

            return products;
        }


        public Order GetOrder(int orderId)
        {
            using var db = new NorthwindContext();
            Order order = new Order();


            foreach(var findOrder in db.Orders
                .Where(x => x.Id == orderId)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Category))
            {
                order = findOrder;
            }


            return order;
        }

        
        public IList<Order> GetOrders()
        {
            using var db = new NorthwindContext();
            IList<Order> orders = new List<Order>();

            foreach(var order in db.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Category))
            {
                Order ordr = new Order();
                orders.Add(ordr);

            }
            return orders;
        }

        public IList<OrderDetails> GetOrderDetailsByOrderId(int ordrDetailId)
        {
            using var db = new NorthwindContext();
            var orders = new List<OrderDetails>();

            foreach (var ordrDetail in db.OrderDetails
                .Where(x => x.OrderId == ordrDetailId)
                .Include(x => x.Product)
                .ThenInclude(x => x.Category))
            {
                orders.Add(ordrDetail);
            }

            return orders;
        }


        public IList<OrderDetails> GetOrderDetailsByProductId(int productId)
        {
            using var db = new NorthwindContext();
            var orders = new List<OrderDetails>();

            foreach (var ordrDetail in db.OrderDetails
                .Where(x => x.ProductId == productId)
                .Include(x => x.Order)
                .Include(x => x.Product)
                .ThenInclude(x => x.Category)
                .OrderBy(x => x.UnitPrice))
            {
                orders.Add(ordrDetail);
            }

            return orders;
        }

    }
}
