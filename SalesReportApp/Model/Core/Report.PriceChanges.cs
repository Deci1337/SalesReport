using System;
using System.Collections.Generic;

namespace Model.Core
{
    public partial class Report
    {
        public PricePoint[] GetPriceChanges(string article)
        {
            List<ITProduct> matched = new List<ITProduct>();
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Article == article && Products[i].SaleDate.HasValue)
                {
                    matched.Add(Products[i]);
                }
            }

            List<DateTime> dates = new List<DateTime>();
            for (int i = 0; i < matched.Count; i++)
            {
                DateTime date = matched[i].SaleDate.Value.Date;
                bool found = false;
                for (int j = 0; j < dates.Count; j++)
                {
                    if (dates[j] == date)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dates.Add(date);
                }
            }

            for (int i = 0; i < dates.Count - 1; i++)
            {
                for (int j = 0; j < dates.Count - 1 - i; j++)
                {
                    if (dates[j] > dates[j + 1])
                    {
                        DateTime temp = dates[j];
                        dates[j] = dates[j + 1];
                        dates[j + 1] = temp;
                    }
                }
            }

            PricePoint[] result = new PricePoint[dates.Count];
            for (int i = 0; i < dates.Count; i++)
            {
                decimal sum = 0;
                int count = 0;
                for (int j = 0; j < matched.Count; j++)
                {
                    if (matched[j].SaleDate.Value.Date == dates[i])
                    {
                        sum += matched[j].Price;
                        count++;
                    }
                }
                PricePoint point = new PricePoint();
                point.Date = dates[i];
                point.AvgPrice = sum / count;
                result[i] = point;
            }

            return result;
        }
    }
}
