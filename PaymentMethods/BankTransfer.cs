using System;
using System.Globalization;

namespace PaymentMethods
{
    /// <summary>Оплата банковским переводом.</summary>
    public class BankTransfer : IPaymentMethod
    {
        private readonly string _account;
        private readonly string _bankName;

        public BankTransfer(string account, string bankName)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("Номер счёта не может быть пустым.", "account");
            if (string.IsNullOrWhiteSpace(bankName))
                throw new ArgumentException("Название банка не может быть пустым.", "bankName");

            _account = account.Trim();
            _bankName = bankName.Trim();
        }

        public string Name
        {
            get { return "Банковский перевод"; }
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine(
                "Оплата через банковский перевод: {0} переведено со счёта {1} ({2}).",
                FormatMoney(amount),
                _account,
                _bankName);
        }

        private static string FormatMoney(decimal amount)
        {
            return string.Format(CultureInfo.GetCultureInfo("ru-RU"), "{0:N2} ₽", amount);
        }
    }
}
