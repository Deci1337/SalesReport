using System;
using System.Collections.Generic;

namespace Model.Core
{
    public partial class Report
    {
        public PricePoint[] GetPriceChanges(string article)
        {
            List<DateTime> dates = new List<DateTime>();
            List<decimal> pricesForDate = new List<decimal>();
            List<int> counts = new List<int>();

            foreach (ITProduct p in Products)
            {
                if (p.Article != article)
                {
                    continue;
                }
                if (p.SaleDate == null)
                {
                    continue;
                }

                DateTime saleDay = p.SaleDate.Value.Date;

                int index = -1;
                for (int i = 0; i < dates.Count; i++)
                {
                    if (dates[i] == saleDay)
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                {
                    dates.Add(saleDay);
                    pricesForDate.Add(p.Price);
                    counts.Add(1);
                }
                else
                {
                    pricesForDate[index] = pricesForDate[index] + p.Price;
                    counts[index] = counts[index] + 1;
                }
            }

            List<PricePoint> result = new List<PricePoint>();
            for (int i = 0; i < dates.Count; i++)
            {
                PricePoint point = new PricePoint();
                point.Date = dates[i];
                point.AvgPrice = pricesForDate[i] / counts[i];
                result.Add(point);
            }

            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = 0; j < result.Count - 1 - i; j++)
                {
                    if (result[j].Date > result[j + 1].Date)
                    {
                        PricePoint temp = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = temp;
                    }
                }
            }

            return result.ToArray();
        }
    }
}
