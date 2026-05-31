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

        private static List<ITProduct> CreateProducts()
        {
            List<ITProduct> products = new List<ITProduct>();

            Laptop l1 = new Laptop();
            l1.Id = 1; l1.Article = "LT-001"; l1.Brand = "Asus"; l1.ModelName = "VivoBook 15";
            l1.BasePrice = 30000; l1.RamGb = 8; l1.StorageGb = 512;
            l1.SaleDate = new DateTime(2025, 3, 5);
            products.Add(l1);

            Laptop l2 = new Laptop();
            l2.Id = 2; l2.Article = "LT-002"; l2.Brand = "Lenovo"; l2.ModelName = "IdeaPad 5";
            l2.BasePrice = 35000; l2.RamGb = 16; l2.StorageGb = 512;
            l2.SaleDate = new DateTime(2025, 3, 10);
            products.Add(l2);

            Laptop l3 = new Laptop();
            l3.Id = 3; l3.Article = "LT-003"; l3.Brand = "HP"; l3.ModelName = "Pavilion 15";
            l3.BasePrice = 28000; l3.RamGb = 8; l3.StorageGb = 256;
            l3.SaleDate = new DateTime(2025, 4, 2);
            products.Add(l3);

            Laptop l4 = new Laptop();
            l4.Id = 4; l4.Article = "LT-004"; l4.Brand = "Acer"; l4.ModelName = "Aspire 5";
            l4.BasePrice = 25000; l4.RamGb = 8; l4.StorageGb = 256;
            l4.SaleDate = new DateTime(2025, 4, 15);
            products.Add(l4);

            Laptop l5 = new Laptop();
            l5.Id = 5; l5.Article = "LT-005"; l5.Brand = "Dell"; l5.ModelName = "Inspiron 15";
            l5.BasePrice = 40000; l5.RamGb = 16; l5.StorageGb = 1024;
            products.Add(l5);

            Smartphone s1 = new Smartphone();
            s1.Id = 6; s1.Article = "SM-001"; s1.Brand = "Samsung"; s1.ModelName = "Galaxy A55";
            s1.BasePrice = 20000; s1.CameraMP = 50; s1.BatteryMah = 5000;
            s1.SaleDate = new DateTime(2025, 3, 8);
            products.Add(s1);

            Smartphone s2 = new Smartphone();
            s2.Id = 7; s2.Article = "SM-002"; s2.Brand = "Xiaomi"; s2.ModelName = "Redmi Note 13";
            s2.BasePrice = 15000; s2.CameraMP = 108; s2.BatteryMah = 5000;
            s2.SaleDate = new DateTime(2025, 3, 20);
            products.Add(s2);

            Smartphone s3 = new Smartphone();
            s3.Id = 8; s3.Article = "SM-003"; s3.Brand = "Apple"; s3.ModelName = "iPhone 15";
            s3.BasePrice = 80000; s3.CameraMP = 48; s3.BatteryMah = 3877;
            s3.SaleDate = new DateTime(2025, 4, 1);
            products.Add(s3);

            Smartphone s4 = new Smartphone();
            s4.Id = 9; s4.Article = "SM-004"; s4.Brand = "Realme"; s4.ModelName = "C65";
            s4.BasePrice = 10000; s4.CameraMP = 50; s4.BatteryMah = 5000;
            s4.SaleDate = new DateTime(2025, 4, 10);
            products.Add(s4);

            Smartphone s5 = new Smartphone();
            s5.Id = 10; s5.Article = "SM-005"; s5.Brand = "OnePlus"; s5.ModelName = "12R";
            s5.BasePrice = 45000; s5.CameraMP = 50; s5.BatteryMah = 5500;
            products.Add(s5);

            Tablet t1 = new Tablet();
            t1.Id = 11; t1.Article = "TB-001"; t1.Brand = "Apple"; t1.ModelName = "iPad 10";
            t1.BasePrice = 50000; t1.ScreenInch = 10.9; t1.HasLTE = false;
            t1.SaleDate = new DateTime(2025, 3, 15);
            products.Add(t1);

            Tablet t2 = new Tablet();
            t2.Id = 12; t2.Article = "TB-002"; t2.Brand = "Samsung"; t2.ModelName = "Galaxy Tab A9";
            t2.BasePrice = 25000; t2.ScreenInch = 8.7; t2.HasLTE = true;
            t2.SaleDate = new DateTime(2025, 3, 25);
            products.Add(t2);

            Tablet t3 = new Tablet();
            t3.Id = 13; t3.Article = "TB-003"; t3.Brand = "Lenovo"; t3.ModelName = "Tab P12";
            t3.BasePrice = 30000; t3.ScreenInch = 12.7; t3.HasLTE = false;
            t3.SaleDate = new DateTime(2025, 4, 5);
            products.Add(t3);

            Tablet t4 = new Tablet();
            t4.Id = 14; t4.Article = "TB-004"; t4.Brand = "Huawei"; t4.ModelName = "MatePad 11";
            t4.BasePrice = 35000; t4.ScreenInch = 11.0; t4.HasLTE = true;
            t4.SaleDate = new DateTime(2025, 4, 20);
            products.Add(t4);

            Tablet t5 = new Tablet();
            t5.Id = 15; t5.Article = "TB-005"; t5.Brand = "Xiaomi"; t5.ModelName = "Pad 6";
            t5.BasePrice = 28000; t5.ScreenInch = 11.0; t5.HasLTE = false;
            products.Add(t5);

            return products;
        }

        private static List<Report> CreateReports(List<ITProduct> products)
        {
            List<Report> reports = new List<Report>();

            Predicate<ITProduct> isSoldInMarch = delegate(ITProduct p)
            {
                return p.SaleDate.HasValue && p.SaleDate.Value.Month == 3;
            };

            Predicate<ITProduct> isSoldInApril = delegate(ITProduct p)
            {
                return p.SaleDate.HasValue && p.SaleDate.Value.Month == 4;
            };

            List<ITProduct> laptops = Filter<Laptop>(products);
            List<ITProduct> smartphones = Filter<Smartphone>(products);
            List<ITProduct> tablets = Filter<Tablet>(products);

            List<ITProduct> r1Items = new List<ITProduct>();
            for (int i = 0; i < laptops.Count; i++)
            {
                if (isSoldInMarch(laptops[i]))
                {
                    r1Items.Add(laptops[i]);
                }
            }
            Report r1 = new Report("Ноутбуки Март", new DateTime(2025, 3, 1), new DateTime(2025, 3, 31), r1Items);
            r1.Id = 1;
            reports.Add(r1);

            List<ITProduct> r2Items = new List<ITProduct>();
            for (int i = 0; i < smartphones.Count; i++)
            {
                if (isSoldInMarch(smartphones[i]))
                {
                    r2Items.Add(smartphones[i]);
                }
            }
            Report r2 = new Report("Смартфоны Март", new DateTime(2025, 3, 1), new DateTime(2025, 3, 31), r2Items);
            r2.Id = 2;
            reports.Add(r2);

            List<ITProduct> r3Items = new List<ITProduct>();
            for (int i = 0; i < tablets.Count; i++)
            {
                if (isSoldInMarch(tablets[i]))
                {
                    r3Items.Add(tablets[i]);
                }
            }
            Report r3 = new Report("Планшеты Март", new DateTime(2025, 3, 1), new DateTime(2025, 3, 31), r3Items);
            r3.Id = 3;
            reports.Add(r3);

            List<ITProduct> r4Items = new List<ITProduct>();
            for (int i = 0; i < laptops.Count; i++)
            {
                if (isSoldInApril(laptops[i]))
                {
                    r4Items.Add(laptops[i]);
                }
            }
            Report r4 = new Report("Ноутбуки Апрель", new DateTime(2025, 4, 1), new DateTime(2025, 4, 30), r4Items);
            r4.Id = 4;
            reports.Add(r4);

            List<ITProduct> r5Items = new List<ITProduct>();
            for (int i = 0; i < smartphones.Count; i++)
            {
                if (isSoldInApril(smartphones[i]))
                {
                    r5Items.Add(smartphones[i]);
                }
            }
            Report r5 = new Report("Смартфоны Апрель", new DateTime(2025, 4, 1), new DateTime(2025, 4, 30), r5Items);
            r5.Id = 5;
            reports.Add(r5);

            List<ITProduct> r6Items = new List<ITProduct>();
            for (int i = 0; i < tablets.Count; i++)
            {
                if (isSoldInApril(tablets[i]))
                {
                    r6Items.Add(tablets[i]);
                }
            }
            Report r6 = new Report("Планшеты Апрель", new DateTime(2025, 4, 1), new DateTime(2025, 4, 30), r6Items);
            r6.Id = 6;
            reports.Add(r6);

            return reports;
        }
    }
}
