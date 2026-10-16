using System;
using System.Collections.Generic;
using System.Linq;

namespace AlcoholTracker
{
    public enum LimitStatus
    {
        Safe,
        NearLimit,
        Exceeded
    }

    public class DrinkEntry
    {
        public Drink Drink { get; private set; }
        public int Servings { get; private set; }

        public DrinkEntry(Drink drink, int servings)
        {
            Drink = drink;
            Servings = servings;
        }

        public double Grams
        {
            get { return Drink.GramsPerServing * Servings; }
        }
    }

    /// <summary>Учёт выпитого и сравнение с дневной нормой.</summary>
    public class Tracker
    {
        // Стандартная доза (единица алкоголя) по ВОЗ — 10 г чистого спирта
        public const double GramsPerStandardUnit = 10.0;
        // Предупреждать заранее при достижении 80% нормы
        private const double NearLimitRatio = 0.8;

        private readonly List<DrinkEntry> _entries = new List<DrinkEntry>();

        public double DailyLimitGrams { get; private set; }

        public Tracker(double dailyLimitGrams)
        {
            if (dailyLimitGrams <= 0)
                throw new ArgumentOutOfRangeException("dailyLimitGrams");
            DailyLimitGrams = dailyLimitGrams;
        }

        public IList<DrinkEntry> Entries
        {
            get { return _entries.AsReadOnly(); }
        }

        public void Add(Drink drink, int servings)
        {
            if (drink == null)
                throw new ArgumentNullException("drink");
            if (servings <= 0)
                throw new ArgumentOutOfRangeException("servings", "Количество порций должно быть больше нуля.");
            _entries.Add(new DrinkEntry(drink, servings));
        }

        public void Reset()
        {
            _entries.Clear();
        }

        public double TotalGrams
        {
            get { return _entries.Sum(e => e.Grams); }
        }

        public double TotalStandardUnits
        {
            get { return TotalGrams / GramsPerStandardUnit; }
        }

        public LimitStatus Status
        {
            get
            {
                double total = TotalGrams;
                if (total > DailyLimitGrams)
                    return LimitStatus.Exceeded;
                if (total >= DailyLimitGrams * NearLimitRatio)
                    return LimitStatus.NearLimit;
                return LimitStatus.Safe;
            }
        }
    }
}
