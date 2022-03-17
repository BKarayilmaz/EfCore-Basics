using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EfCore
{
    //Convention
    //Data Annotations
    //Fuent Api
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //It's for see SQL Logs
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("Data Source=ShopDb.dp");

            optionsBuilder.
                UseLoggerFactory(MyLoggerFactory)
               // UseSqlite(@"Data Source=C:\Users\Bilgisayar\source\repos\EfCore\EfCore\ShopDb.dp");
               //.UseSqlServer(@"Data Source=DESKTOP-QVNJVT9\SQLEXPRESS;Initial Catalog=ShopDb;Integrated Security=SSPI;");
               .UseMySql(@"server=localhost;port=3306;database=ShopDat;user=root;password=mysql1234");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasKey(t => new { t.ProductId, t.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p=>p.ProductCategories)
                .HasForeignKey(pc=>pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);
        }
    }

    public class User
    {
        public User()
        {
            this.Addresses = new List<Address>();
        }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Customer Customer { get; set; }
        public List<Address> Addresses { get; set; }//Navigation property
    }

    public class Customer
    {

        public int CustomerID { get; set; }
        public string IdentityNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }

    }
    public class Supplier
    {

        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string TaxNumber { get; set; }

    }

    public class Address
    {
        public int AddressID { get; set; }
        public string Fullname { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public User User { get; set; }//Navigation property
        public int UserId { get; set; }
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
        public int CategoryID { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required] // Can Not Be Null
        public string CategoryName { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // InsertUsers();
            // InsertAddresses();
            using (ShopContext shopContext = new ShopContext())
            {
                //var products = new List<Product>(){
                //    new Product { ProductName = "Samsung S10", ProductPrice = 25000 },
                //    new Product { ProductName = "Iphone 13Pro", ProductPrice = 35000 },
                //    new Product { ProductName = "Apple Watch", ProductPrice = 8000 },
                //    new Product { ProductName = "Mi Band", ProductPrice = 500 },
                //};
                //shopContext.Products.AddRange(products);

                //var categories = new List<Category>(){
                //    new Category { CategoryName="Smart Watch" },
                //    new Category { CategoryName="Phone" },
                //    new Category { CategoryName="Technology" },
                //};
                //shopContext.Categories.AddRange(categories);


                var categories = new int[2] { 1, 2 };
                var p = shopContext.Products.Find(1);
                p.ProductCategories = categories.Select(cid => new ProductCategory()
                {
                    CategoryId=cid,
                    ProductId=p.ProductID
                }).ToList();

                    shopContext.SaveChanges();

                //User user = new Usercategories
                //{
                //    Username = "defnesevil",
                //    Email = "test@defnesevil.com",
                //    Customer = new Customer()
                //    {
                //        Firstname = "Defne",
                //        Lastname = "Sevil",
                //        IdentityNumber = "423562425"
                //    }
                //};
                //shopContext.Users.Add(user);
                //shopContext.SaveChanges();
            }
        }
        static void InsertUsers()
        {
            var users = new List<User>()
            {
                new User(){Username="yildizemre",Email="test@yildizemre.com"},
                new User(){Username="sedatgeckin",Email="test@sedatgeckin.com"},
                new User(){Username="sukrubilge",Email="test@sukrubilge.com"},
                new User(){Username="sahikaderin",Email="test@sahikaderin.com"},
            };
            using (ShopContext shopContext = new ShopContext())
            {
                shopContext.Users.AddRange(users);
                shopContext.SaveChanges();
                Console.WriteLine("All user has been created.");
            }
        }
        static void InsertAddresses()
        {
            var addresses = new List<Address>()
            {
                new Address(){Fullname="Yıldız Emre",Title="Ev Adresi",Body="İzmir",UserId=1},
                new Address(){Fullname="Yıldız Emre",Title="İş Adresi",Body="İzmir",UserId=1},
                new Address(){Fullname="Sedat Geçkin",Title="Ev Adresi",Body="İzmir",UserId=2},
                new Address(){Fullname="Sedat Geçkin",Title="İş Adresi",Body="İzmir",UserId=2},
                new Address(){Fullname="Şükrü Bilgi",Title="Ev Adresi",Body="İzmir",UserId=3},
                new Address(){Fullname="Şahika Derin",Title="İş Adresi",Body="İzmir",UserId=4},

            };
            using (ShopContext shopContext = new ShopContext())
            {
                shopContext.Addresses.AddRange(addresses);
                shopContext.SaveChanges();
                Console.WriteLine("All user has been created.");
            }
        }
        //static void DeleteProduct(int id)
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        //var p = shopContext.Products.FirstOrDefault(i=>i.ProductID==id);
        //        //if (p != null)
        //        //{
        //        //    shopContext.Products.Remove(p);
        //        //    shopContext.SaveChanges();

        //        //    Console.WriteLine("Data deleted");
        //        //}

        //        //Delete without select
        //        var p = new Product() { ProductID = 7 };
        //        //shopContext.Products.Remove(p); We can use this row or EntityState row 
        //        shopContext.Entry(p).State = EntityState.Deleted;
        //        shopContext.SaveChanges();


        //    }

        //}
        //static void UpdateProduct()
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        var p = shopContext.Products.Where(i => i.ProductID == 1).FirstOrDefault();
        //        if (p != null)
        //        {
        //            p.ProductPrice = 8700;
        //            shopContext.Products.Update(p);
        //            shopContext.SaveChanges();
        //        }
        //    }



        //    //    using (ShopContext shopContext = new ShopContext())
        //    //{
        //    //    //Change tracking
        //    //    var p = shopContext.Products
        //    //        .Where(i => i.ProductID == 1)
        //    //        .FirstOrDefault();
        //    //    if (p!=null)
        //    //    {
        //    //        p.ProductPrice *= 1.2m;
        //    //        shopContext.SaveChanges();
        //    //        Console.WriteLine("Data Updated");
        //    //    }

        //    //    //İf we don't want to use change tracking
        //    //    ////var pr = shopContext.Products
        //    //    ////    .Where(i => i.ProductID == 1)
        //    //    ////    .AsNoTracking()
        //    //    ////    .FirstOrDefault();
        //    //    ////if (pr != null)
        //    //    ////{
        //    //    ////    pr.ProductPrice *= 1.2m;
        //    //    ////    shopContext.SaveChanges();
        //    //    ////    Console.WriteLine("Data Updated");
        //    //    ////}
        //    //}


        //    //Update without select
        //    ////using (ShopContext shopContext = new ShopContext())
        //    ////{
        //    ////    var entity = new Product() { ProductID = 1 };
        //    ////    shopContext.Products.Attach(entity);
        //    ////    entity.ProductPrice = 3300;
        //    ////    shopContext.SaveChanges();
        //    ////}
        //}
        //static void GetProductByName(string name)
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        var products = shopContext.Products.
        //            Where(p => p.ProductName
        //            .ToLower()
        //            .Contains(name.ToLower())).ToList();

        //        foreach (var item in products)
        //        {
        //            Console.WriteLine($"Name: {item.ProductName} || Price: {item.ProductPrice}");
        //        }
        //    }
        //}
        //static void GetProductById(int id)
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        var result = shopContext.Products.Where(p => p.ProductID == id).FirstOrDefault();

        //        Console.WriteLine($"Name: {result.ProductName} || Price: {result.ProductPrice}");
        //    }
        //}
        //static void GetAllProducts()
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        //var products = shopContext.Products.ToList();
        //        var products = shopContext.Products
        //            .Select(p => new
        //            {
        //                p.ProductName,
        //                p.ProductPrice
        //            }).ToList();
        //        foreach (var item in products)
        //        {
        //            Console.WriteLine($"Name: {item.ProductName} || Price: {item.ProductPrice}");
        //        }

        //    }
        //}

        //static void AddProducts()
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        //One data add
        //        Product product = new Product { ProductName = "Samsung S5", ProductPrice = 1000 };
        //        shopContext.Products.Add(product);
        //        shopContext.SaveChanges();
        //        Console.WriteLine("Data has been added.");
        //        //Multiple data add
        //        var products = new List<Product>
        //        {
        //            new Product { ProductName = "Samsung S10", ProductPrice = 25000 },
        //            new Product { ProductName = "Iphone 13Pro", ProductPrice = 35000 },
        //            new Product { ProductName = "Apple Watch", ProductPrice = 8000 },
        //            new Product { ProductName = "Mi Band", ProductPrice = 500 },
        //        };
        //        //We can use AddRange method for add collection
        //        shopContext.Products.AddRange(products);
        //        shopContext.SaveChanges();
        //        Console.WriteLine("All data has been added.");
        //    }
        //}

        //static void AddProduct()
        //{
        //    using (ShopContext shopContext = new ShopContext())
        //    {
        //        Product product = new Product { ProductName = "Samsung 43AU9000", ProductPrice = 8699 };
        //        shopContext.Products.Add(product);
        //        shopContext.SaveChanges();
        //        Console.WriteLine("Data has been added.");

        //    }
        //}
    }
}
