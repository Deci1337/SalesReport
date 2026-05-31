using System;

namespace Model.Core
{
    public class Smartphone : ITProduct
    {
        public int CameraMP { get; set; }
        public int BatteryMah { get; set; }

        public Smartphone()
        {
        }

        public Smartphone(int id, string article, string brand, string model, decimal basePrice, DateTime? saleDate, int cameraMP, int batteryMah)
            : base(id, article, brand, model, basePrice, saleDate)
        {
            CameraMP = cameraMP;
            BatteryMah = batteryMah;
        }

        public override decimal Price
        {
            get
            {
                return BasePrice + CameraMP * 50 + BatteryMah * 0.05m;
            }
        }

        public override string ToString()
        {
            return "Smartphone: " + Brand + " " + Model + " [" + Article + "] Камера:" + CameraMP + "MP Батарея:" + BatteryMah + "mAh Цена:" + Price + " руб.";
        }
    }
}
