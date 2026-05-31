using System;

namespace Model.Core
{
    [Serializable]
    public class Laptop : ITProduct
    {
        public int RamGb { get; set; }
        public int StorageGb { get; set; }

        public override decimal Price
        {
            get
            {
                return BasePrice + RamGb * 200 + StorageGb * 0.5m;
            }
        }

        public override string ToString()
        {
            return "Ноутбук: " + Brand + " " + ModelName
                + " RAM:" + RamGb + "GB SSD:" + StorageGb + "GB"
                + " Цена:" + Price + " руб.";
        }
    }
}
