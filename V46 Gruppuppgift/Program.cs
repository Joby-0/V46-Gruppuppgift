namespace Query_Expressions_Gruppövning
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Metrics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

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

            // Implementera query expressions här
            CategoryList();

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

        public static void Tools()
        {
            //Lista alla produkter i kategorin "Verktyg" sorterade efter pris(stigande).

            var tools = from t in inventory
                        where t.Category == "Verktyg"
                        select t;

            foreach (var t in tools)
            {
                Console.WriteLine(t);
            }
        }

        public static void CategoryList()
        {
            //Gruppera produkterna efter kategori och visa antalet produkter i varje kategori.

            var kategori = from k in inventory
                           group k by k.Category into Categories
                           select new { 
                               Categories = Categories.Key, 
                               quantityNumber = Categories.Count() };

            foreach (var k in kategori)
            {
                Console.WriteLine($"{k.Categories} Antal: {k.quantityNumber}");
            }
                           
        }
    }
}