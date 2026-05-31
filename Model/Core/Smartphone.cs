using System;

namespace Model.Core
{
    [Serializable]
    public class Smartphone : ITProduct
    {
        public int CameraMP { get; set; }
        public int BatteryMah { get; set; }

        public override decimal Price
        {
            get
            {
                return BasePrice + CameraMP * 50 + BatteryMah * 0.05m;
            }
        }

        public override string ToString()
        {
            return "Смартфон: " + Brand + " " + ModelName
                + " Камера:" + CameraMP + "MP Батарея:" + BatteryMah + "mAh"
                + " Цена:" + Price + " руб.";
        }
    }
}
