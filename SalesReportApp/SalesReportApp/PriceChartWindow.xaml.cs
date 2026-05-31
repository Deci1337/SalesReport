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

        private const double PadLeft   = 90;
        private const double PadRight  = 30;
        private const double PadTop    = 30;
        private const double PadBottom = 50;
        private const int    GridLines = 5;

        public PriceChartWindow(PricePoint[] points, string article)
        {
            InitializeComponent();
            _points  = points;
            _article = article;
            Title    = "Динамика цены: " + article;
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

            double W = chartCanvas.ActualWidth;
            double H = chartCanvas.ActualHeight;
            if (W < 20 || H < 20) return;

            double cW = W - PadLeft - PadRight;
            double cH = H - PadTop  - PadBottom;

            decimal minP = _points[0].AvgPrice;
            decimal maxP = _points[0].AvgPrice;
            for (int i = 1; i < _points.Length; i++)
            {
                if (_points[i].AvgPrice < minP) minP = _points[i].AvgPrice;
                if (_points[i].AvgPrice > maxP) maxP = _points[i].AvgPrice;
            }
            decimal spread = maxP - minP;
            if (spread == 0) { spread = minP * 0.1m; }
            decimal pad = spread * 0.15m;
            minP -= pad;
            maxP += pad;
            double priceRange = (double)(maxP - minP);

            // Координаты точек
            double[] xs = new double[_points.Length];
            double[] ys = new double[_points.Length];
            for (int i = 0; i < _points.Length; i++)
            {
                double xRatio = _points.Length > 1 ? (double)i / (_points.Length - 1) : 0.5;
                xs[i] = PadLeft + xRatio * cW;
                double yRatio = (double)(_points[i].AvgPrice - minP) / priceRange;
                ys[i] = PadTop + cH - yRatio * cH;
            }

            // Фон области графика
            Rectangle bg = new Rectangle();
            bg.Width  = cW;
            bg.Height = cH;
            bg.Fill   = new SolidColorBrush(Color.FromRgb(252, 249, 255));
            bg.RadiusX = 6; bg.RadiusY = 6;
            Canvas.SetLeft(bg, PadLeft);
            Canvas.SetTop(bg,  PadTop);
            chartCanvas.Children.Add(bg);

            // Горизонтальные линии сетки + метки Y
            for (int g = 0; g <= GridLines; g++)
            {
                double ratio = (double)g / GridLines;
                double y = PadTop + cH - ratio * cH;
                decimal price = minP + (decimal)(ratio * priceRange);

                Line grid = new Line();
                grid.X1 = PadLeft; grid.Y1 = y;
                grid.X2 = PadLeft + cW; grid.Y2 = y;
                grid.Stroke = new SolidColorBrush(Color.FromRgb(221, 208, 232));
                grid.StrokeThickness = 1;
                grid.StrokeDashArray = new DoubleCollection(new double[] { 4, 3 });
                chartCanvas.Children.Add(grid);

                TextBlock label = new TextBlock();
                label.Text = ((int)price).ToString("N0");
                label.FontSize = 10;
                label.Foreground = new SolidColorBrush(Color.FromRgb(154, 138, 165));
                label.Width = PadLeft - 8;
                label.TextAlignment = TextAlignment.Right;
                Canvas.SetLeft(label, 0);
                Canvas.SetTop(label, y - 8);
                chartCanvas.Children.Add(label);
            }

            // Заливка под линией
            if (_points.Length > 1)
            {
                PathFigure fig = new PathFigure();
                fig.StartPoint = new Point(xs[0], PadTop + cH);
                fig.Segments.Add(new LineSegment(new Point(xs[0], ys[0]), false));
                for (int i = 1; i < _points.Length; i++)
                {
                    fig.Segments.Add(new LineSegment(new Point(xs[i], ys[i]), true));
                }
                fig.Segments.Add(new LineSegment(new Point(xs[_points.Length - 1], PadTop + cH), false));
                fig.IsClosed = true;

                PathGeometry geo = new PathGeometry();
                geo.Figures.Add(fig);
                Path fill = new Path();
                fill.Data = geo;
                fill.Fill = new SolidColorBrush(Color.FromArgb(40, 165, 107, 181));
                chartCanvas.Children.Add(fill);
            }

            // Линия графика
            for (int i = 0; i < _points.Length - 1; i++)
            {
                Line line = new Line();
                line.X1 = xs[i];   line.Y1 = ys[i];
                line.X2 = xs[i+1]; line.Y2 = ys[i+1];
                line.Stroke = new SolidColorBrush(Color.FromRgb(165, 107, 181));
                line.StrokeThickness = 2.5;
                line.StrokeLineJoin = PenLineJoin.Round;
                chartCanvas.Children.Add(line);
            }

            // Точки и подписи
            for (int i = 0; i < _points.Length; i++)
            {
                // Кружок — белый с фиолетовой обводкой
                Ellipse outer = new Ellipse();
                outer.Width  = 12; outer.Height = 12;
                outer.Fill   = Brushes.White;
                outer.Stroke = new SolidColorBrush(Color.FromRgb(165, 107, 181));
                outer.StrokeThickness = 2.5;
                Canvas.SetLeft(outer, xs[i] - 6);
                Canvas.SetTop(outer,  ys[i] - 6);
                chartCanvas.Children.Add(outer);

                // Подпись цены над точкой
                TextBlock priceLabel = new TextBlock();
                priceLabel.Text       = ((int)_points[i].AvgPrice).ToString("N0");
                priceLabel.FontSize   = 10;
                priceLabel.FontWeight = FontWeights.SemiBold;
                priceLabel.Foreground = new SolidColorBrush(Color.FromRgb(92, 45, 110));
                Canvas.SetLeft(priceLabel, xs[i] - 22);
                Canvas.SetTop(priceLabel,  ys[i] - 22);
                chartCanvas.Children.Add(priceLabel);

                // Подпись даты под осью X
                TextBlock dateLabel = new TextBlock();
                dateLabel.Text       = _points[i].Date.ToString("dd.MM");
                dateLabel.FontSize   = 10;
                dateLabel.Foreground = new SolidColorBrush(Color.FromRgb(154, 138, 165));
                Canvas.SetLeft(dateLabel, xs[i] - 14);
                Canvas.SetTop(dateLabel,  PadTop + cH + 8);
                chartCanvas.Children.Add(dateLabel);

                // Вертикальная пунктирная линия вниз
                Line drop = new Line();
                drop.X1 = xs[i]; drop.Y1 = ys[i] + 6;
                drop.X2 = xs[i]; drop.Y2 = PadTop + cH;
                drop.Stroke = new SolidColorBrush(Color.FromArgb(60, 165, 107, 181));
                drop.StrokeThickness = 1;
                drop.StrokeDashArray = new DoubleCollection(new double[] { 3, 3 });
                chartCanvas.Children.Add(drop);
            }

            // Ось X
            Line axisX = new Line();
            axisX.X1 = PadLeft; axisX.Y1 = PadTop + cH;
            axisX.X2 = PadLeft + cW; axisX.Y2 = PadTop + cH;
            axisX.Stroke = new SolidColorBrush(Color.FromRgb(165, 107, 181));
            axisX.StrokeThickness = 1.5;
            chartCanvas.Children.Add(axisX);
        }
    }
}
