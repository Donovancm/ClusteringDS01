using ClusteringDS01.Distances;
using ClusteringDS01.Model;
using ClusteringDS01.Reader;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
        public static Dictionary<int, double> centroidsAvgSSE { get; set; }
        public static Dictionary<int, HashSet<double>> clusterClients { get; set; }
        public static Dictionary<int, double[,]> centroidDistances { get; set; }

        public static Dictionary<int, List<int>> centroids { get; set; }
        public static Dictionary<int, List<Tuple<int,double>>> centroidDistances_New { get; set; }

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
            //for (int i = 0; i < k; i++)
            //{
            //    Random rng = new Random();
            //    double X = rng.NextDouble() * 100; //50: 2 = 25
            //    double Y = rng.NextDouble() * 31; //15:2 = 
            //    kList.Add(new CentroidPosition(i + 1, X, Y));
            //    Console.WriteLine("Centroid " + i + " with the location X of " + X + " with the location Y of " + Y);

            //}
            kList.Add(new CentroidPosition(1,  24.0, 7.0));
            kList.Add(new CentroidPosition(2,  49.5, 14.0));
            kList.Add(new CentroidPosition(3,  69.9, 21.0));
            kList.Add(new CentroidPosition(4, 98.4, 28.0));
            foreach (var centroid in kList)
            {
                Console.WriteLine(centroid.number + " " + centroid.X + " " + centroid.Y);
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
            
           

            // set initial K -> K = 4 as centroids with X & Y position
            int k = 4;
            int[,] centroidLocation = new int[k, 2]; // matrix to save current centroids location
                                                     //kMatrixList = InitializeK(k);
            var centroidPositionList = new List<CentroidPosition>();

            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine("Total iteration: "+(i+1));
                // Cluster points
                clusterPoints = new Dictionary<int, int[,]>(); //  key klnr , productnr-> = {[0,0] , centroidnr --> = [0,1]} /// int[,] -> = {row = 32}, (columns = 2) [1] -> prdnr, [2] -> centroidnr
                centroidDistances = new Dictionary<int, double[,]>(); // key centroidnr , kln_nr-> = {[0,0] , distance --> = [0,1]} /// int[,] -> = {row = 100}, (columns = 2) [1] -> kln_nr, [2] -> distance


                if (i == 0)
                {
                    //OLD: place K Randomly at first time.
                    centroidPositionList = PlaceRandomCentroid(k);

                    //NEW: place K Randomly at first time.
                    centroids = Centroid.Initialize(k);

                }


                //eucl distance from centroid to offers / items
                centroidDistances_New = new Dictionary<int, List<Tuple<int, double>>>();
                CalcCentroidDistance();

                // step: 3 + 3.5 | after calc distances add data to cluster dictionary {klnr prodnr centroidnr}| 
                AssignToCluster();


                // step: 4 |  calculate average of all SSE of clusterpoints assigned to a certain Centroid.   Average SSE == Centre --> relocate Centroid to Centrepoint
                clusterClients = GetClusterClients();
                centroidsAvgSSE = CalcAverageSSECentroids(clusterClients, centroidDistances);


                // step: 4.5 | vergelijk oude sse met nieuw Sse
                UpdateSSE(centroidsAvgSSE);

                if(i > 0)
                {
                    // stap 5: | relocate centroids

                    centroidPositionList = RelocateCentroidsPositions(k, centroidsAvgSSE, centroidDistances, clusterClients);
                }

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

        }
        public static List<CentroidPosition> RelocateCentroidsPositions(int k, Dictionary<int, double> centroidsAvgSSE, Dictionary<int, double[,]> centroidDistance, Dictionary<int,HashSet<double>> clusterClients)
        {
            // stap 1: get klnr list for each centroid
            // stap 2: get prods of avg klnr -> new centroid location X
            // stap 3: get avg prod -> new centroid location Y 
            var customerList = GetCustomers(k, centroidsAvgSSE, centroidDistance, clusterClients);
            var productList = GetProducts(k, customerList, clusterPoints);

            List<CentroidPosition> kList = new List<CentroidPosition>();
            for (int c = 1; c <= k; c++)
            {
                double Y = double.Parse(productList[c].ToString());
                double X = double.Parse( customerList[c].ToString());
                kList.Add(new CentroidPosition(c, X, Y));
                Console.WriteLine("Centroid " + c + " with the location X of " + X + " with the location Y of " + Y);
            }
            return kList;
        }

        public static Dictionary<int, int> GetCentroidsAvgNumber(int k, Dictionary<int, List<double>> List)
        {
            Dictionary<int, int> centroidAvgNumbers = new Dictionary<int, int>();
            for (int c = 1; c <= k; c++)
            {
                var array = List[c].ToArray();
                Array.Sort(array);
                int lenght = array.Length - 1;
                int avgLength = lenght / 2;
                int nr = Int32.Parse(array[avgLength].ToString());
                centroidAvgNumbers.Add(c, nr);
            }
            return centroidAvgNumbers;
        }
        public static Dictionary<int, int> GetProducts(int k, Dictionary<int, int> customerList, Dictionary<int, int[,]> clusterPoints)
        {
            Dictionary<int, List<double>> centroidProducts = new Dictionary<int, List<double>>(); // key = centroid number, list = products
            for (int c = 1; c <= k; c++)
            {
                var matrix = clusterPoints[customerList[c]];
                for (int row = 0; row < matrix.GetLength(0)-1; row++)
                {
                    //  key klnr , productnr-> = {[0,0] , centroidnr --> = [0,1]} 
                    if (matrix[row,1] == c)
                    {
                        if (!centroidProducts.ContainsKey(c))
                        {
                            centroidProducts.Add(c, new List<double>());
                            centroidProducts[c].Add(matrix[row, 0]);
                        }
                        else 
                        {
                            centroidProducts[c].Add(matrix[row, 0]);
                        }
                            
                    }
                   
                }
            }
            return GetCentroidsAvgNumber(k, centroidProducts);
        }

        public static Dictionary<int,int> GetCustomers(int k, Dictionary<int, double> centroidsAvgSSE, Dictionary<int, double[,]> centroidDistance, Dictionary<int, HashSet<double>> clusterClients)
        {
            Dictionary<int, List<double>> centroidCustomers = new Dictionary<int, List<double>>(); // key = centroid number, list = klnr
            for (int c = 1; c <= k; c++)
            {
                var matrix = centroidDistance[c];
                for (int row = 0; row < matrix.GetLength(0) - 1; row++)
                {
                    //kln_nr-> = {[0,0] , distance 

                    var klnr = matrix[row, 0];
                    var distance = matrix[row, 1];
                    var margin = 1.5;

                    var minRange = centroidsAvgSSE[c] - margin;
                    var maxRange = centroidsAvgSSE[c] + margin;

                    if (clusterClients[c].Contains(klnr) && (distance >= minRange && distance <= maxRange))
                    {
                        if (!centroidCustomers.ContainsKey(c))
                        {
                            centroidCustomers.Add(c, new List<double>());
                            centroidCustomers[c].Add(klnr);
                        }
                        else
                        {
                            centroidCustomers[c].Add(klnr);
                        }
                    }
                  
                }
            }

            return GetCentroidsAvgNumber(k, centroidCustomers);
        }

        public static double CompareSSE(double sseNew, double sseOld, int c) // cnr -> 1 
        {
            var sse = sseOld;
            if (sseOld != 0.0)
            {
                if (sseOld > sseNew)
                {
                    sse = sseNew;
                    // cnr => 1 : ne sse < oud sse -> update en relocate centroid position
                    RelocateCentroidsPositions(c,centroidsAvgSSE,centroidDistances, clusterClients);

                }

            }
            return sse;

        }

        public static void UpdateSSE(Dictionary<int, double> centroidsAvgSSE)
        {
            foreach (var centroid in centroidsAvgSSE)
            {
                switch (centroid.Key)
                {
                    case 1:
                        sseAverageCentroidOne = CompareSSE(centroid.Value, sseAverageCentroidOne,1);
                        break;
                    case 2:
                        //do iets
                        sseAverageCentroidTwo = CompareSSE(centroid.Value, sseAverageCentroidTwo,2);
                        break;
                    case 3:
                        //do iets
                        sseAverageCentroidThree = CompareSSE(centroid.Value, sseAverageCentroidThree,3);
                        break;
                    case 4:
                        //do iets
                        sseAverageCentroidFour = CompareSSE(centroid.Value, sseAverageCentroidFour,4);
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

            for (int c = 1; c <= 4; c++)
            {
                avgSSE = 0.0;
                int row = centroidDistance[c].GetLength(0) - 1;
                avgTimes = 0.0;
                for (int i = 0; i < row; i++)// 1 -> klanten
                {
                    if (clusterProducts[c].Contains(centroidDistance[c][i, 0])) // per klnr
                    {
                        avgSSE += centroidDistance[c][i, 1];
                        avgTimes++;
                    }
                }
                centroidsAvgSSE.Add(c, (avgSSE / avgTimes));
            }
            return centroidsAvgSSE;
        }

        public static Dictionary<int, HashSet<double>> GetClusterClients()
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

        public static void CalcCentroidDistance()
        {
            
            foreach (var customer in CsvReader.customersDictionary)
            {
                
                DistanceToCentroid(customer.Value);
            }

        }

        public static void DistanceToCentroid(CustomerInfo customer)
        {

            var pointsDistance = new List<Tuple<int, double>>();
            foreach (var centroid in centroids)
            {
                double distance = Centroid.ComputeDistance(centroid.Value.ToArray(), customer.Points.ToArray());
         
                if (centroidDistances_New.ContainsKey(customer.CustomerId))
                {
                    centroidDistances_New[customer.CustomerId].Add(new Tuple<int, double>(centroid.Key, distance));
                }
                else {
                    pointsDistance.Add(new Tuple<int, double>(centroid.Key, distance));
                    centroidDistances_New.Add(customer.CustomerId, pointsDistance);
                }
                
            }
            

        }
        public static void AssignToCluster()
        {
            ////dictionary of clusters
            //for (int i = 0; i < 101; i++) // loops through kln_nr amount
            //{
            //    // gets i {kln_nr} of centroiddistance dictionary -> then pick the smallest value
            //    double[,] distance = new double[4, 3]; // K{aantal centroids} = 4
            //    int index = 0;
            //    foreach (var centroid in centroidDistance)
            //    {
            //        // Example   (3,50, 2.49)
            //        distance[index, 0] = centroid.Key;
            //        distance[index, 1] = centroid.Value[i, 0]; // klant nummer
            //        distance[index, 2] = centroid.Value[i, 1]; // distance
            //        index++;
            //    }
            //    // method die de 2d array object opslaat in de behoorde cluster 2darray
            //    AssignToClusterExtension(ShortestDistance(distance));
            //    //ShortestDistance( distance)[0] return centroidnummer en klnt -> then add to dictionary
            //}


            foreach (var distance in centroidDistances_New)
            {
                int centroidNumber =  ShortestDistance_New(distance.Value).Item1;
                Centroid.AddPoint(centroidNumber, CsvReader.customersDictionary[distance.Key]);
            }
            //Method Array 
        }

        public static Tuple<int, double> ShortestDistance_New(List<Tuple<int,double>> centroidDistance )
        {
            //TODO FIX SORTING OF ARRAY, pick the right shortest distance to Centroid.
            Tuple<int, double>[] tupleArray = centroidDistance.ToArray();
            Array.Sort(tupleArray);
            Tuple<int, double> distanceCentroid = tupleArray.ToList().First();
            return distanceCentroid;
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
                        klnrArray[i, 0] = int.Parse(offers[i, j] + "");
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
            for (int a = 0; a < data.GetLength(0); a++)
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
