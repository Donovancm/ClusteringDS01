using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClusteringDS01.Reader
{
    public class CsvReader
    {
        public static Dictionary<string, double[]> GetData()
        {
            //<-----
            var dictionary = new Dictionary<string, double[]>();

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
                    for (int i = 1; i <= userArray.Length -1 ; i++)
                    {
                        string[] user = item.Split(",");
                        userArray[i-1] = user[i];
                        dictionary.Add(user[i], new double[32]);

                    }
                }
                else
                {
                    // dictionary key user 1
                    
                    int[] users = new int[100];
                    for (int i = 0; i < users.Length; i++)
                    {
                        string[] userOffers = item.Split(",");
                        if(userOffers[i+1] == "") {
                            double[] offers = dictionary[userArray[i]];
                            offers[index - 2] = 0;
                            dictionary[userArray[i]] = offers;


                        }
                        else {
                            users[i] = int.Parse(userOffers[i+1]);
                            double[] offers = dictionary[userArray[i]];
                            offers[index - 2] = int.Parse(userOffers[i + 1]);
                        }
                    }
                   
                    // dictionary user set value offers
                    // Adam, [1,3]
                    //persoon, offerte, wel of niet gekocht

               
                }
            }
            return dictionary;
        }
    }
}
