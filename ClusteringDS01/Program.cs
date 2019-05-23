using System;

namespace ClusteringDS01
{
    class Program
    {
        public static string targetUser;
        public static string secTargetUser;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var dictionary = Reader.CsvReader.GetData();
            //var Adams = dictionary["Adams"];
            //var Allen = dictionary["Allen"];
            PickTargetUsers();
            var result =  Distances.Euclidean.ComputeDistance(dictionary[targetUser], dictionary[secTargetUser]);
            Console.WriteLine(result);
            Console.ReadLine();
        }
        public static void PickTargetUsers()
        {
            //Reminder weergeven lijst van beschikbare personen
            //Kies eerste persoon en daarna kies 2de persoon



            Console.WriteLine("Selecteer eerste persoon");
            targetUser = Console.ReadLine()+"";
            Console.WriteLine("Selecteer tweede persoon");
            secTargetUser = Console.ReadLine() + "";

        }
    }
}
