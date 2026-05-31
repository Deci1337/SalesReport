using System;

namespace Model.Core
{
    public struct PricePoint
    {
        public DateTime Date { get; set; }
        public decimal AvgPrice { get; set; }
    }
}
