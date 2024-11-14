namespace Query_Expressions_Gruppövning
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastRestocked { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Category: {Category}, Quantity: {Quantity}, Price: {Price:C}, Last Restocked: {LastRestocked:d}";
        }
    }

    class Program
    {
        static List<Product> inventory;

        static void Main(string[] args)
        {
            InventoryFileGenerator inventoryFileGenerator = new InventoryFileGenerator();
            inventoryFileGenerator.GenerateInventoryFile("inventory.txt", 5000);
            var prod = LoadInventoryData();



            Console.ReadLine();
            
        }

        static List<Product> LoadInventoryData()
        {
            string[] lines = File.ReadAllLines("inventory.txt");
            inventory = lines.Skip(1) // Hoppa över rubrikraden
                            .Select(line =>
                            {
                                var parts = line.Split(',');
                                return new Product
                                {
                                    Id = int.Parse(parts[0]),
                                    Name = parts[1],
                                    Category = parts[2],
                                    Quantity = int.Parse(parts[3]),
                                    Price = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                                    LastRestocked = DateTime.ParseExact(parts[5], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                };
                            }).ToList();
            return inventory;
        }
        static void FiveProduktsWithLowSaldo()
        {
            Console.Clear();
            var fiveProduktsWithLowSaldo = inventory
                .OrderBy(p => p.Quantity)
                .Take(5);
            foreach (var produkt in fiveProduktsWithLowSaldo)
            {
                Console.WriteLine(produkt);
            }

        }
        static void FindLongTimeToRestock()
        {
            Console.WriteLine();
            var longTimeToRestock = inventory
                .Where(p => p.LastRestocked < DateTime.Now - new TimeSpan(30, 0, 0, 0))
                .OrderBy(p => p.LastRestocked);

            foreach (var product in longTimeToRestock)
            {
                Console.WriteLine(product);
            }

        }
        static void FindHighestAvgPriceOfCat()
        {




            var findHighestAvgPriceOfCat = inventory
                .GroupBy(p => p.Category)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    AvgPrice = g.Average(p => p.Price),
                    Totalprice = g.Sum(p => p.Price)

                });
            foreach (var products in findHighestAvgPriceOfCat)
            {
                Console.WriteLine(products.Category);
                Console.WriteLine($" - Count {products.Count}");
                Console.WriteLine($" - Avg Price {products.AvgPrice:F}");
                Console.WriteLine($" - Total Price {products.Totalprice:C}");

            }




        }
        static void FindProduktInvPriceOvertosundsofdollors()
        {
            var v = inventory
                .Where(p => p.Price * p.Quantity > 1000)
                .GroupBy(p => p.Name)
                .OrderBy(p => p.Key)
                .Select(p => new
                {
                    Produktname = p.Key,
                    Totalprice = p.Sum(p => p.Price * p.Quantity)

                });
            foreach (var item in v)
            {
                Console.WriteLine($"{item.Produktname} - {item.Totalprice:C}");

            }
        }
        static void FindTheTotolValueOfALL()
        {
            var a = inventory
                .Sum(p => p.Price * p.Quantity);
            Console.WriteLine($"{a:C}");
        }
    }
}