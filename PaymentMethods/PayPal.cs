using System;
using System.Globalization;

namespace PaymentMethods
{
    /// <summary>Оплата через PayPal.</summary>
    public class PayPal : IPaymentMethod
    {
        private readonly string _email;

        public PayPal(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.IndexOf('@') < 0)
                throw new ArgumentException("Укажите корректный e-mail аккаунта PayPal.", "email");

            _email = email.Trim();
        }

        public string Name
        {
            get { return "PayPal"; }
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine(
                "Оплата через PayPal: со счёта {0} списано {1}.",
                _email,
                FormatMoney(amount));
        }

        private static string FormatMoney(decimal amount)
        {
            return string.Format(CultureInfo.GetCultureInfo("ru-RU"), "{0:N2} ₽", amount);
        }
    }
}
