using System;
using System.Collections.Generic;
using System.Text;

namespace ClusteringDS01.Model
{
    // cluster punt class naam veranderen
    public class CustomerInfo
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<int> Offer { get; set; } // klanten col is van 32 offerte verkoop id

        public CustomerInfo(int customerNumber, string customerName ,List<int> points )
        {
            CustomerId = customerNumber;
            CustomerName = customerName;
            Offer = points;
        }

        public CustomerInfo() { }
    }
}
