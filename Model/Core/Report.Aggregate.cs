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

            foreach (Report report in reports)
            {
                foreach (ITProduct p in report.Products)
                {
                    if (p.SaleDate == null)
                    {
                        continue;
                    }
                    if (p.SaleDate.Value < start || p.SaleDate.Value > end)
                    {
                        continue;
                    }
                    AddProduct(p);
                }
            }
        }
    }
}
