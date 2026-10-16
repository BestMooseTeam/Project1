using System;
using System.Globalization;

namespace PaymentMethods
{
    /// <summary>Товар интернет-магазина.</summary>
    public class Product
    {
        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым.", "name");
            if (price <= 0)
                throw new ArgumentOutOfRangeException("price", "Цена должна быть больше нуля.");

            Name = name.Trim();
            Price = price;
        }

        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.GetCultureInfo("ru-RU"),
                "{0} — {1:N2} ₽",
                Name, Price);
        }
    }
}
