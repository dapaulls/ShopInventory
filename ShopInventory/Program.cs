using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShopInventory
{
    class Program
    {
        static void Main(string[] args)
        {
            // CREATE A NEW PRODUCT LIST
            List<Product> prodInventory = new List<Product>(); 
            // CREATE A NEW SALES LIST
            List<Sale> salesList = new List<Sale>();
            
            // DISPLAY MAIN MENU
            do
            {
                Console.Clear();
                Console.WriteLine("TOP BRAND INTERNATIONAL\n");
                Console.WriteLine(" 1. Display Product Inventory");
                Console.WriteLine(" 2. Add Products");
                Console.WriteLine(" 3. Calculate a Selling Price");
                Console.WriteLine(" 4. Generate a Receipt");
                Console.WriteLine(" 5. Generate a Sales Report - Total Sales");
                Console.WriteLine(" 6. Generate a Sales Report - Slow Selling Products");
                Console.WriteLine(" 7. Generate a Sales Report - Fast Selling Products");
                Console.WriteLine(" 8. Generate a Sales Report - Percentage of Fast Selling Products");
                Console.WriteLine(" 9. Check Stock of a Product");
                Console.WriteLine("10. Check Stock of Slow Selling Products");
                Console.WriteLine("11. Check Stock of Fast Selling Products");
                Console.Write("\nPlease enter a choice between 1 and 11: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        displayInventory(prodInventory);
                        break;
                    case "2":
                        Product newProduct = addProduct();
                        prodInventory.Add(newProduct);
                        break;
                    case "3":
                        calculateSellingPrice(prodInventory);
                        break;
                    case "4":
                        generateReceipt(prodInventory, salesList);
                        break;
                    case "5":
                        displaySalesReport(salesList);
                        break;
                    case "6":
                        displaySalesReport(salesList, 'N', "slow");
                        break;
                    case"7":
                        displaySalesReport(salesList, 'Y', "fast");
                        break;
                    case "8":
                        percentageSales(salesList);
                        break;
                    case "9":
                        displayQuantity(prodInventory);
                        break;
                    case "10":
                        displayQuantity(prodInventory, 'N', "slow");
                        break;
                    case "11":
                        displayQuantity(prodInventory, 'Y', "fast");
                        break;
                    default:
                        Console.WriteLine("Enter a valid choice");
                        break;
                }
                Console.Write("\nWould you like to perform another action? (Y/N)");
            }
            while (Console.ReadLine().ToLower() == "y");            
        }

        private static void displayInventory(List<Product> inventory)
        {
            Console.Clear();
            Console.WriteLine("\nName\tBrand\tCost Price\tFast Selling\tQuantity\tDescription");
            Console.WriteLine("----\t-----\t----------\t------------\t--------\t-----------");

            foreach (Product p in inventory)
            {
                Console.WriteLine("{0}\t{1}\t{2:C}\t\t{3}\t\t{4}\t\t{5}", p.ProductName, p.ProductBrand, p.ProductCostPrice, p.ProductFastSelling, p.ProductQuantity, p.ProductDescription);
            }
        }

        private static Product addProduct()
        {
            Product newProduct = new Product();
            Console.Clear();
            Console.WriteLine("Input a New Product\n");
            Console.Write("Enter Product Name: ");
            newProduct.ProductName = Console.ReadLine();
            Console.Write("Enter Product Brand: ");
            newProduct.ProductBrand = Console.ReadLine();
            Console.Write("Enter Product Description: ");
            newProduct.ProductDescription = Console.ReadLine();
            Console.Write("Enter Cost Price: £");
            newProduct.ProductCostPrice = decimal.Parse(Console.ReadLine());
            Console.Write("Is the Product fast selling? ");
            newProduct.ProductFastSelling = char.Parse(Console.ReadLine().ToUpper());
            Console.Write("Enter Quantity: ");
            newProduct.ProductQuantity = int.Parse(Console.ReadLine());
            return newProduct;
        }

        private static void calculateSellingPrice(List<Product> inventory)
        {
            Console.Clear();
            Console.WriteLine("Calculate a Selling Price\n");
            string inputProd = inputProductName();
            var calcSP = from p in inventory
                         where p.ProductName == inputProd
                         select p;
            foreach (var p in calcSP)
            {
                decimal sellingPriceFactor = (p.ProductFastSelling == 'Y') ? p.FastSellingFactor : p.SlowSellingFactor;
                decimal productSP = p.ProductCostPrice * sellingPriceFactor;
                Console.WriteLine("\nName\tFast Selling\tCost Price\tSelling Price");
                Console.WriteLine("----\t------------\t----------\t-------------");
                Console.WriteLine("{0}\t{1}\t\t{2:C}\t\t{3:C}", p.ProductName, p.ProductFastSelling, p.ProductCostPrice, productSP);
            }
        }

        private static void generateReceipt(List<Product> inventory, List<Sale> salesList)
        {
            string inputProd = inputProductName();
            var soldProduct = from p in inventory
                              where p.ProductName == inputProd
                              select p;
            foreach (var p in soldProduct)
            {
                decimal sellingPriceFactor = (p.ProductFastSelling == 'Y') ? p.FastSellingFactor : p.SlowSellingFactor;
                decimal productSP = p.ProductCostPrice * sellingPriceFactor;
                Console.Clear();
                Console.WriteLine("\tTOP BRAND INTERNATIONAL");
                Console.WriteLine("\t=======================");
                Console.WriteLine("\t{0}", DateTime.Now);
                Console.WriteLine("\n\n{0}\t\t\t\t{1:C}", p.ProductName, productSP);
                Console.WriteLine("\nTotal:\t\t\t\t{0:C}", productSP);
                Console.WriteLine("\n\tThank you for shopping with us.");

                p.ProductQuantity--;
                Sale addSale = new Sale() 
                { 
                    SaleName = p.ProductName, 
                    SaleFastSelling = p.ProductFastSelling, 
                    SaleDate = DateTime.Now 
                };
                salesList.Add(addSale);                
            }                 
        }

        private static void displaySalesReport(List<Sale> salesList)
        {
            Console.Clear();
            foreach (Sale s in salesList)
            {
                Console.WriteLine("Name: {0}\t\t Sold On: {1}", s.SaleName, s.SaleDate);
            }
            Console.WriteLine("\nTotal Number of Products Sold: {0}", salesList.Count());
        }

        private static void displaySalesReport(List<Sale> salesList, char fastSelling, string option)
        {
            var productSales = from s in salesList
                               where s.SaleFastSelling == fastSelling
                               select s;
            foreach (var s in productSales)
            {
                Console.WriteLine("Name: {0}\t\t Sold On: {1}", s.SaleName, s.SaleDate);
            }
            Console.WriteLine("\nTotal Number of {0} Selling Products Sold: {1}", option, productSales.Count());
        }        

        private static void percentageSales(List<Sale> salesList)
        {
            var fastSS = from fast in salesList // fastSS - Fast Selling Sales
                         where fast.SaleFastSelling == 'Y'
                         select fast;
            int totalFastSS = fastSS.Count();
            var slowSS = from slow in salesList // slowSS - Slow Selling Sales
                         where slow.SaleFastSelling == 'N'
                         select slow;
            int totalSlowSS = slowSS.Count();
            decimal percentFastSelling = (decimal)totalFastSS / (totalFastSS + totalSlowSS);
            Console.WriteLine("Total Number of Fast Selling Products Sold: {0}", totalFastSS);
            Console.WriteLine("Total Number of Slow Selling Products Sold: {0}", totalSlowSS);
            Console.WriteLine("Number of Fast Selling Products Sold: {0:P}", percentFastSelling);
        }

        private static void displayQuantity(List<Product> inventory)
        {
            string inputProd = inputProductName();
            Console.Clear();
            var productStock = from p in inventory
                               where p.ProductName == inputProd
                               select p;
            foreach (var p in productStock)
            {
                Console.WriteLine("Name: {0}\tQuantity: {1}", p.ProductName, p.ProductQuantity);
            }
        }

        private static void displayQuantity(List<Product> inventory, char fastSelling, string option)
        {
            Console.Clear();
            var productStock = from p in inventory
                               where p.ProductFastSelling == fastSelling
                               select p;
            foreach (var p in productStock)
            {
                Console.WriteLine("Name: {0}\tQuantity: {1}", p.ProductName, p.ProductQuantity);
            }
            var productTotal = productStock.Sum(p => p.ProductQuantity);
            Console.WriteLine("\nTotal Number of {0} Selling Products: {1}", option, productTotal);
        }        

        private static string inputProductName()
        {
            Console.WriteLine("Enter the product name: ");
            string inputProd = Console.ReadLine();
            return inputProd;
        }        
    }
}
