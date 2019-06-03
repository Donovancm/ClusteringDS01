using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringDS01.Model
{
    class CentroidPosition
    {
        public int number;
        public double X;
        public double Y;

        public CentroidPosition(int number, double x, double y)
        {
            this.number = number;
            X = x;
            Y = y;
        }
    }
}
