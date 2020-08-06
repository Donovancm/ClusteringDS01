using ClusteringDS01.Distances;
using ClusteringDS01.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClusteringDS01.Export
{
    public class ExportExcel
    {
        public static ExcelPackage excelHelper { get; set; }
        public static int k { get; set; }
        public static ExcelWorksheet currWorksheet { get; set; }

        public static void Init()
        {
            k = Centroid.sseCentroids.Count;
            excelHelper = new ExcelPackage();
            currWorksheet = excelHelper.Workbook.Worksheets.Add("K:" + k);
        }

        public static void Export()
        {
            var curDir = Directory.GetCurrentDirectory();
            var rootProjectDir = curDir.Remove(curDir.IndexOf("\\bin\\Debug\\netcoreapp2.2"));

            var memStream = new MemoryStream();
            excelHelper.SaveAs(memStream);
            memStream.Position = 0;
            byte[] bytes = new byte[memStream.Length];
            memStream.Read(bytes, 0, (int)memStream.Length);
            System.IO.File.WriteAllBytes($"{rootProjectDir}\\Output\\" + "K_" + k + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx", bytes);
        }
        public static void CreateClusterWorkSheet()
        {
            currWorksheet.Cells[1, 1].Value = "SSE: " + Centroid.sse;
            foreach (var cluster in Centroid.sseCentroids)
            {
                currWorksheet.Cells[cluster.Key + 3, 1].Value = cluster.Key;
                currWorksheet.Cells[cluster.Key + 3, 1].Style.Font.Bold = true;
                List<CustomerInfo> customerInfos = cluster.Value;
                for (int i = 0; i < customerInfos.Count; i++)
                {
                    currWorksheet.Cells[cluster.Key + 3, i + 3].Value = customerInfos.ElementAt(i).CustomerName;
                }
            }
        }
        public static void CreateSilhouetteWorkSheet()
        {
            currWorksheet = excelHelper.Workbook.Worksheets.Add("Silhouette");
            currWorksheet.Cells[1, 1].Value = "Silhout: " + Silhouette.SilhouetteValues.Average(x => x.Value);
            int rowCount = 3;
            foreach (var customer in Silhouette.SilhouetteValues)
            {
                currWorksheet.Cells[rowCount, 1].Value = customer.Key;
                currWorksheet.Cells[rowCount, 2].Value = customer.Value;
                rowCount++;
            }
        }

        public static void CreateTopDealsWorkSheet()
        {
            currWorksheet = excelHelper.Workbook.Worksheets.Add("TopDeals");
            Dictionary<int, List<Tuple<int, int>>> topDeals = Centroid.GetTopDeals();
            Dictionary<int, int> productSales = new Dictionary<int, int>();
            List<int> topDealsK = new List<int>();

            foreach (var product in topDeals)
            {
                productSales.Add(product.Key, product.Value.Sum(x => x.Item2));
            }

            int position = 1;

            topDealsK = topDeals[1].ConvertAll(t => t.Item1);
            topDealsK.Sort();

            for (int i = 0; i < topDealsK.Count; i++)
            {
                currWorksheet.Cells[1, i + 2].Value = "K" + (topDealsK[i]);
            }

         
            foreach (var product in productSales.OrderByDescending(x => x.Value))
            {
                currWorksheet.Cells[position + 1, 1].Value = "Product: " + product.Key;
                int i = 0;
                var clusterDeals = topDeals[product.Key];
                clusterDeals.Sort();
                foreach (var deal in clusterDeals)
                {
                    currWorksheet.Cells[position + 1, i + 2].Value = deal.Item2;
                    i++;
                }
                position++;
            }
        }
    }
}
