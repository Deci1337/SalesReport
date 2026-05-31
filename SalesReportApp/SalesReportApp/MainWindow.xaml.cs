using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Model.Core;
using Model.Data;

namespace SalesReportApp
{
    public class CheckBoxItem
    {
        public bool IsChecked { get; set; }
        public Report Report { get; set; }
        public string DisplayName { get; set; }
    }

    public partial class MainWindow : Window
    {
        private bool _formatChanging = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            cbDeviceType.Items.Add("Все");
            cbDeviceType.Items.Add("Ноутбуки");
            cbDeviceType.Items.Add("Смартфоны");
            cbDeviceType.Items.Add("Планшеты");
            cbDeviceType.SelectedIndex = 0;

            cbPeriod.Items.Add("День");
            cbPeriod.Items.Add("Неделя");
            cbPeriod.Items.Add("Месяц");
            cbPeriod.Items.Add("Квартал");
            cbPeriod.Items.Add("Год");

            cbFormat.Items.Add("JSON");
            cbFormat.Items.Add("XML");
            cbFormat.SelectedIndex = 0;
        }

        private void cbDeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateReportList();
        }

        private void cbPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateReportList();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateReportList();
        }

        private void UpdateReportList()
        {
            if (lbReports == null) return;
            if (cbPeriod.SelectedItem == null) return;
            if (dpDate.SelectedDate == null) return;

            DateTime selectedDate = dpDate.SelectedDate.Value;
            string period = cbPeriod.SelectedItem.ToString();

            DateTime start = GetPeriodStart(selectedDate, period);
            DateTime end = GetPeriodEnd(selectedDate, period);

            List<CheckBoxItem> items = new List<CheckBoxItem>();
            List<Report> reports = App.Reports;

            Type deviceType = GetSelectedDeviceType();

            for (int i = 0; i < reports.Count; i++)
            {
                Report report = reports[i];
                if (ReportHasProductsInRange(report, start, end, deviceType))
                {
                    CheckBoxItem item = new CheckBoxItem();
                    item.Report = report;
                    item.IsChecked = false;
                    item.DisplayName = report.Name + " (" + report.PeriodStart.ToShortDateString() + " - " + report.PeriodEnd.ToShortDateString() + ")";
                    items.Add(item);
                }
            }

            lbReports.ItemsSource = null;
            lbReports.ItemsSource = items;
            btnShowReport.IsEnabled = false;
        }

        private bool ReportHasProductsInRange(Report report, DateTime start, DateTime end, Type deviceType)
        {
            for (int i = 0; i < report.Products.Count; i++)
            {
                ITProduct product = report.Products[i];
                if (!product.SaleDate.HasValue) continue;
                DateTime saleDate = product.SaleDate.Value;
                if (saleDate < start || saleDate > end) continue;
                if (deviceType == typeof(ITProduct)) return true;
                if (product.GetType() == deviceType) return true;
            }
            return false;
        }

        private DateTime GetPeriodStart(DateTime date, string period)
        {
            if (period == "День")
            {
                return date.Date;
            }
            if (period == "Неделя")
            {
                int dayOfWeek = (int)date.DayOfWeek;
                if (dayOfWeek == 0) dayOfWeek = 7;
                return date.Date.AddDays(-(dayOfWeek - 1));
            }
            if (period == "Месяц")
            {
                return new DateTime(date.Year, date.Month, 1);
            }
            if (period == "Квартал")
            {
                int quarter = (date.Month - 1) / 3;
                int startMonth = quarter * 3 + 1;
                return new DateTime(date.Year, startMonth, 1);
            }
            if (period == "Год")
            {
                return new DateTime(date.Year, 1, 1);
            }
            return date.Date;
        }

        private DateTime GetPeriodEnd(DateTime date, string period)
        {
            if (period == "День")
            {
                return date.Date;
            }
            if (period == "Неделя")
            {
                return GetPeriodStart(date, "Неделя").AddDays(6);
            }
            if (period == "Месяц")
            {
                DateTime start = new DateTime(date.Year, date.Month, 1);
                return start.AddMonths(1).AddDays(-1);
            }
            if (period == "Квартал")
            {
                DateTime start = GetPeriodStart(date, "Квартал");
                return start.AddMonths(3).AddDays(-1);
            }
            if (period == "Год")
            {
                return new DateTime(date.Year, 12, 31);
            }
            return date.Date;
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            bool anyChecked = false;
            if (lbReports.ItemsSource != null)
            {
                List<CheckBoxItem> items = (List<CheckBoxItem>)lbReports.ItemsSource;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].IsChecked)
                    {
                        anyChecked = true;
                        break;
                    }
                }
            }
            btnShowReport.IsEnabled = anyChecked;
        }

        private void cbFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_formatChanging) return;
            if (cbFormat.SelectedItem == null) return;

            string newFormat = cbFormat.SelectedItem.ToString().ToLower();
            if (newFormat == App.CurrentFormat) return;

            _formatChanging = true;
            try
            {
                string oldFolder = App.DataFolder;
                string newFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data_" + newFormat);

                Serializer newSerializer = SerializerFactory.Create(newFormat);
                newSerializer.Save(App.Reports, newFolder);

                App.CurrentFormat = newFormat;
                App.DataFolder = newFolder;
                App.CurrentSerializer = newSerializer;

                MessageBox.Show("Данные скопированы в формат " + cbFormat.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка смены формата: " + ex.Message);
            }
            finally
            {
                _formatChanging = false;
            }
        }

        private void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            List<Report> selectedReports = new List<Report>();

            if (lbReports.ItemsSource != null)
            {
                List<CheckBoxItem> items = (List<CheckBoxItem>)lbReports.ItemsSource;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].IsChecked)
                    {
                        selectedReports.Add(items[i].Report);
                    }
                }
            }

            if (selectedReports.Count == 0) return;

            Type deviceType = GetSelectedDeviceType();

            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;

            if (dpDate.SelectedDate != null && cbPeriod.SelectedItem != null)
            {
                string period = cbPeriod.SelectedItem.ToString();
                start = GetPeriodStart(dpDate.SelectedDate.Value, period);
                end = GetPeriodEnd(dpDate.SelectedDate.Value, period);
            }

            ReportWindow window = new ReportWindow(selectedReports, deviceType, start, end);
            window.Show();
        }

        private Type GetSelectedDeviceType()
        {
            if (cbDeviceType.SelectedItem == null)
            {
                return typeof(ITProduct);
            }
            string selected = cbDeviceType.SelectedItem.ToString();
            if (selected == "Ноутбуки")
            {
                return typeof(Laptop);
            }
            if (selected == "Смартфоны")
            {
                return typeof(Smartphone);
            }
            if (selected == "Планшеты")
            {
                return typeof(Tablet);
            }
            return typeof(ITProduct);
        }
    }
}
