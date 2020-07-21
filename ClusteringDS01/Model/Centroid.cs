using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringDS01.Model
{
    public class Centroid
    {
        //Centroidnummer + Coordinaten
        public static Dictionary<int, List<double>> Centroids { get; set; }

        // centroidnumber + customerinfo
        public Dictionary<int, List<CustomerInfo>> Points { get; set; }


        public Centroid(int centroidNumber, List<double> coordinates)
        {
            Centroids.Add(centroidNumber, coordinates);
        }

        public static void Initialize(int k)
        {
            for (int i = 1; i <= k; i++)
            {
                Random rng = new Random();
                double X = rng.NextDouble() * 100;
                double Y = rng.NextDouble() * 31; 
                Centroids.Add(i , new List<double>(){X,Y});
                Console.WriteLine("Centroid " + i + " with the location X of " + X + " with the location Y of " + Y);
            }
        }

        public static void AddPoint(CustomerInfo points) { }
        public static void ClearPointList() { }
        public static void CalculateCentroidPosition() { }
        //maybe some more methods ect
    }
}
