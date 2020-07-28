using ClusteringDS01.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClusteringDS01.Reader
{
 
    public class CsvReader
    {
        public static Dictionary<int, CustomerInfo> customersDictionary { get; set; }
        public static void GetData()
        {
            SetupOffers();
        }


        public static void SetupOffers()
        {
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader("../../../Data/Winecraft.csv"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to clusterPoints.
                }
            }
            int users = 101;
            customersDictionary = Points2Dictionary(list, users);


        }

        public static Dictionary<int, CustomerInfo> Points2Dictionary(List<string> list, int usersColumn)
        {
            Dictionary<int, CustomerInfo> customers = new Dictionary<int, CustomerInfo>();
            for (int i = 1; i < usersColumn; i++)
            {
                int row = 1;
                CustomerInfo customerInfo = new CustomerInfo();
                List<int> points = new List<int>();
                foreach (var item in list)
                {
                    string[] userinfo = item.Split(",");
                    if (row == 1) { customerInfo.CustomerId = i; customerInfo.CustomerName = userinfo[i]; }
                    if (row > 1)
                    {
                        if (userinfo[i] == "")
                        {
                            points.Add(0);
                        }
                        else
                        {
                            points.Add(int.Parse(userinfo[i]));
                        }
                        
                    }
                    row++;
                }
                customerInfo.Offer = points;
                customers.Add(customerInfo.CustomerId, customerInfo);
            }
            return customers;

        }
    }
}
