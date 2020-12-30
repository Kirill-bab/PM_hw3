using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_1._3
{
    class Dictionary
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Created by Kirill Babich\n");
            Console.WriteLine($"This program aloows you to create your own dictionary");
            Console.WriteLine("Simply follow the commands\n");
            int number;
            do
            {
                Console.WriteLine("Please, enter number of elements in Dictionary (must be greater than zero):");
                if (int.TryParse(Console.ReadLine(), out number) && number > 0) break;
                Console.WriteLine("Wrong input! Try again!");
            } while (true);
           

            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(number);
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine($"Element {i + 1}:");
                Console.Write("Enter brand:\t");
                string brand = Console.ReadLine();
                Console.Write("Enter country:\t");
                string country = Console.ReadLine();

                TKey newRegion = new TKey(brand, country);

                if (dictionary.ContainsKey(newRegion))
                {
                    Console.WriteLine("Element with such Key already exists!");
                    Console.WriteLine("Try again!\n");
                    i--;
                    continue;
                }
                Console.Write("Enter web-site:\t");
                TValue newValue = new TValue(Console.ReadLine());

                dictionary.Add(newRegion, newValue);
            }
            Console.WriteLine("============================");
            Console.WriteLine("YOUR DICTIONARY:");

            foreach (var term in dictionary)
            {
                Console.WriteLine($"[{term.Key.Brand},{term.Key.Country}] = [{term.Value.WebSite}]");
            }
        }
    }
    //-1706287298
    //-1706287298

    public interface IRegion 
    {
        public string Brand { get; }
        public string Country { get; } 
    }
    public interface IRegionSettings 
    { 
        public string WebSite { get; }
    }

    class TKey : IRegion
    {
        public string Brand { get; }
        public string Country { get; }

        public TKey(string _Brand, string _Country)
        {
            Brand = _Brand;
            Country = _Country;
        }

        public override int GetHashCode()
        {
            int a = Brand.GetHashCode() ^ Country.GetHashCode();
            return a;
        }

        public override bool Equals(object other)
        {
            if (other is TKey)
            {
                return ((TKey)other).Brand.Equals(this.Brand) && ((TKey)other).Country.Equals(this.Country);
            }
            return false;
        }
    }

    class TValue : IRegionSettings
    {
        public string WebSite { get; }

        public TValue(string _WebSite)
        {
            WebSite = _WebSite;
        }
    }
}
