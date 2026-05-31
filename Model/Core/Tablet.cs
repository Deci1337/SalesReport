using System;

namespace Model.Core
{
    [Serializable]
    public class Tablet : ITProduct
    {
        public double ScreenInch { get; set; }
        public bool HasLTE { get; set; }

        public override decimal Price
        {
            get
            {
                decimal lteBonus = 0;
                if (HasLTE)
                {
                    lteBonus = 2000;
                }
                return BasePrice + (decimal)(ScreenInch * 300) + lteBonus;
            }
        }

        public override string ToString()
        {
            string lte = "WiFi";
            if (HasLTE)
            {
                lte = "LTE";
            }
            return "Планшет: " + Brand + " " + ModelName
                + " " + ScreenInch + "\" " + lte
                + " Цена:" + Price + " руб.";
        }
    }
}
