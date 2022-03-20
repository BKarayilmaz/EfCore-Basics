using EfCore.Data.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EfCore
{

    class Program
    {
        public class CustomerDemo
        {
            public CustomerDemo()
            {
                Orders = new List<OrderDemo>();
            }
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public int OrderCount { get; set; }
            public List<OrderDemo> Orders { get; set; }
        }
        public class OrderDemo
        {
            public int OrderId { get; set; }
            public decimal Total { get; set; }
            public List<ProductDemo> Products { get; set; }
        }
        public class ProductDemo
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
        }
        static void Main(string[] args)
        {
            using (NorthwindContext northwindContext = new NorthwindContext())
            {
                //Bring All Customer
                var allCustomers = northwindContext.Customers.ToList();
                foreach (var item in allCustomers)
                {
                    Console.WriteLine(item.FirstName + " " + item.LastName);
                }
                Console.WriteLine("-----------------------------------------------");

                //Bring All Customer But Just First Name and Last Name Field
                var firstAndLastNameCustomers = northwindContext.Customers.Select(c => new
                {
                    c.FirstName,
                    c.LastName
                });
                foreach (var item in firstAndLastNameCustomers)
                {
                    Console.WriteLine(item.FirstName + " " + item.LastName);
                }
                Console.WriteLine("-----------------------------------------------");

                //Bring Customers/Living New York/Alphabetical
                var customersFromNewYork = northwindContext.Customers
                    .Where(c => c.City == "New York")
                    .ToList();
                foreach (var item in customersFromNewYork)
                {
                    Console.WriteLine(item.FirstName + " " + item.LastName + " City: " + item.City);
                }
                Console.WriteLine("-----------------------------------------------");


                //Product Name/Category="Beverages"
                var BeveragesProduct = northwindContext.Products
                    .Where(p => p.Category == "Beverages")
                    .ToList();
                foreach (var item in BeveragesProduct)
                {
                    Console.WriteLine(item.ProductName + "||| Price: " + item.ListPrice);
                }
                Console.WriteLine("-----------------------------------------------");

                //Recently Added 5 PRODUCTS
                var recentlyAddedFiveProduct = northwindContext.Products.OrderByDescending(i => i.Id).Take(5);
                foreach (var item in recentlyAddedFiveProduct)
                {
                    Console.WriteLine(item.ProductName);
                }
                Console.WriteLine("-----------------------------------------------");

                //Name And Price Information Of Products Whose Price Is Between 10 And 30
                var priceBetween1030Product = northwindContext.Products
                    .Where(i => i.ListPrice >= 10 && i.ListPrice <= 30)
                    .Select(p => new
                    {
                        p.ProductName,
                        p.ListPrice
                    }).ToList();
                foreach (var item in priceBetween1030Product)
                {
                    Console.WriteLine(item.ProductName + " - " + item.ListPrice);
                }
                Console.WriteLine("-----------------------------------------------");

                //Average Prices of Products in the Beverages Category
                var averagePricesofProductsBeveragesCategory = northwindContext.Products
                    .Where(i => i.Category == "Beverages")
                    .Average(p => p.ListPrice);
                Console.WriteLine("Average Price in Beverages: " + averagePricesofProductsBeveragesCategory);
                Console.WriteLine("-----------------------------------------------");

                //Total Prices of Products in Beverages and Condiments Categories
                var totalPricesOfProductsInBeveragesAndCondimentsCategories = northwindContext.Products
                    .Where(i => i.Category == "Beverages" || i.Category == "Condiments")
                    .Sum(p => p.ListPrice);
                Console.WriteLine("Total Prices of Products in Beverages and Condiments Categories: " + totalPricesOfProductsInBeveragesAndCondimentsCategories);
                Console.WriteLine("-----------------------------------------------");

                //Products with the Word "Tea"
                var productsWithTheWordTea = northwindContext.Products
                    .Where(i => i.ProductName.ToLower().Contains("Tea".ToLower()) || i.Description.ToLower().Contains("Tea".ToLower()))
                    .ToList();
                foreach (var item in productsWithTheWordTea)
                {
                    Console.WriteLine(item.ProductName);
                }
                Console.WriteLine("-----------------------------------------------");

                //Most Expensive And Cheapest Product
                //For Price We Can Use Min and Max
                var maxPrice = northwindContext.Products.Max(i => i.ListPrice);
                var minPrice = northwindContext.Products.Min(i => i.ListPrice);
                Console.WriteLine("Minumum Price: " + minPrice + " Maximum Price: " + maxPrice);
                //We Need To Find Min/Max Price's Product In DB
                var productMinPrice = northwindContext.Products
                    .Where(i => i.ListPrice == (northwindContext.Products.Min(a => a.ListPrice)))
                    .FirstOrDefault();
                Console.WriteLine($"Name: {productMinPrice.ProductName} Price: {productMinPrice.ListPrice}");
                Console.WriteLine("-----------------------------------------------");
            }


            using (NorthwindContext northwindContext = new NorthwindContext())
            {
                var customers = northwindContext.Customers
                    .Where(i => i.Orders.Count > 0)//OR  .Where(i => i.Orders.Any())
                    .Select(i => new CustomerDemo
                    {
                        Name = i.FirstName,
                        CustomerId = i.Id,
                        OrderCount = i.Orders.Count
                    })
                    .OrderBy(i => i.OrderCount)
                    .ToList();
                foreach (var item in customers)
                {
                    Console.WriteLine($"Id: {item.CustomerId} Name: {item.Name} Count: {item.OrderCount}");
                }
                Console.WriteLine("-----------------------------------------------");

                var nCustomers = northwindContext.Customers
                   .Where(i => i.Orders.Count > 0)//OR  .Where(i => i.Orders.Any())
                   .Select(i => new CustomerDemo
                   {
                       Name = i.FirstName,
                       CustomerId = i.Id,
                       OrderCount = i.Orders.Count,
                       Orders = i.Orders.Select(a => new OrderDemo
                       {
                           OrderId=a.Id,
                           Total=(decimal)a.OrderDetails.Sum(od=>od.Quantity*od.UnitPrice)
                       }).ToList()
                   })
                   .OrderBy(i => i.OrderCount)
                   .ToList();
                foreach (var customer in nCustomers)
                {
                    Console.WriteLine("       -----        ");
                    Console.WriteLine($"Id: {customer.CustomerId} Name: {customer.Name} Count: {customer.OrderCount}");
                    foreach (var order in customer.Orders)
                    {
                        Console.WriteLine($"Order Id: {order.OrderId} Total: {order.Total}");
                    }
                }
                Console.WriteLine("-----------------------------------------------");

                var idCustomers = northwindContext.Customers
                  .Where(i => i.Id==8)
                  .Select(i => new CustomerDemo
                  {
                      Name = i.FirstName,
                      CustomerId = i.Id,
                      OrderCount = i.Orders.Count,
                      Orders = i.Orders.Select(a => new OrderDemo
                      {
                          OrderId = a.Id,
                          Total = (decimal)a.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                      }).ToList()
                  })
                  .OrderBy(i => i.OrderCount)
                  .ToList();
                foreach (var customer in idCustomers)
                {
                    Console.WriteLine($"Id: {customer.CustomerId} Name: {customer.Name} Count: {customer.OrderCount}");
                    foreach (var order in customer.Orders)
                    {
                        Console.WriteLine($"Order Id: {order.OrderId} Total: {order.Total}");
                    }
                }
                Console.WriteLine("-----------------------------------------------");

                var pCustomers = northwindContext.Customers
                  .Where(i => i.Id == 8)
                  .Select(i => new CustomerDemo
                  {
                      Name = i.FirstName,
                      CustomerId = i.Id,
                      OrderCount = i.Orders.Count,
                      Orders = i.Orders.Select(a => new OrderDemo
                      {
                          OrderId = a.Id,
                          Total = (decimal)a.OrderDetails.Sum(od => od.Quantity * od.UnitPrice),
                          Products=a.OrderDetails.Select(p=>new ProductDemo { 
                              ProductId=(int)p.ProductId,
                              Name=p.Product.ProductName
                          }).ToList()
                      }).ToList()
                  })
                  .OrderBy(i => i.OrderCount)
                  .ToList();
                foreach (var customer in pCustomers)
                {
                    Console.WriteLine($"Id: {customer.CustomerId} Name: {customer.Name} Count: {customer.OrderCount}");
                    foreach (var order in customer.Orders)
                    {
                        Console.WriteLine($"-Order Id: {order.OrderId} Total: {order.Total}");
                        foreach (var product in order.Products)
                        {
                            Console.WriteLine($"--Product Id: {product.ProductId} Name: {product.Name}");
                        }
                    }
                }
                Console.WriteLine("-----------------------------------------------");
            }

        }

    }
}
