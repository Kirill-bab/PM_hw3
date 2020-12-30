using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Task_2._1
{
    /// <summary>
    /// For testers: you can edit Products.csv, Tags.csv and Inventory.csv files initial data in FillFiles() method
    /// if program ends uncorrectly or with exception (exiting code -1) be sure to delete a directory 
    /// by addres: D:\SomeDir
    /// </summary>
    class ERP_Bot
    {
        static string pathProducts = @"D:\SomeDir\Products.csv";
        static string pathTags = @"D:\SomeDir\Tags.csv";
        static string pathInventory = @"D:\SomeDir\Inventory.csv";
        static string localDirectoryPath = @"D:\SomeDir";

        static List<Product> products = new List<Product>();

        static List<Tag> tags = new List<Tag>();
        public static List<Remainings> remainings = new List<Remainings>();
           
        static int Main(string[] args)
        {
            try
            {
                Bot();
            }
            catch (Exception)
            {
                return -1;
            }
            return 0;
        }

        public static void Bot()
        {
            

            DirectoryInfo localDir = new DirectoryInfo(localDirectoryPath);
            localDir.Create();

            WriteInFile(pathProducts, "Id;Brand;Model;Price");
            WriteInFile(pathTags, "ProductId;Value");
            WriteInFile(pathInventory, "productId;location;Balance");

            FillFiles();
            FillProductsList();
            FillRemainingsList();
            FillTagsList();

            Console.WriteLine("Created by Kirill Babich\n");
            Console.WriteLine($"This program aloows you to interract with artificial database");
            Console.WriteLine("Simply follow the rules in menu\n");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("    Welcome to ERP Reports Bot!\n======================================\n   MENU");
                Console.WriteLine("1. Exit");
                Console.WriteLine("2. Products");
                Console.WriteLine("3. Remainings");
                Console.WriteLine("--------------------------------------");
                Console.Write("Enter option's number: ");
                bool inputIsCorrect = int.TryParse(Console.ReadLine().Trim(), out int answer);
                bool toExit = false;
                if (!inputIsCorrect)
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                switch (answer)
                {
                    case 1:
                        toExit = true;
                        break;
                    case 2:
                        Products();
                        break;
                    case 3:
                        Remaining();
                        break;
                    default:
                        inputIsCorrect = false;
                        break;
                }

                if (!inputIsCorrect)
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (toExit) break;
            }

            localDir.Delete(true);
            Console.WriteLine("Directory is deleted");
        }

        static void Remaining()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("\tREMAINNGS\n======================================");
                Console.WriteLine("1. Go back");
                Console.WriteLine("2. Seе abscent products");
                Console.WriteLine("3. See all products by remainings ascending");
                Console.WriteLine("4. See all products by remainings descending");
                Console.WriteLine("5. See remainings by product ID");
                Console.WriteLine("--------------------------------------");
                Console.Write("Enter option's number: ");
                bool inputIsCorrect = int.TryParse(Console.ReadLine().Trim(), out int answer);
                bool goBack = false;
                if (!inputIsCorrect)
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                switch (answer)
                {
                    case 1:
                        goBack = true;
                        break;
                    case 2:
                        Abscent();
                        break;
                    case 3:
                        ListRemainings(false);
                        break;
                    case 4:
                        ListRemainings(true);
                        break;
                    case 5:
                        RemainingsById();
                        break;
                    default:
                        inputIsCorrect = false;
                        break;
                }

                if (!inputIsCorrect)
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (goBack) break;
            }
        }
        static void RemainingsById()
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine(" PRODUCT REMAININGS BY ID\n======================================");
            Console.WriteLine("To find product remainings, please enter a product ID:");

            string input = Console.ReadLine().Trim().ToLower();

            if (!int.TryParse(input, out _))
            {
                Console.WriteLine("Wrong ID! (must be an integer)");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            int id = int.Parse(input);

            var list = remainings.Where(t => t.ProductId == id).OrderBy(r => r.Balance);

            if (list.Count() == 0)
            {
                Console.WriteLine("No matches found!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine(" Storage\tRemainings");
            Console.WriteLine("----------------------------");
            foreach (var item in list)
            {
                Console.WriteLine($" {item.Location} \t {item.Balance}");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void ListRemainings(bool byDescending)
        {
            Console.Clear();
            Console.WriteLine("==========================================================================");
            Console.WriteLine("\t\t\t Remainings List");
            Console.WriteLine("==========================================================================");

            if (byDescending) remainings = remainings.OrderByDescending(p => remainings.Where(rm => rm.ProductId == p.ProductId).Select(rms => rms.Balance).Sum()).ToList();
            else remainings = remainings.OrderBy(p => remainings.Where(rm => rm.ProductId == p.ProductId).Select(rms => rms.Balance).Sum()).ToList();

            var usedIDs = new List<int>();
            Console.WriteLine(" ID \t BRAND  \t MODEL  \t PRICE \t  REMAININGS\n");
            foreach (var remain in remainings)
            {
                var prod = products.Find(t => t.ID == remain.ProductId);
                if (usedIDs.Contains(prod.ID)) continue;
                Console.WriteLine(prod.ToString() + " " + (remain?.ToString() ?? " No recordings found!") );
                usedIDs.Add(prod.ID);
            }
            foreach (var prod in products)
            {
                if(remainings.Find(t => prod.ID == t.ProductId) == null)
                Console.WriteLine(prod.ToString() + " No recordings found!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static void Abscent()
        {
            Console.Clear();
            Console.WriteLine("==========================================================================");
            Console.WriteLine("\t\t\t ABSCENT PRODUCTS");
            Console.WriteLine("==========================================================================");
            var abscentProducts = products.Where(p => !(remainings.Select(r => r.ProductId)).Contains(p.ID) ||
            (remainings.Where(rm => rm.ProductId == p.ID && remainings.Where(rms => rms.ProductId == p.ID).Select(rms=>rms.Balance).Sum() == 0)).Select(rm=>rm.ProductId).Contains(p.ID)).ToList();
            if (abscentProducts.Count == 0)
            {
                Console.WriteLine("No matches found!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            abscentProducts = abscentProducts.OrderBy(p => p.ID).ToList();
            Console.WriteLine(" ID \t BRAND  \t MODEL  \t PRICE \t    TAGS\n");
            foreach (var product in abscentProducts)
            {
                var tag = tags.Find(t => t.ProductId == product.ID);
                Console.WriteLine(product.ToString() + tag.ToString());
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        static void Products()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("\tPRODUCTS\n======================================");
                Console.WriteLine("1. Go back");
                Console.WriteLine("2. Search product");
                Console.WriteLine("3. See all products by price ascending");
                Console.WriteLine("4. See all products by price descending");
                Console.WriteLine("--------------------------------------");
                Console.Write("Enter option's number: ");
                bool inputIsCorrect = int.TryParse(Console.ReadLine().Trim(), out int answer);
                bool goBack = false;
                if (!inputIsCorrect)
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                switch (answer)
                {
                    case 1:
                        goBack = true;
                        break;
                    case 2:
                        Search();
                        break;
                    case 3:
                        ListAll(false);
                        break;
                    case 4:
                        ListAll(true);
                        break;
                    default:
                        inputIsCorrect = false;
                        break;
                }

                if (!inputIsCorrect)
                {
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (goBack) break;
            }
        }

        static void Search()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("\tSearch Menu\n======================================");

                Console.WriteLine("To find product, please enter a string in format:\n\"ProductID;ProductModel;ProductBrand;ProductTag1,ProductTag2,...\"");
                string[] input = Console.ReadLine().Trim().ToLower().Split(";");
                if (input.Length != 4)
                {
                    Console.WriteLine("Wrong quantity of fields!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }
                if (!int.TryParse(input[0], out _))
                {
                    Console.WriteLine("Wrong ID! (must be an integer)");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                int id = int.Parse(input[0]);
                string model = input[1];
                string brand = input[2];
                string[] taggs = input[3].Split(",");

                Console.WriteLine("......................................................................");
                Console.WriteLine("\t\t\t Search results: ");
                Console.WriteLine("......................................................................");
               

                var first = products.FindAll(p => p.ID == id);

                var second = products.FindAll(p => p.Brand.ToLower() == brand || p.Model.ToLower() == model);

                var firstLength = first.Count;
                first.AddRange(second);
                if (first.Count == firstLength) firstLength = -1;
                first = first.Distinct(new IEqualityComparer()).ToList<Product>();
                var secondLength = first.Count;

                var third = new List<Product>();
                foreach (var item in taggs)
                {
                    foreach (var i in tags)
                    {
                        var collection = i.Value.Split(",");
                        if (collection.Contains(item) && item != "")
                        {
                            third.AddRange(products.Where(m => m.ID == i.ProductId));
                        }

                    }
                }
                first.AddRange(third);
                first = first.Distinct(new IEqualityComparer()).ToList<Product>();

                if(first.Count == 0)
                {
                    Console.WriteLine("No matches found!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                }
                Console.WriteLine(" ID \t BRAND  \t MODEL  \t PRICE \t    TAGS");
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("\t\t\t By ID");
                Console.WriteLine("----------------------------------------------------------------------");

                for (int i = 0; i < first.Count; i++)
                {
                    if (i == firstLength)
                    {
                        Console.WriteLine("----------------------------------------------------------------------");
                        Console.WriteLine("\t\t\t By Brand & Model");
                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                    else if( i== secondLength)
                    {
                        Console.WriteLine("----------------------------------------------------------------------");
                        Console.WriteLine("\t\t\t By Tags");
                        Console.WriteLine("----------------------------------------------------------------------");
                    }
                    var tag = tags.Find(t => t.ProductId == first[i].ID);
                    Console.WriteLine(first[i].ToString() + tag.ToString());
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            }
        }

        static void ListAll(bool byDescending)
        {
            Console.Clear();
            Console.WriteLine("==========================================================================");
            Console.WriteLine("\t\t\t Products List\n==========================================================================");

            if (byDescending) products = products.OrderByDescending(p => p.Price).ToList();
            else products.Sort(new PriceSorter());

            Console.WriteLine(" ID \t BRAND  \t MODEL  \t PRICE \t    TAGS\n");

            foreach (var item in products)
            {
                var tag = tags.Find(t => t.ProductId == item.ID);
                Console.WriteLine(item.ToString() + tag.ToString());
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void WriteInFile(string path, string message)
        {
            using (var file = new StreamWriter(path, true))
            {
                file.WriteLine(message);
            }
        }

        static string[] ReadFromFile(string path)
        {
            string[] str;
            
                using (var file = new StreamReader(path))
                {                
                    str = File.ReadAllLines(path);
                }
            return str;
        }

        static void FillFiles()
        {
            WriteInFile(pathProducts, "1;Puma;Sport;300");
            WriteInFile(pathProducts, "2;Nike;Aqua;350");
            WriteInFile(pathProducts, "14;Adidas;City;450");
            WriteInFile(pathProducts, "10;Puma;Aer;200");
            WriteInFile(pathProducts, "4;Puma;Outdoors;500");
            WriteInFile(pathProducts, "8;Reebok;Sport;100");
            WriteInFile(pathProducts, "44;Reebok;Aerobics;250");
            WriteInFile(pathProducts, "30;Nike;Night;399");
            WriteInFile(pathProducts, "56;Nike;Sport;156");
            WriteInFile(pathProducts, "15;Adidas;ActiveLife;555");
            WriteInFile(pathProducts, "6;Adidas;Jumper;339");


            WriteInFile(pathTags, "1;boots,2020,green");
            WriteInFile(pathTags, "2;boots,2019,blue");
            WriteInFile(pathTags, "14; boots,2015,gray");
            WriteInFile(pathTags, "10;boots,2018,white");
            WriteInFile(pathTags, "4;boots,2020,camo");
            WriteInFile(pathTags, "8;boots,2011,red");
            WriteInFile(pathTags, "44;boots,2017,purple");
            WriteInFile(pathTags, "30;boots,2020,neongreen");
            WriteInFile(pathTags, "56;boots,2018,yellow");
            WriteInFile(pathTags, "15;boots,2020,orange");
            WriteInFile(pathTags, "6;boots,2019,lightgreen");


            WriteInFile(pathInventory, "1;NewYork;300");
            WriteInFile(pathInventory, "2;Toronto;1000");
            WriteInFile(pathInventory, "14;Moscow;3000");
            WriteInFile(pathInventory, "10;Dnipro;216");
            WriteInFile(pathInventory, "10;Charkiv;0");
            WriteInFile(pathInventory, "4;Tbilisi;40");
            WriteInFile(pathInventory, "8;Yalta;200");
            WriteInFile(pathInventory, "44;Colorado;0");
            WriteInFile(pathInventory, "30;NewMexico;20");
            WriteInFile(pathInventory, "56;Minsk;400");
            WriteInFile(pathInventory, "15;Warshaw;420");
        }

        static void FillTagsList()
        {
            string[] prs = ReadFromFile(pathTags);
            bool flag = true;
            foreach (var item in prs)
            {
                if (flag)
                {
                    flag = !flag;
                    continue;
                }
                string[] parameters = item.Split(";");
                tags.Add(new Tag(int.Parse(parameters[0]), parameters[1]));
            }
        }
        static void FillProductsList()
        {
            string[] prs = ReadFromFile(pathProducts);
            bool flag = true;
            foreach (var item in prs)
            {
                if (flag)
                {
                    flag = !flag;
                    continue;
                }
                string[] parameters = item.Split(";");
                products.Add(new Product(int.Parse(parameters[0]), parameters[1], parameters[2],decimal.Parse(parameters[3])));
            }
        }
        static void FillRemainingsList()
        {
            string[] prs = ReadFromFile(pathInventory);
            bool flag = true;
            foreach (var item in prs)
            {
                if (flag)
                {
                    flag = !flag;
                    continue;
                }
                string[] parameters = item.Split(";");
                remainings.Add(new Remainings(int.Parse(parameters[0]), parameters[1], int.Parse(parameters[2])));
            }
        }
    }
    class IEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product first, Product second)
        {
            if (Object.ReferenceEquals(first, second)) return true;

            if (first == null || second == null) return false;

            return
                first.ID == second.ID &&
                String.Equals(first.Brand + first.Model, second.Brand + second.Model) &&
                first.Price == second.Price;
        }

        public int GetHashCode(Product product)
        {
            if (product == null) return 0;

            return
                product.ID.GetHashCode() +
                (product.Brand ?? String.Empty + product.Model ?? String.Empty).GetHashCode() +
                product.Price.GetHashCode();
        }
    }
    class Product
    {
        public int ID { get; }
        public string Brand { get; }
        public string Model { get; }
        public decimal Price { get; }

        public Product(int _ID, string _Brand, string _Model, decimal _Price)
        {
            ID = _ID;
            Brand = _Brand;
            Model = _Model;
            Price = _Price;
        }
        public override string ToString()
        {
            return $"[#{ID} \t \"{Brand}\" \t \"{Model}\"  \t ${Price}]";
        }
    }
    class Tag
    {
        public int ProductId { get; }
        public string Value { get; }

        public Tag(int _ProductId, string _Value)
        {
            ProductId = _ProductId;
            Value = _Value;
        }
        public override string ToString()
        {
            return $" ({Value})";
        }
    }
    class Remainings
    {
        public int ProductId { get; }
        public string Location { get; }
        public int Balance { get; }

        public Remainings(int _ProductId, string _Location, int _Balance)
        {
            ProductId = _ProductId;
            Location = _Location;
            Balance = _Balance;
        }
        public override string ToString()
        {
            return $"Total remainings: {ERP_Bot.remainings.Where(rm=>rm.ProductId ==this.ProductId).Select(r=>r.Balance).Sum()}";
        }
    }
    
   class PriceSorter : IComparer<Product>
    {
        public int Compare(Product a, Product b)
        {
            if (a.Price > b.Price) return 1;
            if (a.Price < b.Price) return -1;
            return 0;
        }
    }

}
