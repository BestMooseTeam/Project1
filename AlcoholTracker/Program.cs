using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AlcoholTracker
{
    public static class Program
    {
        // Ориентиры низкорискового потребления: 2 стандартные дозы в день для мужчин, 1 — для женщин
        private const double MaleLimitGrams = 20.0;
        private const double FemaleLimitGrams = 10.0;

        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Трекер выпитого алкоголя ===");
            Console.WriteLine();

            Tracker tracker = new Tracker(AskDailyLimit());

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 — добавить напиток");
                Console.WriteLine("2 — показать итог");
                Console.WriteLine("3 — сбросить счётчик");
                Console.WriteLine("0 — выход");
                Console.Write("Выбор: ");

                string choice = ReadLineOrExit();
                switch (choice)
                {
                    case "1":
                        AddDrink(tracker);
                        break;
                    case "2":
                        PrintSummary(tracker);
                        break;
                    case "3":
                        tracker.Reset();
                        Console.WriteLine("Счётчик сброшен.");
                        break;
                    case "0":
                        PrintSummary(tracker);
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }

        private static double AskDailyLimit()
        {
            while (true)
            {
                Console.Write("Ваш пол (m/f): ");
                string input = ReadLineOrExit().ToLowerInvariant();
                if (input == "м" || input == "m")
                    return MaleLimitGrams;
                if (input == "ж" || input == "f" || input == "w")
                    return FemaleLimitGrams;
                Console.WriteLine("Введите «м» или «ж».");
            }
        }

        private static void AddDrink(Tracker tracker)
        {
            Drink drink = AskDrink();
            int servings = AskPositiveInt(string.Format("Количество порций ({0} мл): ", drink.ServingMl));

            tracker.Add(drink, servings);
            Console.WriteLine("Добавлено: {0} × {1} = {2:F1} г спирта.",
                drink.Name, servings, drink.GramsPerServing * servings);

            PrintStatus(tracker);
        }

        private static Drink AskDrink()
        {
            Drink[] drinks = Drink.Defaults;
            while (true)
            {
                Console.WriteLine("Тип напитка:");
                for (int i = 0; i < drinks.Length; i++)
                {
                    Console.WriteLine("  {0} — {1} ({2} мл, {3}%, {4:F1} г спирта)",
                        i + 1, drinks[i].Name, drinks[i].ServingMl, drinks[i].AlcoholPercent, drinks[i].GramsPerServing);
                }
                Console.Write("Номер или название: ");
                string input = ReadLineOrExit();

                int index;
                if (int.TryParse(input, out index) && index >= 1 && index <= drinks.Length)
                    return drinks[index - 1];

                Drink byName = drinks.FirstOrDefault(d =>
                    string.Equals(d.Name, input, StringComparison.CurrentCultureIgnoreCase));
                if (byName != null)
                    return byName;

                Console.WriteLine("Такого напитка нет, попробуйте ещё раз.");
            }
        }

        private static int AskPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                int value;
                if (int.TryParse(ReadLineOrExit(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value)
                    && value > 0 && value <= 100)
                    return value;
                Console.WriteLine("Введите целое число от 1 до 100.");
            }
        }

        private static void PrintSummary(Tracker tracker)
        {
            Console.WriteLine();
            Console.WriteLine("--- Итог ---");
            if (tracker.Entries.Count == 0)
            {
                Console.WriteLine("Пока ничего не добавлено.");
            }
            else
            {
                foreach (var group in tracker.Entries.GroupBy(e => e.Drink))
                {
                    Console.WriteLine("{0}: {1} порц., {2:F1} г спирта",
                        group.Key.Name, group.Sum(e => e.Servings), group.Sum(e => e.Grams));
                }
            }
            PrintStatus(tracker);
        }

        private static void PrintStatus(Tracker tracker)
        {
            Console.WriteLine("Всего: {0:F1} г чистого спирта ({1:F1} станд. доз). Норма: {2:F0} г.",
                tracker.TotalGrams, tracker.TotalStandardUnits, tracker.DailyLimitGrams);

            ConsoleColor oldColor = Console.ForegroundColor;
            switch (tracker.Status)
            {
                case LimitStatus.Exceeded:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ВНИМАНИЕ: безопасная дневная норма превышена на {0:F1} г. Лучше остановиться.",
                        tracker.TotalGrams - tracker.DailyLimitGrams);
                    break;
                case LimitStatus.NearLimit:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Вы близки к дневной норме: осталось {0:F1} г.",
                        tracker.DailyLimitGrams - tracker.TotalGrams);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("В пределах нормы.");
                    break;
            }
            Console.ForegroundColor = oldColor;
        }

        // Завершает программу, если ввод закончился (Ctrl+Z / конец потока)
        private static string ReadLineOrExit()
        {
            string line = Console.ReadLine();
            if (line == null)
                Environment.Exit(0);
            // BOM может прийти в первой строке при перенаправлении ввода
            return line.Trim().Trim('\uFEFF').Trim();
        }
    }
}
