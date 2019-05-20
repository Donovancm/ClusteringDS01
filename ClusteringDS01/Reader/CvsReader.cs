using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClusteringDS01.Reader
{
    public class CsvReader
    {
        public static Dictionary<string, int[]> GetData()
        {
            var dictionary = new Dictionary<string, int[]>();

            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader("C:/Users/Donovan/source/repos/ClusteringDS01/ClusteringDS01/Data/Winecraft.csv"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to list.
                    Console.WriteLine(line); // Write to console.
                }
            }
            int index = 0;
            string[] userArray = { };
            foreach (var item in list)
            {
                index++;

                if (index == 1)
                {
                    userArray = new string[101];
                    for (int i = 0; i <= userArray.Length -1 ; i++)
                    {
                        string[] user = item.Split(",");
                        userArray[i] = user[i];

                    }
                }
                else
                {
                    // dictionary key user 1
                    
                    int[] offers = new int[32];
                    for (int i = 1; i < offers.Length; i++)
                    {
                        string[] userOffers = item.Split(",");
                        if(userOffers[i] == "") { offers[i] = 0; }
                        else { offers[i] = int.Parse(userOffers[i]); }
                    }
                    dictionary.Add(userArray[index], offers);
                    // dictionary user set value offers
                    // Adam, [1,3]
               
                }
            }
            return dictionary;
        }
    }
}
