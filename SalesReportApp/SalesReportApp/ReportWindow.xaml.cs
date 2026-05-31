using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Model.Core;

namespace SalesReportApp
{
    public class ProductRow
    {
        public string Article { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public string SaleDate { get; set; }
    }

    public partial class ReportWindow : Window
    {
        private Report _mergedReport;
        private Type _deviceType;
        private List<ProductRow> _currentRows;
        private static int _saveCounter = 1;

        public ReportWindow(List<Report> selectedReports, Type deviceType, DateTime start, DateTime end)
        {
            InitializeComponent();

            _deviceType = deviceType;
            _mergedReport = new Report("Сводный отчёт", selectedReports, start, end);
            _mergedReport.Sort(true);

            LoadTable();
            LoadArticleComboBox();
        }

        private void LoadTable()
        {
            IReportable reportable = (IReportable)_mergedReport;
            List<ITProduct> products = reportable.Select(_deviceType);

            _currentRows = new List<ProductRow>();
            for (int i = 0; i < products.Count; i++)
            {
                ITProduct product = products[i];
                ProductRow row = new ProductRow();
                row.Article = product.Article;
                row.Brand = product.Brand;
                row.ModelName = product.ModelName;
                row.TypeName = GetTypeName(product);
                row.Price = product.Price;
                if (product.SaleDate.HasValue)
                {
                    row.SaleDate = product.SaleDate.Value.ToShortDateString();
                }
                else
                {
                    row.SaleDate = "—";
                }
                _currentRows.Add(row);
            }

            dgProducts.ItemsSource = null;
            dgProducts.ItemsSource = _currentRows;
        }

        private void LoadArticleComboBox()
        {
            cbArticle.Items.Clear();
            for (int i = 0; i < _currentRows.Count; i++)
            {
                bool alreadyAdded = false;
                for (int j = 0; j < cbArticle.Items.Count; j++)
                {
                    if (cbArticle.Items[j].ToString() == _currentRows[i].Article)
                    {
                        alreadyAdded = true;
                        break;
                    }
                }
                if (!alreadyAdded)
                {
                    cbArticle.Items.Add(_currentRows[i].Article);
                }
            }
        }

        private string GetTypeName(ITProduct product)
        {
            if (product is Laptop)
            {
                return "Ноутбук";
            }
            if (product is Smartphone)
            {
                return "Смартфон";
            }
            if (product is Tablet)
            {
                return "Планшет";
            }
            return "Неизвестно";
        }

        private void btnSortAsc_Click(object sender, RoutedEventArgs e)
        {
            _mergedReport.Sort(true);
            LoadTable();
        }

        private void btnSortDesc_Click(object sender, RoutedEventArgs e)
        {
            _mergedReport.Sort(false);
            LoadTable();
        }

        private void cbArticle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnPriceChart.IsEnabled = cbArticle.SelectedItem != null;
        }

        private void btnPriceChart_Click(object sender, RoutedEventArgs e)
        {
            if (cbArticle.SelectedItem == null) return;
            string article = cbArticle.SelectedItem.ToString();
            PricePoint[] points = _mergedReport.GetPriceChanges(article);
            if (points.Length == 0)
            {
                MessageBox.Show("Нет данных о продажах для артикула: " + article);
                return;
            }
            PriceChartWindow chartWindow = new PriceChartWindow(points, article);
            chartWindow.Show();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string start = _mergedReport.PeriodStart.ToString("dd.MM");
                string end = _mergedReport.PeriodEnd.ToString("dd.MM");
                string fileName = "Отчет_№" + _saveCounter + "_за_" + start + "-" + end + ".txt";
                _saveCounter++;

                string folder = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(folder, fileName);

                IExportable exportable = (IExportable)_mergedReport;
                string content = exportable.Export();

                File.WriteAllText(filePath, content, Encoding.UTF8);
                MessageBox.Show("Отчёт сохранён: " + filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}
