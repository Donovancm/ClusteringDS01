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
        // KlantId --> <centroid, klantID, afstand>
        public static Dictionary<int, List<Tuple<int, int, double>>> DistancesSeperation = new Dictionary<int, List<Tuple<int, int, double>>>();

        public static Dictionary<string, double> SilhouetteValues = new Dictionary<string, double>();

        public static void CalculateSilhouette(int customerId)
        {
            double silhouette = 0.0;
            var cohesion = AverageCohesion(customerId);
            var seperation = AverageSeparation(customerId);
            if (cohesion < seperation)
            {
                silhouette = 1 - (cohesion / seperation);
            }
            if (cohesion == seperation)
            {
                // === 0
                silhouette = 0.0;
            }
            if (seperation < cohesion)
            {
                // (sep(i) / co(i) ) - 1
                silhouette = (seperation / cohesion) - 1;
            }
            var customerName = CsvReader.customersDictionary[customerId].CustomerName;
            SilhouetteValues.Add(customerName, silhouette);
        }

        public static Boolean HasDuplicate(int x, int y, int centroid)
        {
            return DistancesCohesion[centroid].Any<Tuple<int, int, double>>(value => (value.Item1 == x && value.Item2 == y) || (value.Item1 == y && value.Item2 == x));
        }

        public static Boolean HasDuplicate2(int x, int y)
        {
            if (DistancesSeperation.ContainsKey(x) && DistancesSeperation[x].Any<Tuple<int, int, double>>(value => (value.Item2 == y)))
            {
                return true;
            }
            else if (DistancesSeperation.ContainsKey(y) && DistancesSeperation[y].Any<Tuple<int, int, double>>(value => (value.Item2 == x)))
            {
                return true;
            }
            return false;
        }

        public static void CalculateDistanceSeperation(int customerId)
        {
            if (!DistancesSeperation.ContainsKey(customerId))
            {
                DistancesSeperation.Add(customerId, new List<Tuple<int, int, double>>());
            }
            Euclidean euclidean = new Euclidean();
            var customerCentroid = Centroid.GetSSECentroidByCustomerId(customerId);
            foreach (var cluster in Centroid.sseCentroids)
            {
                if (cluster.Key != customerCentroid)
                {
                    foreach (var customer in cluster.Value)
                    {
                        if (!HasDuplicate2(customerId, customer.CustomerId))
                        {
                            int[] pointX = Centroid.sseCentroids[customerCentroid].Find(c => c.CustomerId == customerId).Offer.ToArray();
                            DistancesSeperation[customerId].Add(new Tuple<int, int, double>(cluster.Key, customer.CustomerId, euclidean.ComputeDistance(pointX, customer.Offer.ToArray())));
                        }
                    }
                }

            }

        }

        public static double AverageSeparation(int customerId)
        {
            var output = 0.0;
            // int centroid = Centroid.GetSSECentroidByCustomerId(customerId);
            Dictionary<int, double> interCluster = new Dictionary<int, double>();
            //loop combinaties van key and value waar id. -->  concat to list

            List<int> listCentroid = new List<int>();
            foreach (var customer in DistancesSeperation)
            {
                if (customer.Key == customerId)
                {
                    listCentroid.AddRange(customer.Value.Select(value => value.Item1));
                }
                else
                {
                    var filterByCustomerId = customer.Value.Where(value => value.Item2 == customerId);
                    if(filterByCustomerId.Count() != 0)
                    {
                        listCentroid.AddRange(customer.Value.Select(value => value.Item1));
                    }
                }
            }
            listCentroid = listCentroid.Distinct().ToList();
            foreach (var centroidNumber in listCentroid)
            {
                var list = DistancesSeperation[customerId].Where(x => x.Item1 == centroidNumber).ToList();
                var observationCount = list.Count;
                double result = list.Sum(x => x.Item3) / observationCount;
                interCluster.Add(centroidNumber, result);
            }
            interCluster.OrderByDescending(x => x.Value);
            output = interCluster.First().Value;
            return output;
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
            var filterList = DistancesCohesion[centroid].Where(c => c.Item1 == customerId || c.Item2 == customerId).ToList();
            var observationCount = filterList.Count;
            result = filterList.Sum(x => x.Item3) / observationCount;
            return result;
        }

        public static void Init()
        {
            foreach (var cluster in Centroid.sseCentroids)
            {
                foreach (var customer in cluster.Value)
                {
                    CalculateDistanceCohesion(customer.CustomerId, cluster.Key);
                    CalculateDistanceSeperation(customer.CustomerId);
                }
            }

        }
    }
}
