using ClusteringDS01.Model;
using ClusteringDS01.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusteringDS01.Distances
{
    public class Silhouette
    {
        //s(i) = (sep(i) - co(i) )/ (max { co(i), sep(i})
        // KlantId --> <centroid, klantID, afstand>
        //Cohesion afstanden berekenen binnen 1 centroid met elkaar klant --> klant
        public static Dictionary<int, List<Tuple<int, int, double>>> DistancesCohesion = new Dictionary<int, List<Tuple<int, int, double>>>();
        //Seperation klant ---> berekenen buiten zijn eigen centroid berekenen afstanden
        public static Dictionary<int, List<Tuple<int, int, double>>> DistancesSeperation = new Dictionary<int, List<Tuple<int, int, double>>>();

        //public static void CalculateSilhouette(int x, int y)
        //{
        //    foreach (var customer in CsvReader.customersDictionary)
        //    {
        //        var cohesion = AverageCohesion(customer.Key);
        //        var seperation = AverageSeperation(customer.Key);
        //        if (cohesion < seperation)
        //        {
        //            // 1 - (  co(i) / sep(i))  )
        //        }
        //        if (cohesion == seperation)
        //        {
        //            // === 0
        //        }
        //        if (seperation < cohesion)
        //        {
        //            // (sep(i) / co(i) ) - 1
        //        }

        //    }

        //}

        public static Boolean HasDuplicate(int x, int y, int centroid)
        {
            return DistancesCohesion[centroid].Any<Tuple<int, int, double>>(value => (value.Item1 == x && value.Item2 == y) || (value.Item1 == y && value.Item2 == x));
        }

        //afstanden bereken binnen centroid van klant naar klant
        //
        public static void CalculateDistanceCohesion(int customerId, int centroid)
        {
            if (!DistancesCohesion.ContainsKey(centroid))
            {
                DistancesCohesion.Add(centroid, new List<Tuple<int, int, double>>());
            }
            Euclidean euclidean = new Euclidean();
            var cluster = Centroid.sseCentroids[centroid];
            var filterCustomers = cluster.Where(customer => customer.CustomerId != customerId);
            foreach (var customer in filterCustomers)
            {
                if (!HasDuplicate(customerId, customer.CustomerId, centroid))
                {
                    int[] pointX = Centroid.sseCentroids[centroid].Find(c => c.CustomerId == customerId).Offer.ToArray();
                    DistancesCohesion[centroid].Add(new Tuple<int, int, double>(customerId, customer.CustomerId, euclidean.ComputeDistance(pointX, customer.Offer.ToArray())));
                }
            }
        }

        public static double AverageCohesion(int customerId)
        {
            double result = 0.0;
            int centroid = Centroid.GetSSECentroidByCustomerId(customerId);
            var cluster = Centroid.sseCentroids[centroid];
            if (DistancesCohesion.Count == 0)
            {
                foreach (var customer in cluster)
                {
                    CalculateDistanceCohesion(customer.CustomerId, centroid);
                }
                var filterList = DistancesCohesion[centroid].Where(c => c.Item1 == customerId || c.Item2 == customerId).ToList();
                var observationCount = filterList.Count;
                result = filterList.Sum(x => x.Item3) / observationCount;
            }
            else
            {
                var filterList = DistancesCohesion[centroid].Where(c => c.Item1 == customerId || c.Item2 == customerId).ToList();
                var observationCount = filterList.Count;
                result = filterList.Sum(x => x.Item3) / observationCount;
            }
            return result;
        }
    }
}
