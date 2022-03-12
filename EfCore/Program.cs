using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace EfCore
{
    public class ShopContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ShopDb.dp");
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
            Console.WriteLine("Hello World!");
        }
    }
}
