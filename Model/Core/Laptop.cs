using System;

namespace Model.Core
{
    public class Laptop : ITProduct
    {
        public int RamGb { get; set; }
        public int StorageGb { get; set; }

        public Laptop()
        {
        }

        public Laptop(int id, string article, string brand, string model, decimal basePrice, DateTime? saleDate, int ramGb, int storageGb)
            : base(id, article, brand, model, basePrice, saleDate)
        {
            RamGb = ramGb;
            StorageGb = storageGb;
        }

        public override decimal Price
        {
            get
            {
                return BasePrice + RamGb * 200 + StorageGb * 0.5m;
            }
        }

        public override string ToString()
        {
            return "Laptop: " + Brand + " " + Model + " [" + Article + "] RAM:" + RamGb + "GB SSD:" + StorageGb + "GB Цена:" + Price + " руб.";
        }
    }
}
