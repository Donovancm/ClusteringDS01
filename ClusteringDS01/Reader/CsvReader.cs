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
            Dictionary<int, int[,]> list = new Dictionary<int, int[,]>(); //  key klnr , productnr-> = {[0,0] , centroidnr --> = [0,1]} /// int[,] -> = {row = 32}, (columns = 2) [1] -> prdnr, [2] -> centroidnr

            // Code to setup offers matrix
            var dictionary = new Dictionary<string, double[]>();

            // setup Offers Matrix
            SetupOffersMatrix(offers);

            // set initial K -> K = 4 as centroids
            int[,] centroidLocation = new int[4,2]; // matrix to save current centroids location
            kMatrixList = InitializeK(4);

            //place K Randomly at first time.
            PlaceRandomCentroid(kMatrixList, centroidLocation);


            Console.ReadLine();

            /* START HERE NEXT TIME!
             * then after first time place in center of centroids own cluster/points 
             * checks centroids items positions min and max of rows and columns to place centroid randomly between min and max position
             */


            //eucl distance from centroid to offers / items


            // step: 3 after calc distances add data to cluster dictionary {klnr prodnr centroidnr} 


            // step1: create matrixs for centroids k amount : done
            // step 2: eucl distance from centroid to offers/items : todo <--
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

        public static void SetupOffersMatrix(double[,] matrix)
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

        public static void PlaceRandomCentroid(List<double[,]> kList, int[,] centroidLocation)
        {
            int centroid = 0; // number to indicate centroid
            int index = 0; // number to indicate location of a centroid
            foreach (var k in kList)
            {
                centroid++;
                Random rng = new Random();
                var Y = rng.Next(0,100);
                var X = rng.Next(0, 31);
                k[X, Y] = centroid;
                centroidLocation[index,0] = X;
                centroidLocation[index,1] = Y;
                index++;
            }
        }

        public static void CalcDistances(double[,] offers, List<double[,]> kList, int[,] centroidLocation)
        {
            // offers[0,0] = 1
            // centr 1 is on location[12,12]
            // calc distance of offers[0,0] = 1 and centr 1
            // place distance in centroid 1 matrix.
            // repeat with each of the 4 centroids
        }
    }
}
