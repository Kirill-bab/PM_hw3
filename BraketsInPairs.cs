using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_1._4
{
    class BraketsInPairs
    {
        static void Main(string[] args)
        {
            string input;
            Console.WriteLine("Created by Kirill Babich\n");
            Console.WriteLine($"This program determines wether each bracket has it's pair\n\n");
            while (true)
            {
                Console.WriteLine("Please, enter expression: ");
                 input = Console.ReadLine().Trim();
                if (input.Length < 1)
                {
                    Console.WriteLine("wrong input! You entered empty row!");
                    continue;
                }
                break;
            }
            

            int answer = PairsCheck(input);
            if (answer == -1)
            {
                Console.WriteLine("Everything is Ok!");
            }
            else
            {
                if ("{[<(".Contains(input[answer]))
                {
                    Console.WriteLine($"Error at position {answer}: no closing bracket for {input[answer]}");
                }
                else
                {
                    Console.WriteLine($"Error at position {answer}: no opening bracket for {input[answer]}");
                }
            }

        }

        static int PairsCheck(string str)
        {
            string brakets = "[{(<]})>";
            Stack<char> stack = new Stack<char>();

            for (int i = 0; i < str.Length; i++)
            {
                int f = brakets.IndexOf(str[i]);

                if (f >= 0 && f <= 3)
                    stack.Push(str[i]);

                if (f > 3)
                {
                    if (stack.Count == 0 || stack.Pop() != brakets[f - 4])
                        return i;
                }
            }

            if (stack.Count != 0)
                return str.LastIndexOf(stack.Pop());

            return -1;
        }

    }
}
