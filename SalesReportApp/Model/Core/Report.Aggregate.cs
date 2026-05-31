using System;
using System.Collections.Generic;

namespace Model.Core
{
    public partial class Report
    {
        public Report(string name, List<Report> reports, DateTime start, DateTime end)
        {
            Name = name;
            PeriodStart = start;
            PeriodEnd = end;
            Products = new List<ITProduct>();

            for (int i = 0; i < reports.Count; i++)
            {
                List<ITProduct> reportProducts = reports[i].Products;
                for (int j = 0; j < reportProducts.Count; j++)
                {
                    ITProduct product = reportProducts[j];
                    if (product.SaleDate.HasValue)
                    {
                        if (product.SaleDate.Value >= start && product.SaleDate.Value <= end)
                        {
                            AddProduct(product);
                        }
                    }
                }
            }
        }
    }
}
