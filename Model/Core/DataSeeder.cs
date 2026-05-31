using System;
using System.Collections.Generic;

namespace Model.Core
{
    public class DataSeeder
    {
        private static Report CreateReport(string name, DateTime start, DateTime end, List<ITProduct> all, int[] indexes)
        {
            List<ITProduct> products = new List<ITProduct>();
            for (int i = 0; i < indexes.Length; i++)
            {
                products.Add(all[indexes[i]]);
            }
            return new Report(name, start, end, products);
        }

        public static List<T> Filter<T>(List<T> list, Predicate<T> pred) where T : ITProduct
        {
            List<T> result = new List<T>();
            foreach (T item in list)
            {
                if (pred(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static List<ITProduct> CreateProducts()
        {
            List<ITProduct> all = new List<ITProduct>();

            all.Add(new Laptop(1,  "LT-001", "Asus",   "VivoBook 15",  45000, new DateTime(2024, 1, 10), 8,  512));
            all.Add(new Laptop(2,  "LT-002", "Lenovo", "IdeaPad 3",    38000, new DateTime(2024, 1, 15), 16, 256));
            all.Add(new Laptop(3,  "LT-003", "HP",     "Pavilion 14",  52000, new DateTime(2024, 2, 5),  8,  1024));
            all.Add(new Laptop(4,  "LT-004", "Dell",   "Inspiron 15",  60000, null,                      32, 512));
            all.Add(new Laptop(5,  "LT-005", "Acer",   "Aspire 5",     35000, new DateTime(2024, 3, 20), 8,  256));

            all.Add(new Smartphone(6,  "SM-001", "Samsung", "Galaxy A54",    25000, new DateTime(2024, 1, 12), 50,  5000));
            all.Add(new Smartphone(7,  "SM-002", "Xiaomi",  "Redmi Note 12", 18000, new DateTime(2024, 1, 20), 48,  5000));
            all.Add(new Smartphone(8,  "SM-003", "Apple",   "iPhone 14",     70000, new DateTime(2024, 2, 14), 12,  3279));
            all.Add(new Smartphone(9,  "SM-004", "Realme",  "11 Pro",        22000, null,                      108, 5000));
            all.Add(new Smartphone(10, "SM-005", "Honor",   "90 Lite",       16000, new DateTime(2024, 3, 5),  100, 4500));
            all.Add(new Smartphone(11, "SM-006", "Poco",    "X5 Pro",        20000, new DateTime(2024, 3, 18), 64,  5000));

            all.Add(new Tablet(12, "TB-001", "Samsung", "Galaxy Tab A8", 22000, new DateTime(2024, 1, 25), 10.5,  true));
            all.Add(new Tablet(13, "TB-002", "Lenovo",  "Tab M10",       18000, new DateTime(2024, 2, 8),  10.1,  false));
            all.Add(new Tablet(14, "TB-003", "Apple",   "iPad 10",       55000, null,                      10.9,  true));
            all.Add(new Tablet(15, "TB-004", "Huawei",  "MatePad 11",    32000, new DateTime(2024, 3, 11), 10.95, false));

            return all;
        }

        public static List<Report> CreateReports()
        {
            List<ITProduct> all = CreateProducts();
            List<Report> reports = new List<Report>();

            reports.Add(CreateReport("Январь — неделя 1",          new DateTime(2024, 1, 1),  new DateTime(2024, 1, 14), all, new int[] { 0, 5, 11 }));
            reports.Add(CreateReport("Январь — неделя 2",          new DateTime(2024, 1, 15), new DateTime(2024, 1, 31), all, new int[] { 1, 6, 12 }));
            reports.Add(CreateReport("Февраль",                    new DateTime(2024, 2, 1),  new DateTime(2024, 2, 29), all, new int[] { 2, 7, 13 }));
            reports.Add(CreateReport("Март — планшеты и остатки",  new DateTime(2024, 3, 1),  new DateTime(2024, 3, 31), all, new int[] { 14, 8, 3 }));

            List<Laptop> allLaptops = new List<Laptop>();
            foreach (ITProduct p in all)
            {
                if (p is Laptop)
                {
                    allLaptops.Add((Laptop)p);
                }
            }

            List<Laptop> cheapLaptops = Filter<Laptop>(allLaptops, delegate (Laptop l)
            {
                if (l.BasePrice < 50000)
                {
                    return true;
                }
                return false;
            });

            List<ITProduct> marchProducts = new List<ITProduct>();
            foreach (Laptop l in cheapLaptops)
            {
                if (l.Article == "LT-005")
                {
                    marchProducts.Add(l);
                }
            }
            marchProducts.Add(all[9]);
            marchProducts.Add(all[10]);
            reports.Add(new Report("Март — смартфоны и ноутбуки", new DateTime(2024, 3, 1), new DateTime(2024, 3, 31), marchProducts));

            Func<ITProduct, bool> hasSaleDate = delegate (ITProduct p)
            {
                if (p.SaleDate != null)
                {
                    return true;
                }
                return false;
            };

            List<ITProduct> soldProducts = new List<ITProduct>();
            foreach (ITProduct p in all)
            {
                if (hasSaleDate(p))
                {
                    soldProducts.Add(p);
                }
            }
            reports.Add(new Report("Все продажи Q1 2024", new DateTime(2024, 1, 1), new DateTime(2024, 3, 31), soldProducts));

            return reports;
        }
    }
}
