using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_1._2
{
    class CsGo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Created by Kirill Babich\n");
            Console.WriteLine($"This program shows distinct list of CS:GO players sorted by Age, Name and Rank separately\n");

            List<Player> Players = new List<Player>
            {
                new Player(29,"Ivan","Ivanenko",PlayerRank.Captain),
                new Player(19,"Peter","Petrenko",PlayerRank.Private),
                new Player(59,"Ivan","Ivanov",PlayerRank.General),
                new Player(52,"Ivan","Snezko",PlayerRank.Lieutenant),
                new Player(34,"Alex","Zeshko",PlayerRank.Captain),
                new Player(29,"Ivan","Ivanenko",PlayerRank.Captain),
                new Player(19,"Peter","Petrenko",PlayerRank.Private),
                new Player(34,"Vasiliy","Solkol",PlayerRank.Major),
                new Player(31,"Alex","Alexeenko",PlayerRank.Major),
                new Player(999,"Kratos","",PlayerRank.God),
            };

            Players = Players.Distinct(new IEqualityComparer()).ToList(); 

            Console.WriteLine("------------------------");
            Console.WriteLine("sorted by Age:");
            Players.Sort(new AgeSorter());
            
            foreach (var player in Players)
            {
                Console.WriteLine( player.ToString());
            }

            Console.WriteLine("------------------------");
            Console.WriteLine("sorted by Rank:");
            Players.Sort(new RankSorter());
           
            foreach (var player in Players)
            {
                Console.WriteLine(player.ToString());
            }

            Console.WriteLine("------------------------");
            Console.WriteLine("sorted by Name:");
            Players.Sort(new NameSorter());
            
            foreach (var player in Players)
            {
                Console.WriteLine(player.ToString());
            }
        }
    }

    enum PlayerRank
    {
        Private = 2,
        Lieutenant = 21, 
        Captain = 25, 
        Major = 29,
        Colonel = 33, 
        General = 39,
        God = 100
    }

    interface IPlayer
    {
        public int Age { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public PlayerRank Rank { get; }

    }

    class Player : IPlayer
    {
        public int Age { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public PlayerRank Rank { get; }

        public Player( int _Age, string _FirstName, string _LastName, PlayerRank _Rank)
        {
            Age = _Age;
            FirstName = _FirstName;
            LastName = _LastName;
            Rank = _Rank;
        }
        
        public override string ToString()
        {
            return $"[{Age},\"{FirstName}\",\"{LastName}\",{Rank}]";
        }
    }

    class IEqualityComparer : IEqualityComparer<Player>
    {
        public bool Equals(Player first, Player second)
        {
            if (Object.ReferenceEquals(first, second)) return true;

            if (first == null || second == null) return false;

            return 
                first.Age == second.Age &&
                String.Equals(first.FirstName + first.LastName, second.FirstName + second.LastName) &&
                first.Rank == second.Rank;
        }

        public int GetHashCode(Player player)
        {
            if (player == null) return 0;

            return
                player.Age.GetHashCode() +
                (player.FirstName ?? String.Empty + player.LastName ?? String.Empty).GetHashCode() +
                player.Rank.GetHashCode();
        }
    }

   
    class AgeSorter : IComparer<Player>
    {
        public int Compare(Player a, Player b)
        {
            if (a.Age > b.Age) return 1;
            if (a.Age < b.Age) return -1;
            return 0;
        }
    }
    class NameSorter : IComparer<Player>
    {
        public int Compare(Player a, Player b)
        {
            return String.Compare(a.FirstName + a.LastName, b.FirstName + b.LastName);
        }
    }
    class RankSorter : IComparer<Player>
    {
        public int Compare(Player a, Player b)
        {
            if (a.Rank > b.Rank) return 1;
            if (a.Rank < b.Rank) return -1;
            return 0;
        }
    }
}
