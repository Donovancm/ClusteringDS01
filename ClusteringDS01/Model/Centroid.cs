using ClusteringDS01.Reader;
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
        public static Dictionary<int, List<CustomerInfo>> Points { get; set; }

        
        public Centroid() { }

        public static Dictionary<int, List<double>> Initialize(int k)
        {
            Centroids = new Dictionary<int, List<double>>();
            for (int i = 1; i <= k; i++)
            {
                Centroids.Add(i , GenerateCentroidPosition());
            }
            return Centroids;

        }

        public static List<double> GenerateCentroidPosition()
        {
            List<double> centroidposition = new List<double>();
            Random rng = new Random();
            for (int i = 1; i <= 32; i++)
            {
                double randomNumber = rng.NextDouble() * (1.0 - 0.0) + 0.0;
                centroidposition.Add(randomNumber);
            }
            return centroidposition;
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

        public static Dictionary<int, List<double>> CalculateCentroidPosition()
        {
            foreach (var cluster in Points)
            {
                List<CustomerInfo> customers = cluster.Value;
                List<double> positions = new List<double>();
                for (int i = 0; i < 32; i++)
                {
                    double totalOfferPoints = 0.0; // todo andere naam
                    foreach (var customer in customers)
                    {
                        totalOfferPoints += customer.Offer.ToArray()[i];
                    }
                    positions.Add(totalOfferPoints / customers.Count);
                }
                Centroids[cluster.Key] = positions;
            }

            return Centroids;
        }

        public static double ComputeDistance( double[] X, int[] Y)
        {
            double distance = 0.0;
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
