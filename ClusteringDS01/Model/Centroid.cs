using ClusteringDS01.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusteringDS01.Model
{
    public class Centroid
    {
        //Centroidnummer + Coordinaten
        public static Dictionary<int, List<double>> Centroids { get; set; }

        // centroidnumber + customerinfo
        public static Dictionary<int, List<CustomerInfo>> Points { get; set; }

        public static double sse = 0.0;
        public static Dictionary<int, List<CustomerInfo>> sseCentroids { get; set; }

        public static Dictionary<int, List<Tuple<int, double>>> centroidDistances { get; set; }


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

        public static void ClearPointList()
        {
            Points.Clear();
        }

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

        public static void UpdateSSE()
        {
            double newSse = CalcAverageSSECentroids();

            if (sse != 0.0)
            {
                if (sse > newSse)
                {
                    sse = newSse;
                }
                sseCentroids = Points;
            }
            else { sse = newSse; sseCentroids = Points; }

        }

        //To Centroid Class
        public static double CalcAverageSSECentroids()
        {
            Dictionary<int, double> centroidsAvgSSE = new Dictionary<int, double>();
            double sseAverage = 0.0;
            foreach (var cluster in Points)
            {
                List<CustomerInfo> customers = cluster.Value;
                double totalDistance = 0.0;
                foreach (var customer in customers)
                {
                    totalDistance += ComputeDistance(Centroids[cluster.Key].ToArray(), customer.Offer.ToArray());
                }
                sseAverage += (totalDistance / customers.Count);
            }
            return sseAverage;
        }

        public static void CalcCentroidDistance()
        {
            centroidDistances = new Dictionary<int, List<Tuple<int, double>>>();
            foreach (var customer in CsvReader.customersDictionary)
            {
                DistanceToCentroid(customer.Value);
            }

        }

        public static void DistanceToCentroid(CustomerInfo customer)
        {

            var pointsDistance = new List<Tuple<int, double>>();
            foreach (var centroid in Centroids)
            {
                double distance = ComputeDistance(centroid.Value.ToArray(), customer.Offer.ToArray());

                if (centroidDistances.ContainsKey(customer.CustomerId))
                {
                    centroidDistances[customer.CustomerId].Add(new Tuple<int, double>(centroid.Key, distance));
                }
                else
                {
                    pointsDistance.Add(new Tuple<int, double>(centroid.Key, distance));
                    centroidDistances.Add(customer.CustomerId, pointsDistance);
                }

            }


        }

        public static void AssignToCluster()
        {
            foreach (var distance in centroidDistances)
            {
                int centroidNumber = ShortestDistance(distance.Value).Item1;
                AddPoint(centroidNumber, CsvReader.customersDictionary[distance.Key]);
            }
        }
        public static Tuple<int, double> ShortestDistance(List<Tuple<int, double>> centroidDistance)
        {
            Tuple<int, double> distanceCentroid = centroidDistance.OrderBy(x => x.Item2).First();
            return distanceCentroid;
        }

        //Topdeals
        //Dictonary van Int = offerteid, List van Tuple < int<centroid),  int (count van offertes) >
        public static Dictionary<int, List<Tuple<int,int>>> GetTopDeals()
        {
            //per row(32) bekijken van de cluster(centroid) sum offerte per offerte id met row number
            //
            Dictionary<int, List<Tuple<int, int>>> topDeals = new Dictionary<int, List<Tuple<int, int>>>();
            for (int offerID = 1; offerID <= 32; offerID++)
            {
                foreach (var cluster in Points)
                {
                    List<CustomerInfo> customers = cluster.Value;
                    int sum = 0;
                    foreach (var customer in customers)
                    {
                        sum += customer.Offer.ElementAt(offerID-1);
                    }
                    if (topDeals.ContainsKey(offerID))
                    {
                        topDeals[offerID].Add( new Tuple<int, int>( cluster.Key, sum ));
                    }
                    else { topDeals.Add(offerID, new List<Tuple<int, int>> { new Tuple<int, int>(cluster.Key, sum) }); }
                }
            }
            return topDeals;
        }

        public static  int GetSSECentroidByCustomerId(int id)
        {
           return sseCentroids.FirstOrDefault(cluster => cluster.Value.Find(x => x.CustomerId == id).CustomerId == id).Key;
        }
    }
}
