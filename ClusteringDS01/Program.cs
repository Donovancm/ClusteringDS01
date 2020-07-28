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
        public static int numberOfCentroids;
        public static int numberOfIterations;

        public static Dictionary<int, List<double>> centroids { get; set; }
        public static Dictionary<int, List<Tuple<int,double>>> centroidDistances { get; set; }
        public static double sse = 0.0;
        public static Dictionary<int, List<CustomerInfo>> sseCentroids { get; set; }

        static void Main(string[] args)
        {
            UserChoices();
            Init();

        }
        public static void UserChoices()
        {
            //Reminder weergeven lijst van beschikbare personen
            //Kies eerste persoon en daarna kies 2de persoon
            Console.WriteLine("Set the amount of Centroids (k)");
            numberOfCentroids = int.Parse( Console.ReadLine().ToString());
            Console.WriteLine("Set the amount of Iterations");
            numberOfIterations = int.Parse(Console.ReadLine().ToString());

        }

        public static void Init()
        {
            // init variables
            CsvReader.GetData();
            
           

            // set initial K -> K = 4 as centroids with X & Y position
            int k = numberOfCentroids;

            for (int i = 0; i < numberOfIterations; i++)
            {
              
                if (i == 0)
                {

                    //NEW: place K Randomly at first time.
                    centroids = Centroid.Initialize(k);

                }


                //eucl distance from centroid to offers / items
                centroidDistances = new Dictionary<int, List<Tuple<int, double>>>();
                CalcCentroidDistance();

                // step: 3 | after calc distances add data to cluster dictionary {klnr prodnr centroidnr}| 
                if( i > 0) { Centroid.ClearPointList(); }
                AssignToCluster();

                // step: 4 | vergelijk oude sse met nieuw Sse
                UpdateSSE();

                if (i > 0)
                {
                    // stap 5: | relocate centroids
                    centroids = RelocateCentroidsPositions();
                }
                //Deepcopy
                //Controleren of 
                //
            }
            PrintResults();
            Console.ReadLine();
        }

        public static void PrintResults()
        {
            Console.WriteLine("SSE: " + sse);
            SortedDictionary<int, List<CustomerInfo>> sortedData = new SortedDictionary<int, List<CustomerInfo>>(sseCentroids);
            foreach (var cluster in sortedData)
            {
                Console.WriteLine("K: " + cluster.Key + "\t");
                List<CustomerInfo> customers = cluster.Value;
                foreach (var customer in customers)
                {
                    Console.Write( customer.CustomerName + ", ");
                }
                Console.WriteLine();
            }
        }
        public static Dictionary<int, List<double>> RelocateCentroidsPositions()
        {
            return Centroid.CalculateCentroidPosition();
        }

        public static void UpdateSSE()
        {
            double newSse = CalcAverageSSECentroids();

            if(sse != 0.0)
            {
                if (sse > newSse)
                {
                    sse = newSse;
                }
                sseCentroids = Centroid.Points;
            }
            else { sse = newSse; sseCentroids = Centroid.Points; }
            
        }

        public static double CalcAverageSSECentroids()
        {
            Dictionary<int, double> centroidsAvgSSE = new Dictionary<int, double>();
            double sseAverage = 0.0;
            foreach (var cluster in Centroid.Points)
            {
                List<CustomerInfo> customers = cluster.Value;
                double totalDistance = 0.0;
                foreach (var customer in customers)
                {
                    totalDistance += Centroid.ComputeDistance(Centroid.Centroids[cluster.Key].ToArray(), customer.Offer.ToArray());
                }
                sseAverage += (totalDistance / customers.Count);
            }
            return sseAverage;
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
                double distance = Centroid.ComputeDistance(centroid.Value.ToArray(), customer.Offer.ToArray());
         
                if (centroidDistances.ContainsKey(customer.CustomerId))
                {
                    centroidDistances[customer.CustomerId].Add(new Tuple<int, double>(centroid.Key, distance));
                }
                else {
                    pointsDistance.Add(new Tuple<int, double>(centroid.Key, distance));
                    centroidDistances.Add(customer.CustomerId, pointsDistance);
                }
                
            }
            

        }
        public static void AssignToCluster()
        {
            foreach (var distance in centroidDistances)
            {
                int centroidNumber =  ShortestDistance_New(distance.Value).Item1;
                Centroid.AddPoint(centroidNumber, CsvReader.customersDictionary[distance.Key]);
            }
        }

        public static Tuple<int, double> ShortestDistance_New(List<Tuple<int,double>> centroidDistance)
        {
            Tuple<int, double> distanceCentroid = centroidDistance.OrderBy(x => x.Item2).First();
            return distanceCentroid;
        }
    
    }
}
