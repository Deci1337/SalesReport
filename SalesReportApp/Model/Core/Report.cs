using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Model.Core
{
    [Serializable]
    public partial class Report : IReportable, IExportable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public List<ITProduct> Products { get; set; }

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
            for (int i = 0; i < products.Count; i++)
            {
                Products.Add(products[i]);
            }
        }

        public void Sort(bool ascending)
        {
            for (int i = 0; i < Products.Count - 1; i++)
            {
                for (int j = 0; j < Products.Count - 1 - i; j++)
                {
                    bool shouldSwap = false;
                    if (ascending)
                    {
                        shouldSwap = string.Compare(Products[j].Article, Products[j + 1].Article) > 0;
                    }
                    else
                    {
                        shouldSwap = string.Compare(Products[j].Article, Products[j + 1].Article) < 0;
                    }
                    if (shouldSwap)
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
                for (int i = 0; i < Products.Count; i++)
                {
                    ITProduct product = Products[i];
                    result.Add(product);
                }
                return result;
            }
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].GetType() == type)
                {
                    ITProduct product = Products[i];
                    result.Add(product);
                }
            }
            return result;
        }

        public string Export()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Отчёт: " + Name);
            sb.AppendLine("Период: " + PeriodStart.ToShortDateString() + " - " + PeriodEnd.ToShortDateString());
            sb.AppendLine("Устройства:");
            for (int i = 0; i < Products.Count; i++)
            {
                ITProduct product = Products[i];
                sb.AppendLine("  " + product.Article + " | " + product.Brand
                    + " | " + product.ModelName + " | " + product.Price + " руб.");
            }
            return sb.ToString();
        }

        public void AddProduct(ITProduct p)
        {
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Id == p.Id)
                {
                    return;
                }
            }
            Products.Add(p);
        }

        public void AddProducts(List<ITProduct> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                AddProduct(items[i]);
            }
        }

        public void Merge(Report other)
        {
            for (int i = 0; i < other.Products.Count; i++)
            {
                AddProduct(other.Products[i]);
            }
        }
    }
}
