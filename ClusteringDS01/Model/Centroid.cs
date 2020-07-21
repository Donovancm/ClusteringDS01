using ClusteringDS01.Reader;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringDS01.Model
{
    public class Centroid
    {
        //Centroidnummer + Coordinaten
        public static Dictionary<int, List<int>> Centroids { get; set; }

        // centroidnumber + customerinfo
        public static Dictionary<int, List<CustomerInfo>> Points { get; set; }

        
        public Centroid() { }

        public static Dictionary<int, List<int>> Initialize(int k)
        {
            Centroids = new Dictionary<int, List<int>>();
            for (int i = 1; i <= k; i++)
            {
                Random rng = new Random();
                int random = rng.Next(1, 101);
                Centroids.Add(i , CsvReader.customersDictionary[random].Points);
            }
            return Centroids;

        }

        public static void AddPoint(int centroidNumber, CustomerInfo customer)
        {
            if (Points is null)
            {
                Points = new Dictionary<int, List<CustomerInfo>>();
            }
            if (Points.ContainsKey(centroidNumber))
            {
                Points[centroidNumber].Add(customer);
            }
            else { Points.Add(centroidNumber, new List<CustomerInfo>() { customer }); }

        }

        public static void ClearPointList() { }
        public static void CalculateCentroidPosition() { }

        public static double ComputeDistance( int[] X, int[] Y)
        {
            double distance = 0.0;
            //d(p,q) = d(q,p) = 
            int row2DArrayX = X.Length;
            int row2DArrayY = Y.Length;
            for (int i = 0; i < row2DArrayX; i++)
            {
                distance += Math.Pow((X[i] - Y[i]), 2);
            }
            var result = Math.Sqrt(distance);
            return result;
        }
        //maybe some more methods ect
    }
}
