using ClusteringDS01.Distances;
using ClusteringDS01.Model;
using ClusteringDS01.Reader;
using System;
using System.Collections.Generic;

namespace ClusteringDS01
{
    class Program
    {
        public static string targetUser;
        public static string secTargetUser;
        public static Dictionary<int, int[,]> clusterPoints;
        public static double[,] offers;
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //var dictionary = Reader.CsvReader.GetData();
            ////var Adams = dictionary["Adams"];
            ////var Allen = dictionary["Allen"];
            //PickTargetUsers();
            //IDistance iDistance = null;
            //iDistance = new Euclidean();
            //var result =  iDistance.ComputeDistance(dictionary[targetUser], dictionary[secTargetUser]);
            //Console.WriteLine(result);
            //Console.ReadLine();
            Init();

        }
        public static void PickTargetUsers()
        {
            //Reminder weergeven lijst van beschikbare personen
            //Kies eerste persoon en daarna kies 2de persoon
            Console.WriteLine("Selecteer eerste persoon");
            targetUser = Console.ReadLine() + "";
            Console.WriteLine("Selecteer tweede persoon");
            secTargetUser = Console.ReadLine() + "";

        }

        public static List<CentroidPosition> PlaceRandomCentroid(int k)
        {
            List<CentroidPosition> kList = new List<CentroidPosition>();
            for (int i = 0; i < k; i++)
            {
                Random rng = new Random();
                var Y = rng.Next(0, 100);
                var X = rng.Next(0, 31);
                kList.Add(new CentroidPosition(i + 1, X, Y));
            }
            return kList;
        }

        public static List<double[,]> InitializeK(int k)
        {
            List<double[,]> kMatrixList = new List<double[,]>();
            for (int i = 0; i < k; i++)
            {
                double[,] offers = CsvReader.CreateMatrix();
                CsvReader.SetupOffersMatrix(offers);
                kMatrixList.Add(offers);
            }
            return kMatrixList;
        }



        public static void CalcDistances(double[,] offers, List<double[,]> kList, int[,] centroidLocation)
        {
            // offers[0,0] = 1
            // centr 1 is on location[12,12]
            // calc distance of offers[0,0] = 1 and centr 1
            // place distance in centroid 1 matrix.
            // repeat with each of the 4 centroids
        }

        public static void Init()
        {
            // init variables
            offers = CsvReader.GetData();
            List<double[,]> kMatrixList = new List<double[,]>();
            // Cluster points
            clusterPoints = new Dictionary<int, int[,]>(); //  key klnr , productnr-> = {[0,0] , centroidnr --> = [0,1]} /// int[,] -> = {row = 32}, (columns = 2) [1] -> prdnr, [2] -> centroidnr
            Dictionary<int, double[,]> centroidDistances = new Dictionary<int, double[,]>(); // key centroidnr , kln_nr-> = {[0,0] , distance --> = [0,1]} /// int[,] -> = {row = 100}, (columns = 2) [1] -> kln_nr, [2] -> distance


            // set initial K -> K = 4 as centroids with X & Y position
            int k = 4;
            int[,] centroidLocation = new int[k, 2]; // matrix to save current centroids location
            kMatrixList = InitializeK(k);


            //place K Randomly at first time.

            var centroidPositionList = PlaceRandomCentroid(k);

            //eucl distance from centroid to offers / items
            CalcCentroidDistance(centroidPositionList, offers, centroidDistances);

            // step: 3 after calc distances add data to cluster dictionary {klnr prodnr centroidnr} 

            Console.ReadLine();

            /* START HERE NEXT TIME!
             * then after first time place in center of centroids own cluster/points 
             * checks centroids items positions min and max of rows and columns to place centroid randomly between min and max position
             */

            // step: 3 after calc distances add data to cluster dictionary {klnr prodnr centroidnr} 


            // step1: create matrixs for centroids k amount : done
            // step 2: eucl distance from centroid to offers/items : done
            // step 3: shortest distance of centroids and offers/item assign to point object {kl.nr,pr.nr, c.nr} clusterPoints/dictionary.todo <--
            // step 3.5 Complete mergepoints / blabla methode  todo <---
            // step 4: calculate sse store to calc smallest sse value of new and old 
            // step 5: repeat 200-500 times and move centroids each time.
        }
        public static void CalcCentroidDistance(List<CentroidPosition> cList, double[,] offers, Dictionary<int, double[,]> centroidDistance)
        {
            foreach (var centroid in cList)
            {
                double[] pValue = new double[31];
                int klantnr = int.Parse(centroid.Y + "");
                for (int i = 0; i < 31; i++)
                {
                    pValue[i] = offers[i, klantnr];
                }
                DistanceToCentroid(centroid.number, pValue, centroidDistance, offers);
            }

        }

