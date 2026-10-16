namespace AlcoholTracker
{
    /// <summary>Описание типа напитка: стандартная порция и крепость.</summary>
    public class Drink
    {
        // Плотность этанола, г/мл
        private const double EthanolDensity = 0.789;

        public string Name { get; private set; }
        public int ServingMl { get; private set; }
        public double AlcoholPercent { get; private set; }

        public Drink(string name, int servingMl, double alcoholPercent)
        {
            Name = name;
            ServingMl = servingMl;
            AlcoholPercent = alcoholPercent;
        }

        /// <summary>Граммы чистого спирта в одной порции.</summary>
        public double GramsPerServing
        {
            get { return ServingMl * (AlcoholPercent / 100.0) * EthanolDensity; }
        }

        /// <summary>Типичные напитки: пиво 0,5 л 5%, вино 150 мл 12%, водка 50 мл 40%.</summary>
        public static readonly Drink[] Defaults =
        {
            new Drink("Пиво", 500, 5.0),
            new Drink("Вино", 150, 12.0),
            new Drink("Водка", 50, 40.0)
        };
    }
}
