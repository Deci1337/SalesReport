using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Core
{
    public partial class Report : IReportable, IExportable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<ITProduct> Products { get; private set; }

        public Report()
        {
            Products = new List<ITProduct>();
        }

        public Report(string name, DateTime start, DateTime end, List<ITProduct> products)
        {
            Name = name;
            PeriodStart = start;
            PeriodEnd = end;
            Products = new List<ITProduct>();
            foreach (ITProduct p in products)
            {
                AddProduct(p);
            }
        }

        public void Sort(bool ascending)
        {
            int count = Products.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1 - i; j++)
                {
                    int cmp = string.Compare(Products[j].Article, Products[j + 1].Article);
                    bool needSwap = false;
                    if (ascending && cmp > 0)
                    {
                        needSwap = true;
                    }
                    if (!ascending && cmp < 0)
                    {
                        needSwap = true;
                    }
                    if (needSwap)
                    {
                        ITProduct temp = Products[j];
                        Products[j] = Products[j + 1];
                        Products[j + 1] = temp;
                    }
                }
            }
        }

        public List<ITProduct> Select(Type type)
        {
            List<ITProduct> result = new List<ITProduct>();
            if (type == typeof(ITProduct))
            {
                foreach (ITProduct p in Products)
                {
                    result.Add(p);
                }
                return result;
            }
            foreach (ITProduct p in Products)
            {
                if (p.GetType() == type)
                {
                    result.Add(p);
                }
            }
            return result;
        }

        public string Export()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Отчёт: " + Name);
            sb.AppendLine("Период: " + PeriodStart.ToShortDateString() + " - " + PeriodEnd.ToShortDateString());
            foreach (ITProduct p in Products)
            {
                sb.AppendLine(p.ToString());
            }
            return sb.ToString();
        }

        public void AddProduct(ITProduct p)
        {
            bool exists = false;
            foreach (ITProduct existing in Products)
            {
                if (existing == p)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Products.Add(p);
            }
        }

        public void AddProducts(List<ITProduct> items)
        {
            foreach (ITProduct p in items)
            {
                AddProduct(p);
            }
        }

        public void Merge(Report other)
        {
            foreach (ITProduct p in other.Products)
            {
                AddProduct(p);
            }
        }
    }
}
