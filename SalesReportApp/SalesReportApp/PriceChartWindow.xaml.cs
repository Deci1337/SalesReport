using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Model.Core;

namespace SalesReportApp
{
    public partial class PriceChartWindow : Window
    {
        private PricePoint[] _points;
        private string _article;

        private const double PaddingLeft = 70;
        private const double PaddingRight = 20;
        private const double PaddingTop = 20;
        private const double PaddingBottom = 50;

        public PriceChartWindow(PricePoint[] points, string article)
        {
            InitializeComponent();
            _points = points;
            _article = article;
            Title = "Динамика цены: " + article;
            lblTitle.Content = "Динамика цены — артикул: " + article;
        }

        private void chartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            chartCanvas.Children.Clear();

            if (_points == null || _points.Length == 0) return;

            double width = chartCanvas.ActualWidth;
            double height = chartCanvas.ActualHeight;

            if (width < 10 || height < 10) return;

            double chartWidth = width - PaddingLeft - PaddingRight;
            double chartHeight = height - PaddingTop - PaddingBottom;

            decimal minPrice = _points[0].AvgPrice;
            decimal maxPrice = _points[0].AvgPrice;
            for (int i = 1; i < _points.Length; i++)
            {
                if (_points[i].AvgPrice < minPrice) minPrice = _points[i].AvgPrice;
                if (_points[i].AvgPrice > maxPrice) maxPrice = _points[i].AvgPrice;
            }

            if (minPrice == maxPrice)
            {
                minPrice = minPrice - 100;
                maxPrice = maxPrice + 100;
            }

            // Оси
            Line axisX = new Line();
            axisX.X1 = PaddingLeft; axisX.Y1 = PaddingTop + chartHeight;
            axisX.X2 = PaddingLeft + chartWidth; axisX.Y2 = PaddingTop + chartHeight;
            axisX.Stroke = Brushes.Black; axisX.StrokeThickness = 1.5;
            chartCanvas.Children.Add(axisX);

            Line axisY = new Line();
            axisY.X1 = PaddingLeft; axisY.Y1 = PaddingTop;
            axisY.X2 = PaddingLeft; axisY.Y2 = PaddingTop + chartHeight;
            axisY.Stroke = Brushes.Black; axisY.StrokeThickness = 1.5;
            chartCanvas.Children.Add(axisY);

            // Точки и линии
            double[] xCoords = new double[_points.Length];
            double[] yCoords = new double[_points.Length];

            double priceRange = (double)(maxPrice - minPrice);

            for (int i = 0; i < _points.Length; i++)
            {
                double xRatio = 0;
                if (_points.Length > 1)
                {
                    xRatio = (double)i / (_points.Length - 1);
                }
                xCoords[i] = PaddingLeft + xRatio * chartWidth;

                double priceRatio = (double)(_points[i].AvgPrice - minPrice) / priceRange;
                yCoords[i] = PaddingTop + chartHeight - priceRatio * chartHeight;
            }

            // Линии между точками
            for (int i = 0; i < _points.Length - 1; i++)
            {
                Line line = new Line();
                line.X1 = xCoords[i]; line.Y1 = yCoords[i];
                line.X2 = xCoords[i + 1]; line.Y2 = yCoords[i + 1];
                line.Stroke = Brushes.SteelBlue;
                line.StrokeThickness = 2;
                chartCanvas.Children.Add(line);
            }

            // Точки
            for (int i = 0; i < _points.Length; i++)
            {
                Ellipse dot = new Ellipse();
                dot.Width = 8; dot.Height = 8;
                dot.Fill = Brushes.Red;
                Canvas.SetLeft(dot, xCoords[i] - 4);
                Canvas.SetTop(dot, yCoords[i] - 4);
                chartCanvas.Children.Add(dot);

                // Подпись цены над точкой
                TextBlock priceLabel = new TextBlock();
                priceLabel.Text = ((int)_points[i].AvgPrice).ToString();
                priceLabel.FontSize = 10;
                Canvas.SetLeft(priceLabel, xCoords[i] - 16);
                Canvas.SetTop(priceLabel, yCoords[i] - 18);
                chartCanvas.Children.Add(priceLabel);

                // Подпись даты под осью X
                TextBlock dateLabel = new TextBlock();
                dateLabel.Text = _points[i].Date.ToString("dd.MM");
                dateLabel.FontSize = 10;
                dateLabel.RenderTransform = new RotateTransform(-30);
                Canvas.SetLeft(dateLabel, xCoords[i] - 12);
                Canvas.SetTop(dateLabel, PaddingTop + chartHeight + 8);
                chartCanvas.Children.Add(dateLabel);
            }

            // Подпись оси Y (минимум и максимум)
            TextBlock minLabel = new TextBlock();
            minLabel.Text = ((int)minPrice).ToString();
            minLabel.FontSize = 10;
            Canvas.SetLeft(minLabel, 2);
            Canvas.SetTop(minLabel, PaddingTop + chartHeight - 8);
            chartCanvas.Children.Add(minLabel);

            TextBlock maxLabel = new TextBlock();
            maxLabel.Text = ((int)maxPrice).ToString();
            maxLabel.FontSize = 10;
            Canvas.SetLeft(maxLabel, 2);
            Canvas.SetTop(maxLabel, PaddingTop);
            chartCanvas.Children.Add(maxLabel);
        }
    }
}
