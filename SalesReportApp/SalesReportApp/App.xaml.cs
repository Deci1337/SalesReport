using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Model.Core;
using Model.Data;

namespace SalesReportApp
{
    public partial class App : Application
    {
        public static List<Report> Reports { get; set; }
        public static Serializer CurrentSerializer { get; set; }
        public static string CurrentFormat { get; set; }
        public static string DataFolder { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CurrentFormat = "json";
            DataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data_json");
            CurrentSerializer = SerializerFactory.Create(CurrentFormat);

            try
            {
                Reports = CurrentSerializer.Load(DataFolder);
                if (Reports.Count == 0)
                {
                    Reports = DataSeeder.Initialize();
                    CurrentSerializer.Save(Reports, DataFolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                Reports = DataSeeder.Initialize();
            }
        }
    }
}
