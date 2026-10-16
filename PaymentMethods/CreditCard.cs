using System;
using System.Globalization;

namespace PaymentMethods
{
    /// <summary>Оплата банковской картой.</summary>
    public class CreditCard : IPaymentMethod
    {
        private readonly string _holder;
        private readonly string _maskedNumber;

        public CreditCard(string holder, string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(holder))
                throw new ArgumentException("Имя держателя карты не может быть пустым.", "holder");
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentException("Номер карты не может быть пустым.", "cardNumber");

            _holder = holder.Trim();
            _maskedNumber = Mask(cardNumber);
        }

        public string Name
        {
            get { return "Банковская карта"; }
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine(
                "Оплата картой: с карты {0} ({1}) списано {2}.",
                _maskedNumber,
                _holder,
                FormatMoney(amount));
        }

        private static string Mask(string cardNumber)
        {
            string digits = cardNumber.Replace(" ", "").Replace("-", "");
            if (digits.Length < 4)
                return "****";
            return "**** **** **** " + digits.Substring(digits.Length - 4);
        }

        private static string FormatMoney(decimal amount)
        {
            return string.Format(CultureInfo.GetCultureInfo("ru-RU"), "{0:N2} ₽", amount);
        }
    }
}
