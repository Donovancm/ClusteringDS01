using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringDS01.Distances
{
    public class Euclidean : IDistance
    {
        public double ComputeDistance(double[] X, double[] Y)
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
    }
}
