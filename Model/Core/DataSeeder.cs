using System;
using System.Collections.Generic;

namespace Model.Core
{
    public static class DataSeeder
    {
        public static List<Report> Initialize()
        {
            List<ITProduct> products = CreateProducts();
            List<Report> reports = CreateReports(products);
            return reports;
        }

        private static List<ITProduct> Filter<T>(List<ITProduct> list) where T : ITProduct
        {
            List<ITProduct> result = new List<ITProduct>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is T)
                {
                    ITProduct product = list[i];
                    result.Add(product);
                }
            }
            return result;
        }

        private static DateTime RandomDate(Random rnd, int year, int month)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int day = rnd.Next(1, daysInMonth + 1);
            return new DateTime(year, month, day);
        }

        private static List<ITProduct> CreateProducts()
        {
            Random rnd = new Random(42);
            List<ITProduct> products = new List<ITProduct>();
            int id = 1;

            // Ноутбуки — 3 модели, каждая продаётся в марте, апреле и мае
            string[] laptopArticles = { "LT-001", "LT-002", "LT-003" };
            string[] laptopBrands   = { "HONOR",   "Apple",           "ASUS"          };
            string[] laptopModels   = { "X16 Plus", "MacBook Air M4", "ROG Zephyrus"  };
            decimal[] laptopBase    = { 65000m,    110000m,           130000m         };
            int[] laptopRam         = { 16, 16, 32  };
            int[] laptopStorage     = { 512, 512, 1024 };

            for (int m = 0; m < laptopArticles.Length; m++)
            {
                for (int month = 3; month <= 5; month++)
                {
                    for (int sale = 0; sale < 3; sale++)
                    {
                        Laptop l = new Laptop();
                        l.Id = id++;
                        l.Article = laptopArticles[m];
                        l.Brand = laptopBrands[m];
                        l.ModelName = laptopModels[m];
                        l.RamGb = laptopRam[m];
                        l.StorageGb = laptopStorage[m];
                        l.BasePrice = laptopBase[m] + rnd.Next(-3000, 3001);
                        l.SaleDate = RandomDate(rnd, 2026, month);
                        products.Add(l);
                    }
                }
            }

            // Смартфоны — 3 модели, каждая продаётся в марте, апреле и мае
            string[] phoneArticles = { "SM-001", "SM-002", "SM-003" };
            string[] phoneBrands   = { "Apple",          "Samsung",              "Xiaomi"           };
            string[] phoneModels   = { "iPhone 17 Pro Max", "Galaxy S25 Ultra", "Redmi 17 Pro"     };
            decimal[] phoneBase    = { 120000m,          95000m,                 35000m             };
            int[] phoneCam         = { 48, 200, 108 };
            int[] phoneBat         = { 4500, 5000, 5500 };

            for (int m = 0; m < phoneArticles.Length; m++)
            {
                for (int month = 3; month <= 5; month++)
                {
                    for (int sale = 0; sale < 3; sale++)
                    {
                        Smartphone s = new Smartphone();
                        s.Id = id++;
                        s.Article = phoneArticles[m];
                        s.Brand = phoneBrands[m];
                        s.ModelName = phoneModels[m];
                        s.CameraMP = phoneCam[m];
                        s.BatteryMah = phoneBat[m];
                        s.BasePrice = phoneBase[m] + rnd.Next(-2000, 2001);
                        s.SaleDate = RandomDate(rnd, 2026, month);
                        products.Add(s);
                    }
                }
            }

            // Планшеты — 3 модели, каждая продаётся в марте, апреле и мае
            string[] tabArticles = { "TB-001", "TB-002", "TB-003" };
            string[] tabBrands   = { "Apple", "Samsung", "Lenovo" };
            string[] tabModels   = { "iPad 10", "Galaxy Tab A9", "Tab P12" };
            decimal[] tabBase    = { 50000m, 25000m, 30000m };
            double[] tabScreen   = { 10.9, 8.7, 12.7 };
            bool[] tabLte        = { false, true, false };

            for (int m = 0; m < tabArticles.Length; m++)
            {
                for (int month = 3; month <= 5; month++)
                {
                    for (int sale = 0; sale < 3; sale++)
                    {
                        Tablet t = new Tablet();
                        t.Id = id++;
                        t.Article = tabArticles[m];
                        t.Brand = tabBrands[m];
                        t.ModelName = tabModels[m];
                        t.ScreenInch = tabScreen[m];
                        t.HasLTE = tabLte[m];
                        t.BasePrice = tabBase[m] + rnd.Next(-2500, 2501);
                        t.SaleDate = RandomDate(rnd, 2026, month);
                        products.Add(t);
                    }
                }
            }

            return products;
        }

        private static List<Report> CreateReports(List<ITProduct> products)
        {
            List<Report> reports = new List<Report>();

            Predicate<ITProduct> soldInMarch = delegate(ITProduct p)
            {
                return p.SaleDate.HasValue && p.SaleDate.Value.Month == 3;
            };
            Predicate<ITProduct> soldInApril = delegate(ITProduct p)
            {
                return p.SaleDate.HasValue && p.SaleDate.Value.Month == 4;
            };
            Predicate<ITProduct> soldInMay = delegate(ITProduct p)
            {
                return p.SaleDate.HasValue && p.SaleDate.Value.Month == 5;
            };

            List<ITProduct> laptops    = Filter<Laptop>(products);
            List<ITProduct> smartphones = Filter<Smartphone>(products);
            List<ITProduct> tablets    = Filter<Tablet>(products);

            int reportId = 1;

            reportId = AddMonthReport(reports, laptops,     soldInMarch, "Ноутбуки Март",      3, reportId);
            reportId = AddMonthReport(reports, smartphones,  soldInMarch, "Смартфоны Март",     3, reportId);
            reportId = AddMonthReport(reports, tablets,      soldInMarch, "Планшеты Март",      3, reportId);
            reportId = AddMonthReport(reports, laptops,     soldInApril, "Ноутбуки Апрель",    4, reportId);
            reportId = AddMonthReport(reports, smartphones,  soldInApril, "Смартфоны Апрель",  4, reportId);
            reportId = AddMonthReport(reports, tablets,      soldInApril, "Планшеты Апрель",   4, reportId);
            reportId = AddMonthReport(reports, laptops,     soldInMay,   "Ноутбуки Май",       5, reportId);
            reportId = AddMonthReport(reports, smartphones,  soldInMay,   "Смартфоны Май",      5, reportId);
            reportId = AddMonthReport(reports, tablets,      soldInMay,   "Планшеты Май",       5, reportId);

            return reports;
        }

        private static int AddMonthReport(List<Report> reports, List<ITProduct> source,
            Predicate<ITProduct> filter, string name, int month, int reportId)
        {
            List<ITProduct> items = new List<ITProduct>();
            for (int i = 0; i < source.Count; i++)
            {
                if (filter(source[i]))
                {
                    items.Add(source[i]);
                }
            }
            DateTime start = new DateTime(2026, month, 1);
            DateTime end = new DateTime(2026, month, DateTime.DaysInMonth(2026, month));
            Report r = new Report(name, start, end, items);
            r.Id = reportId;
            reports.Add(r);
            return reportId + 1;
        }
    }
}
