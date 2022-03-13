using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EfCore
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        //It's for see SQL Logs
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("Data Source=ShopDb.dp");

            optionsBuilder.
                UseLoggerFactory(MyLoggerFactory).
                UseSqlite(@"Data Source=C:\Users\Bilgisayar\source\repos\EfCore\EfCore\ShopDb.dp");
        }
    }
    public class Product
    {
        //Primary Key(ID, <type_name>Id)
        [Key]
        public int ProductID { get; set; }
        [MaxLength(100)] //Max 100 Character
        [Required] // Can Not Be Null
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }//Decimal value type can not be null
    }
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required] // Can Not Be Null
        public string CategoryName { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            DeleteProduct(6);
        }
        static void DeleteProduct(int id)
        {
            using (ShopContext shopContext = new ShopContext())
            {
                //var p = shopContext.Products.FirstOrDefault(i=>i.ProductID==id);
                //if (p != null)
                //{
                //    shopContext.Products.Remove(p);
                //    shopContext.SaveChanges();

                //    Console.WriteLine("Data deleted");
                //}

                //Delete without select
                var p = new Product() { ProductID = 7 };
                //shopContext.Products.Remove(p); We can use this row or EntityState row 
                shopContext.Entry(p).State = EntityState.Deleted;
                shopContext.SaveChanges();


            }

        }
        static void UpdateProduct()
        {
            using (ShopContext shopContext = new ShopContext())
            {
                var p = shopContext.Products.Where(i => i.ProductID == 1).FirstOrDefault();
                if (p != null)
                {
                    p.ProductPrice = 8700;
                    shopContext.Products.Update(p);
                    shopContext.SaveChanges();
                }
            }



            //    using (ShopContext shopContext = new ShopContext())
            //{
            //    //Change tracking
            //    var p = shopContext.Products
            //        .Where(i => i.ProductID == 1)
            //        .FirstOrDefault();
            //    if (p!=null)
            //    {
            //        p.ProductPrice *= 1.2m;
            //        shopContext.SaveChanges();
            //        Console.WriteLine("Data Updated");
            //    }

            //    //İf we don't want to use change tracking
            //    ////var pr = shopContext.Products
            //    ////    .Where(i => i.ProductID == 1)
            //    ////    .AsNoTracking()
            //    ////    .FirstOrDefault();
            //    ////if (pr != null)
            //    ////{
            //    ////    pr.ProductPrice *= 1.2m;
            //    ////    shopContext.SaveChanges();
            //    ////    Console.WriteLine("Data Updated");
            //    ////}
            //}


            //Update without select
            ////using (ShopContext shopContext = new ShopContext())
            ////{
            ////    var entity = new Product() { ProductID = 1 };
            ////    shopContext.Products.Attach(entity);
            ////    entity.ProductPrice = 3300;
            ////    shopContext.SaveChanges();
            ////}
        }
        static void GetProductByName(string name)
        {
            using (ShopContext shopContext = new ShopContext())
            {
                var products = shopContext.Products.
                    Where(p => p.ProductName
                    .ToLower()
                    .Contains(name.ToLower())).ToList();

                foreach (var item in products)
                {
                    Console.WriteLine($"Name: {item.ProductName} || Price: {item.ProductPrice}");
                }
            }
        }
        static void GetProductById(int id)
        {
            using (ShopContext shopContext = new ShopContext())
            {
                var result = shopContext.Products.Where(p => p.ProductID == id).FirstOrDefault();

                Console.WriteLine($"Name: {result.ProductName} || Price: {result.ProductPrice}");
            }
        }
        static void GetAllProducts()
        {
            using (ShopContext shopContext = new ShopContext())
            {
                //var products = shopContext.Products.ToList();
                var products = shopContext.Products
                    .Select(p => new
                    {
                        p.ProductName,
                        p.ProductPrice
                    }).ToList();
                foreach (var item in products)
                {
                    Console.WriteLine($"Name: {item.ProductName} || Price: {item.ProductPrice}");
                }

            }
        }

        static void AddProducts()
        {
            using (ShopContext shopContext = new ShopContext())
            {
                //One data add
                Product product = new Product { ProductName = "Samsung S5", ProductPrice = 1000 };
                shopContext.Products.Add(product);
                shopContext.SaveChanges();
                Console.WriteLine("Data has been added.");
                //Multiple data add
                var products = new List<Product>
                {
                    new Product { ProductName = "Samsung S10", ProductPrice = 25000 },
                    new Product { ProductName = "Iphone 13Pro", ProductPrice = 35000 },
                    new Product { ProductName = "Apple Watch", ProductPrice = 8000 },
                    new Product { ProductName = "Mi Band", ProductPrice = 500 },
                };
                //We can use AddRange method for add collection
                shopContext.Products.AddRange(products);
                shopContext.SaveChanges();
                Console.WriteLine("All data has been added.");
            }
        }

        static void AddProduct()
        {
            using (ShopContext shopContext = new ShopContext())
            {
                Product product = new Product { ProductName = "Samsung 43AU9000", ProductPrice = 8699 };
                shopContext.Products.Add(product);
                shopContext.SaveChanges();
                Console.WriteLine("Data has been added.");

            }
        }
    }
}
