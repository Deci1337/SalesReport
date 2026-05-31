using System;
using System.Xml.Serialization;

namespace Model.Core
{
    [XmlInclude(typeof(Laptop))]
    [XmlInclude(typeof(Smartphone))]
    [XmlInclude(typeof(Tablet))]
    public abstract class ITProduct
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal BasePrice { get; set; }

        public abstract decimal Price { get; }

        public ITProduct()
        {
        }

        public ITProduct(int id, string article, string brand, string model, decimal basePrice, DateTime? saleDate)
        {
            Id = id;
            Article = article;
            Brand = brand;
            Model = model;
            BasePrice = basePrice;
            SaleDate = saleDate;
        }

        public static bool operator ==(ITProduct a, ITProduct b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
            {
                return true;
            }
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return false;
            }
            return a.Article == b.Article;
        }

        public static bool operator !=(ITProduct a, ITProduct b)
        {
            if (a == b)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            ITProduct other = obj as ITProduct;
            if (other == null)
            {
                return false;
            }
            return this.Article == other.Article;
        }

        public override int GetHashCode()
        {
            if (Article == null)
            {
                return 0;
            }
            return Article.GetHashCode();
        }

        public override string ToString()
        {
            return Brand + " " + Model + " [" + Article + "] " + Price + " руб.";
        }
    }
}
