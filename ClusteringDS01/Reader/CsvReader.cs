using ClusteringDS01.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClusteringDS01.Reader
{
 
    public class CsvReader
    {
       
        public static double[,] GetData()
        {
            // init variables
            double[,] offers = CreateMatrix(); 
            // setup Offers Matrix = Points
            SetupOffersMatrix(offers);
            return offers;
        }

        public static double[,] CreateMatrix()
        {
            //Skip last row and last column //
            return new double[32, 101];
        }

        public static void SetupOffersMatrix(double[,] matrix)
        {
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader("C:/Users/Donovan/source/repos/ClusteringDS01/ClusteringDS01/Data/Winecraft.csv"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to clusterPoints.
                    Console.WriteLine(line); // Write to console.
                }
            }

            int row = 0;
            //string[] userArray = { };
            foreach (var item in list)
            {
                // After the the first row comes offers data
                if (row > 0)
                {

                    // set matrix of offers of clients and products
                    int[] users = new int[100];

                    for (int i = 0; i < users.Length; i++)
                    {
                        string[] userOffers = item.Split(",");
                        if (userOffers[i + 1] == "")
                        {
                            // if  useroffer is empty string then set 0 in matrix
                            matrix[row-1,i]= 0.0;
                        }
                        else
                        {
                            //else useroffer is not empty then set 1 in matrix
                            matrix[row - 1, i] = 1.0;
                        }
                    }
                }
                // set row index to the next line/ the follow up productid of users offers
                row++;
            }
        }

  
    }
}
