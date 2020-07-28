using ClusteringDS01.Distances;
using ClusteringDS01.Model;
using ClusteringDS01.Reader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
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
            }
            PrintResults();
            ExportResults();
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

        //To Centroid Class
        public static Dictionary<int, List<double>> RelocateCentroidsPositions()
        {
            return Centroid.CalculateCentroidPosition();
        }

        //To Centroid Class
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

        //To Centroid Class
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

        //To Centroid Class
        public static void CalcCentroidDistance()
        {
            
            foreach (var customer in CsvReader.customersDictionary)
            {
                
                DistanceToCentroid(customer.Value);
            }

        }

        //To Centroid Class
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

        //To Centroid Class
        public static void AssignToCluster()
        {
            foreach (var distance in centroidDistances)
            {
                int centroidNumber =  ShortestDistance(distance.Value).Item1;
                Centroid.AddPoint(centroidNumber, CsvReader.customersDictionary[distance.Key]);
            }
        }

        //To Centroid Class
        public static Tuple<int, double> ShortestDistance(List<Tuple<int,double>> centroidDistance)
        {
            Tuple<int, double> distanceCentroid = centroidDistance.OrderBy(x => x.Item2).First();
            return distanceCentroid;
        }

        public static void ExportResults()
        {
            int k = sseCentroids.Count;
            var excelHelper = new ExcelPackage();
            var currWorksheet = excelHelper.Workbook.Worksheets.Add("K:" + k);
            currWorksheet.Cells[1, 1].Value = "SSE: " + sse;

            foreach (var cluster in sseCentroids)
            {
                currWorksheet.Cells[cluster.Key + 3, 1].Value = cluster.Key;
                currWorksheet.Cells[cluster.Key + 3, 1].Style.Font.Bold = true;
                List<CustomerInfo> customerInfos = cluster.Value;
                for (int i = 0; i < customerInfos.Count; i++)
                {
                    currWorksheet.Cells[cluster.Key + 3, i + 3].Value = customerInfos.ElementAt(i).CustomerName;
                }
            }
            var curDir = Directory.GetCurrentDirectory();
            var rootProjectDir = curDir.Remove(curDir.IndexOf("\\bin\\Debug\\netcoreapp2.2"));
            
            var memStream = new MemoryStream();
            excelHelper.SaveAs(memStream);
            memStream.Position = 0;
            byte[] bytes = new byte[memStream.Length];
            memStream.Read(bytes, 0, (int)memStream.Length);
            System.IO.File.WriteAllBytes($"{rootProjectDir}\\Output\\" + "K_" + k + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx", bytes);
        }

    }
}
