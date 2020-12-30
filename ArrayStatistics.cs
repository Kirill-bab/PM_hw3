using System;
using System.Linq;

namespace HW3
{
    class ArrayStatistics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Created by Kirill Babich\n");
            Console.WriteLine($"This program gives statistics of the entered array :\n> Minimal and Maximal elements\n> Sum of all elements\n" +
                 $"> Average value of array\n> Standart deviation\n\n");

            bool wrongInput = false;
            string[] input;
            int[] arr;
            do
            {
                Console.WriteLine("Please, enter integers array. Numbers must be divided with \",\" :");
                input = Console.ReadLine().Trim().Split(",");

                arr = input.Where(p => int.TryParse(p, out int temp) && p != String.Empty).Select(p=> int.Parse(p)).ToArray();
                wrongInput = input.Any(p => !int.TryParse(p, out int temp) && p != String.Empty);

                if (arr.Length < 1) wrongInput = true;
            } while (wrongInput);

            
            var min = arr.Min();
            var max = arr.Max();
            var sum = arr.Sum();
            var avg = arr.Average();
            var dev = Math.Sqrt(arr.Sum(p => Math.Pow(p - avg, 2)) / arr.Length);

            Console.WriteLine("------------------------");
            Console.WriteLine("Results:");
            Console.WriteLine($"Min element :\t\t{min}");
            Console.WriteLine($"Max element :\t\t{max}");
            Console.WriteLine($"Element sum:\t\t{sum}");
            Console.WriteLine($"Average value :\t\t{avg}");
            Console.WriteLine($"Normal deviation :\t{dev}");
            Console.WriteLine("-------------------------");
            Console.WriteLine("Sorted distinct array:");
            string output = String.Join(" ", arr.OrderBy(p => p).Distinct());
            Console.WriteLine(output);
        }
    }
}
