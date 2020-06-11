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
        public static double sseAverageCentroidOne = 0.0;
        public static double sseAverageCentroidTwo = 0.0;
        public static double sseAverageCentroidThree = 0.0;
        public static double sseAverageCentroidFour = 0.0;

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
                double Y = rng.NextDouble()*100;
                double X = rng.NextDouble()*31;
                kList.Add(new CentroidPosition(i + 1, X, Y));
            }
            return kList;
        }

        //public static List<double[,]> InitializeK(int k)
        //{
        //    List<double[,]> kMatrixList = new List<double[,]>();
        //    for (int i = 0; i < k; i++)
        //    {
        //        double[,] offers = CsvReader.CreateMatrix();
        //        CsvReader.SetupOffersMatrix(offers);
        //        kMatrixList.Add(offers);
        //    }
        //    return kMatrixList;
        //}



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
            //kMatrixList = InitializeK(k);


            //place K Randomly at first time.

            var centroidPositionList = PlaceRandomCentroid(k);

            //eucl distance from centroid to offers / items
            CalcCentroidDistance(centroidPositionList, offers, centroidDistances);

            // step: 3 + 3.5 | after calc distances add data to cluster dictionary {klnr prodnr centroidnr}| 
            AssignToCluster(centroidDistances, clusterPoints);
            

            // step: 4 |  calculate average of all SSE of clusterpoints assigned to a certain Centroid.   Average SSE == Centre --> relocate Centroid to Centrepoint
            var clusterProducts = GetClusterProducts();
            var centroidsAvgSSE = CalcAverageSSECentroids(clusterProducts, centroidDistances);


            // step: 4.5 | vergelijk oude sse met nieuw Sse
            UpdateSSE(centroidsAvgSSE);
            Console.ReadLine();

            // stap 5: | relocate centroids

            /* START HERE NEXT TIME!
             * then after first time place in center of centroids own cluster/points 
             * checks centroids items positions min and max of rows and columns to place centroid randomly between min and max position
             */

            // step: 3 after calc distances add data to cluster dictionary {klnr prodnr centroidnr} 



            // step1: create matrixs for centroids k amount : done
            // step 2: eucl distance from centroid to offers/items : done
            // step 3: shortest distance of centroids and offers/item assign to point object {kl.nr,pr.nr, c.nr} clusterPoints/dictionary.done
            // step 3.5 Complete mergepoints / blabla methode  done
            // step 4: calculate sse store to calc smallest sse value of new and old todo <---
            // step 5: repeat 200-500 times and move centroids each time.
        }

        public static double CompareSSE( double sseNew, double sseOld)
        {
            if (sseOld != 0.0)
            {
                return sseOld > sseNew ? sseNew : sseOld;
            }
            return sseNew;
            
        }

        public static void UpdateSSE(Dictionary<int, double> centroidsAvgSSE)
        {
            foreach (var centroid in centroidsAvgSSE)
            {
                switch (centroid.Key)
                {
                    case 1:
                        sseAverageCentroidOne = CompareSSE(centroid.Value , sseAverageCentroidOne);
                        break;
                    case 2:
                        //do iets
                        sseAverageCentroidTwo = CompareSSE(centroid.Value, sseAverageCentroidTwo);
                        break;
                    case 3:
                        //do iets
                        sseAverageCentroidThree = CompareSSE(centroid.Value, sseAverageCentroidThree);
                        break;
                    case 4:
                        //do iets
                        sseAverageCentroidFour = CompareSSE(centroid.Value, sseAverageCentroidFour);
                        break;
                    default:
                        break;
                }
            }
        }

        public static Dictionary<int, double> CalcAverageSSECentroids(Dictionary<int, HashSet<double>> clusterProducts, Dictionary<int, double[,]> centroidDistance)
        {
            double avgTimes = 0.0;
            double avgSSE = 0.0;
            Dictionary<int, double> centroidsAvgSSE = new Dictionary<int, double>();
            foreach (var centroid in clusterProducts) // 1
            {
               
                foreach (var centroidD in centroidDistance) //1
                {
                    avgSSE = 0.0;
                    int row = centroidD.Value.GetLength(0) - 1;
                    avgTimes = 0.0;
                    for (int i = 0; i < row; i++)// 1 -> klanten
                    {
                        if (centroid.Value.Contains(centroidD.Value[i,0])) // per klnr
                        {
                            avgSSE += centroidD.Value[i, 1];
                            avgTimes++;
                        }
                    }
                }
                centroidsAvgSSE.Add(centroid.Key, (avgSSE / avgTimes));
            }
            return centroidsAvgSSE;
        }

        public static Dictionary<int, HashSet<double>> GetClusterProducts()
        {
            //HashSet<int> uniqueList = new HashSet<int>();
            Dictionary<int, HashSet<double>> clusterProducts = new Dictionary<int, HashSet<double>>(); //key => centroid, list => klnr
            for (int centroidnr = 1; centroidnr <= 4;)
            {
                foreach (var point in clusterPoints)
                {
                    int row = point.Value.GetLength(0) - 1;
                    for (int j = 0; j < row; j++)
                    {
                        if (point.Value[j, 1] == centroidnr)
                        {
                            if (clusterProducts.ContainsKey(centroidnr))
                            {
                                clusterProducts[centroidnr].Add(point.Key); // add klnr
                            }
                            else
                            {
                                HashSet<double> values = new HashSet<double>();
                                values.Add(point.Key);
                                clusterProducts.Add(centroidnr, values);
                            }
                        }
                    }
                   
                }
                centroidnr++;
            }
            return clusterProducts;
        }

        public static void CalcCentroidDistance(List<CentroidPosition> cList, double[,] offers, Dictionary<int, double[,]> centroidDistance)
        {
            foreach (var centroid in cList)
            {
                double[] pValue = new double[31];
                int klantnr = int.Parse(Math.Round(centroid.Y).ToString());
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
            // Dictonary[CentroidNummer,[klantnr,Distance]]
            centroidDistance.Add(cnumber, distances);
        }

        public static void AssignToCluster(Dictionary<int, double[,]> centroidDistance, Dictionary<int, int[,]> clusterPoints)
        {
            //dictionary of clusters
            for (int i = 0; i < 101; i++) // loops through kln_nr amount
            {
                // gets i {kln_nr} of centroiddistance dictionary -> then pick the smallest value
                double[,] distance = new double[4, 3]; // K{aantal centroids} = 4
                int index = 0;
                foreach (var centroid in centroidDistance)
                {
                    // Example   (3,50, 2.49)
                    distance[index, 0] = centroid.Key;
                    distance[index, 1] = centroid.Value[i, 0]; // klant nummer
                    distance[index, 2] = centroid.Value[i, 1]; // distance
                    index++;
                }
                // method die de 2d array object opslaat in de behoorde cluster 2darray
                AssignToClusterExtension(ShortestDistance(distance));
                //ShortestDistance( distance)[0] return centroidnummer en klnt -> then add to dictionary
            }
        }

        public static void AssignToClusterExtension(double[,] distance)// hulp methode voor complex add points to cluster
        {
            var centroidnumber = int.Parse(distance[0, 0] + "");
            var key = int.Parse(distance[0, 1] + ""); // klnr
            int[,] value = GetOfferValues(offers, key);
            int row = value.GetLength(0);
            int column = value.GetLength(1);
            int[,] clusterArray = new int[row, column];

            if (clusterPoints.ContainsKey(key)) // check if value of dictionany key is empty
            {
                value = AssignProductToCentroid(centroidnumber, value);// 0101010101010
                MergePoints(key, value);
            }
            else
            {
                value = AssignProductToCentroid(centroidnumber, value);
                clusterPoints.Add(key, value);
            }
        }

        public static int[,] AssignProductToCentroid(int centroid, int[,] value)
        {
            //new 2d array with 2 colums and 32 rows: column[1] = prd_nr , column[2] = centroid
            int row = value.GetLength(0);
            int column = value.GetLength(1);
            int[,] productCentroidArray = new int[row, 2];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (value[i, j] == 1) //  It shows if product has been bought
                    {
                        productCentroidArray[i, 0] = i; //product number
                        productCentroidArray[i, 1] = centroid; //centroid number
                    }
                    else //Not bought by any of the clients
                    {
                        productCentroidArray[i, 0] = i; //product number
                        productCentroidArray[i, 1] = 0; //centroid number
                    }
                }
            }
            return productCentroidArray;
        }

        public static int[,] GetOfferValues(double[,] offers, int klnt_nr)
        {
            //zoek alle points in offers van klant nummer en return die points als 2darray
            int[,] klnrArray = new int[32, 1];
            for (int i = 0; i < offers.GetLength(0); i++)
            {
                for (int j = 0; j < offers.GetLength(1); j++)
                {
                    if (j == klnt_nr)
                    {
                        klnrArray[i,0] = int.Parse(offers[i,j]+"");
                    }
                }
            }
            return klnrArray;
        }

        public static void MergePoints(int key, int[,] value)
        {
            int newRow = value.GetLength(0) + clusterPoints[key].GetLength(0);
            int[,] clusterArray = new int[newRow, 2]; // cnr, prod/offer
            int[,] oldArray = clusterPoints[key];
            int lastIndexOfOldArray = oldArray.GetLength(0) - 1;
            int row = clusterArray.GetLength(0);
            int column = clusterArray.GetLength(1);
            bool oldArrayDone = false;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (i <= lastIndexOfOldArray && !oldArrayDone)              
                    {
                        clusterArray[i, j] = oldArray[i, j];
                    }
                    else
                    {
                        if (i == lastIndexOfOldArray && !oldArrayDone)
                        {
                            oldArrayDone = true;
                        }
                        int index = i - (lastIndexOfOldArray + 1);
                        clusterArray[i, j] = value[index, j];
                    }

                }
            }
            clusterPoints[key] = clusterArray;
        }

        public static double[,] ShortestDistance(double[,] datas)
        {
            var data = datas;
            var number = data.GetLength(0);
            var array = new double[number];
            var newArray = new double[number, 3];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                array[i] = data[i, 2];
            }
            Array.Sort(array);
            for (int a = 0; a < data.GetLength(0) ; a++)
            {
                for (int b = 0; b < data.GetLength(0); b++)
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
    }
}
