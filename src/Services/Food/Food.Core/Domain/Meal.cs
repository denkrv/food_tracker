using System;
using System.Collections.Generic;

namespace Food.Core.Domain
{
    public class MealItem
    {
        public int MealId { get; set; }

        public int FoodId { get; set; }

        public Meal Meal { get; set; }

        public Food Food { get; set; }

        public decimal Amount { get; set; }
        
        public NutritionFacts Facts { get; set; }

    }

    public class Meal : IEnitity
    {
        public int Id { get; set; }
        
        public DateTime IntakeTime { get; set; }

        public ICollection<MealItem> Items { get; private set; } = new List<MealItem>();

        public NutritionFacts Facts { get; set; }
        
    }
}