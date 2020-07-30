using ClusteringDS01.Distances;
using ClusteringDS01.Export;
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
                Centroid.CalcCentroidDistance();

                // step: 3 | after calc distances add data to cluster dictionary {klnr prodnr centroidnr}| 
                if( i > 0) { Centroid.ClearPointList(); }
                Centroid.AssignToCluster();

                // step: 4 | vergelijk oude sse met nieuw Sse
                Centroid.UpdateSSE();

                if (i > 0)
                {
                    // stap 5: | relocate centroids
                    centroids = Centroid.CalculateCentroidPosition();
                }
            }
            PrintResults();
            ExportExcel.Init();
            ExportExcel.CreateClusterWorkSheet();
            ExportExcel.CreateTopDealsWorkSheet();
            ExportExcel.Export();
            Console.ReadLine();
        }

        public static void PrintResults()
        {
            Console.WriteLine("SSE: " + Centroid.sse);
            SortedDictionary<int, List<CustomerInfo>> sortedData = new SortedDictionary<int, List<CustomerInfo>>(Centroid.sseCentroids);
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

    }
}
