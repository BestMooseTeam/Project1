using System;
using System.Globalization;
using System.Text;

namespace PaymentMethods
{
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Product[] catalog = new Product[]
            {
                new Product("Клавиатура", 4590m),
                new Product("Мышь", 1890m),
                new Product("Наушники", 3290m),
                new Product("USB-хаб", 990m)
            };

            Cart cart = new Cart();

            Console.WriteLine("=== Интернет-магазин ===");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 — каталог товаров");
                Console.WriteLine("2 — корзина");
                Console.WriteLine("3 — оформить заказ");
                Console.WriteLine("0 — выход");
                Console.Write("Выбор: ");

                switch (ReadLineOrExit())
                {
                    case "1":
                        AddFromCatalog(catalog, cart);
                        break;
                    case "2":
                        ShowCart(cart);
                        break;
                    case "3":
                        Checkout(cart);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }

        private static void AddFromCatalog(Product[] catalog, Cart cart)
        {
            Console.WriteLine();
            Console.WriteLine("Каталог:");
            for (int i = 0; i < catalog.Length; i++)
                Console.WriteLine("  {0} — {1}", i + 1, catalog[i]);

            Console.Write("Номер товара (Enter — назад): ");
            string input = ReadLineOrExit();
            if (string.IsNullOrEmpty(input))
                return;

            int number;
            if (!int.TryParse(input, out number) || number < 1 || number > catalog.Length)
            {
                Console.WriteLine("Нет такого товара.");
                return;
            }

            Product product = catalog[number - 1];
            cart.Add(product);
            Console.WriteLine("В корзину добавлено: {0}", product);
        }

        private static void ShowCart(Cart cart)
        {
            Console.WriteLine();
            Console.WriteLine("Корзина:");
            Console.WriteLine(cart.FormatItems());
            if (cart.Count > 0)
            {
                Console.WriteLine(
                    "Итого: {0}",
                    string.Format(CultureInfo.GetCultureInfo("ru-RU"), "{0:N2} ₽", cart.Total));
            }
        }

        private static void Checkout(Cart cart)
        {
            if (cart.Count == 0)
            {
                Console.WriteLine("Корзина пуста — сначала добавьте товары.");
                return;
            }

            ShowCart(cart);
            IPaymentMethod method = AskPaymentMethod();
            if (method == null)
            {
                Console.WriteLine("Оформление отменено.");
                return;
            }

            cart.PaymentMethod = method;
            cart.Checkout();
        }

        private static IPaymentMethod AskPaymentMethod()
        {
            Console.WriteLine();
            Console.WriteLine("Способ оплаты:");
            Console.WriteLine("  1 — банковская карта");
            Console.WriteLine("  2 — PayPal");
            Console.WriteLine("  3 — банковский перевод");
            Console.Write("Выбор (Enter — отмена): ");

            switch (ReadLineOrExit())
            {
                case "1":
                    return new CreditCard("Иван Петров", "4276 1200 0000 1234");
                case "2":
                    return new PayPal("ivan.petrov@example.com");
                case "3":
                    return new BankTransfer("40817810000001234567", "Т-Банк");
                case "":
                    return null;
                default:
                    Console.WriteLine("Неизвестный способ оплаты.");
                    return null;
            }
        }

        private static string ReadLineOrExit()
        {
            string line = Console.ReadLine();
            if (line == null)
                Environment.Exit(0);
            char bom = (char)0xFEFF;
            return line.Trim().Trim(bom).Trim();
        }
    }
}
