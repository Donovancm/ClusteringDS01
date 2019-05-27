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
            // init variables
            double[,] offers = CreateMatrix();
            List<double[,]> kMatrixList = new List<double[,]>();


            // Code to setupt offers matrix
            var dictionary = new Dictionary<string, double[]>();

            // setup Offers Matrix
            setupOffersMatrix(offers);

            // set initial K -> K = 4 as centroids
            InitializeK(4);
            Console.ReadLine();
            //place K Randomly at first time.
            // then after first time place in center of centroids own cluster/points


            // step1: create matrixs for centroids k amount
            // step 2: eucl distance from centroid to offers/items
            // step 3: shortest distance of centroids and offers/item assign to point object {kl.nr,pr.nr, c.nr} list/dictionary.
            // step 4: calculate sse store to calc smallest sse value of new and old 
            // step 5: repeat 200-500 times and move centroids each time.
            return dictionary;
        }
        public static double[,] CreateMatrix()
        {
            //Skip last row and last column //
            return new double[32, 101];
        }
        public static List<double[,]> InitializeK(int k)
        {
            List<double[,]> kMatrixList = new List<double[,]>();
            for (int i = 0; i < k; i++)
            {
                kMatrixList.Add(CreateMatrix());
            }
            return kMatrixList;
        }

        public static void setupOffersMatrix(double[,] matrix)
        {
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
