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


       
       
    }
}
