using System;

namespace Model.Core
{
    public class Tablet : ITProduct
    {
        public double ScreenInch { get; set; }
        public bool HasLTE { get; set; }

        public Tablet()
        {
        }

        public Tablet(int id, string article, string brand, string model, decimal basePrice, DateTime? saleDate, double screenInch, bool hasLTE)
            : base(id, article, brand, model, basePrice, saleDate)
        {
            ScreenInch = screenInch;
            HasLTE = hasLTE;
        }

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
            string lte = "нет";
            if (HasLTE)
            {
                lte = "есть";
            }
            return "Tablet: " + Brand + " " + Model + " [" + Article + "] Экран:" + ScreenInch + "\" LTE:" + lte + " Цена:" + Price + " руб.";
        }
    }
}
