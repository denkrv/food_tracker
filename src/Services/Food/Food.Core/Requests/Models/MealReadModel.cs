using System;
using Food.Core.Domain;

namespace Food.Core.Requests
{
    public class MealReadModel
    {
        public int Id { get; set; }

        public DateTime IntakeTime { get; set; }

        public NutritionFacts Facts { get; set; }

        public Item[] Items { get; set; }

        public class Item
        {
            public int MealId { get; set; }

            public int FoodId { get; set; }

            public string Name { get; set; }

            public NutritionFacts Facts { get; set; }

            public decimal Amount { get; set; }
        }
    }
}