        public static void DistanceToCentroid(int cnumber, double[] pValue, Dictionary<int, double[,]> centroidDistance, double[,] offers)
        {
            int row = offers.GetLength(0) - 1;
            int column = offers.GetLength(1) - 1;
            double[,] distances = new double[101, 2];
            for (int i = 0; i < column; i++)
            {
                double[] array = new double[31];
                for (int j = 0; j < row; j++)
                {
                    array[j] = offers[j, i];
                }
                IDistance iDistance = null;
                iDistance = new Euclidean();
                distances[i, 0] = i; // kln_nr
                distances[i, 1] = iDistance.ComputeDistance(array, pValue); //ecl ding array en pValue door geven
            }
            // 0 to 99
            centroidDistance.Add(cnumber, distances);
        }

        public static void AssignToCluster(Dictionary<int,double[,]> centroidDistance, Dictionary<int,int[,]> clusterPoints)
        {
            //dictionary of clusters
            for (int i = 0; i < 101; i++) // loops through kln_nr amount
            {
                // gets i {kln_nr} of centroiddistance dictionary -> then pick the smallest value
                double[,] distance = new double[4, 3]; // K{aantal centroids} = 4
                int index = 0 ;
                foreach (var centroid in centroidDistance)
                {
                    distance[index, 0] = centroid.Key;
                    distance[index, 1] = centroid.Value[index,0]; // klant nummer
                    distance[index, 2] = centroid.Value[index, 1]; // distance
                }
                // method die de 2d array object opslaat in de behoorde cluster 2darray
                blabla(ShortestDistance(distance));
                //ShortestDistance( distance)[0] return centroidnummer en klnt -> then add to dictionary
            }
            // cluster = key klnr , productnr-> = {[0,0] , centroidnr --> = [0,1]} /// int[,] -> = {row = 32}, (columns = 2) [1] -> prdnr, [2] -> centroidnr
            // loop door dictionary of centroid -> add distances to array of kln_nr -> check shortes -> add to cluster 
            // 
        }

        public static void blabla(double[,] distance)// hulp methode voor complex add points to cluster
        {
            // check if cluster dictionary if centroid is not empty -> if not then create a new 2darray with double the row

            var centroidnumber = distance[0, 0];
            var klnt_nr = distance[0, 1];

            var key = int.Parse(klnt_nr + "");
            int[,] value = GetOfferValues(offers, key);
            int row = value.GetLength(0);
            int column = value.GetLength(1);
            int[,] clusterArray = new int[row, column];
            if (true) // check if value of dictionany key is empty
            {
                row = value.GetLength(0) + clusterPoints[key].GetLength(0);// if dictionary is empty
                column = value.GetLength(1) + clusterPoints[key].GetLength(1);// if dictionary is empty
                clusterArray = new int[row, column];
                // method om alle oude en nieuw points in de new clusterArray te zetten
                //MergePoints()
            }
            else
            {
                // zet value to dictionary key p
                // Centroids toepassen
            }
        }

        public static int[,] GetOfferValues(double[,] offers, int klnt_nr)
        {
            //zoek alle points in offers van klant nummer en return die points als 2darray
            return new int[1, 1];
        }

        public static void MergePoints(int[,] clusterArray, int[,] offersPoints)
        {
            // alle oude en nieuw points in de new clusterArray te zetten
            // Centroids toepassen
        }

        public static double[,] ShortestDistance(double[,] datas)
        {
            var data = datas;
            var number = data.GetLength(0);
            var array = new double[number];
            var newArray = new double[number, 3];
            for (int i = 0; i <= data.GetLength(0) - 1; i++)
            {
                array[i] = data[i, 2];
            }
            Array.Sort(array);
            for (int a = 0; a <= data.GetLength(0) - 1; a++)
            {
                for (int b = 0; b <= data.GetLength(0) - 1; b++)
                {
                    if (array[a] == data[b, 2])
                    {
                        newArray[a, 0] = data[b, 0];
                        newArray[a, 1] = data[b, 1];
                        newArray[a, 2] = data[b, 2];
                    }
                }
            }
            return newArray;
        }
        //Array.Sort(array);
        //if (array[0] == distance)
        //{
        //    return true;
        //}
        //return false;
    }
}
