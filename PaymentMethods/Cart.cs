using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PaymentMethods
{
    /// <summary>Корзина: товары и выбранный способ оплаты.</summary>
    public class Cart
    {
        private readonly List<Product> _items = new List<Product>();

        public IReadOnlyList<Product> Items
        {
            get { return _items; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public decimal Total
        {
            get
            {
                decimal sum = 0m;
                for (int i = 0; i < _items.Count; i++)
                    sum += _items[i].Price;
                return sum;
            }
        }

        public IPaymentMethod PaymentMethod { get; set; }

        public void Add(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            _items.Add(product);
        }

        public void Clear()
        {
            _items.Clear();
            PaymentMethod = null;
        }

        /// <summary>Проводит оплату выбранным способом и очищает корзину.</summary>
        public void Checkout()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Корзина пуста.");
            if (PaymentMethod == null)
                throw new InvalidOperationException("Не выбран способ оплаты.");

            Console.WriteLine();
            Console.WriteLine("Заказ:");
            Console.WriteLine(FormatItems());
            Console.WriteLine("Итого: {0}", FormatMoney(Total));
            Console.WriteLine("Способ: {0}", PaymentMethod.Name);
            PaymentMethod.Pay(Total);
            Console.WriteLine("Оплата прошла успешно. Спасибо за покупку!");
            Clear();
        }

        public string FormatItems()
        {
            if (_items.Count == 0)
                return "Корзина пуста.";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _items.Count; i++)
            {
                sb.AppendFormat(
                    CultureInfo.GetCultureInfo("ru-RU"),
                    "  {0}. {1}",
                    i + 1, _items[i]);
                if (i < _items.Count - 1)
                    sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string FormatMoney(decimal amount)
        {
            return string.Format(CultureInfo.GetCultureInfo("ru-RU"), "{0:N2} ₽", amount);
        }
    }
}
