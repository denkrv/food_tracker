using System;

namespace Food.Core.Domain
{
    //todo: make immutable
    public class NutritionFacts 
    {
        public decimal Protein { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Fat { get; set; }

        public decimal Calories { get; set; }

        public static NutritionFacts operator +(NutritionFacts a, NutritionFacts b) =>
            new NutritionFacts
            {
                Calories = a.Calories + b.Calories,
                Carbohydrates = a.Carbohydrates + b.Carbohydrates,
                Protein = a.Protein + b.Protein,
                Fat = a.Fat + b.Fat
            };

        public static NutritionFacts operator *(NutritionFacts a, decimal multiplier)
        {
            if (multiplier < 0) throw new ArgumentException(nameof(multiplier));

            return new NutritionFacts
            {
                Calories = a.Calories * multiplier,
                Carbohydrates = a.Carbohydrates * multiplier,
                Protein = a.Protein * multiplier,
                Fat = a.Fat * multiplier
            };
        }

        public static NutritionFacts operator /(NutritionFacts a, decimal divisor)
        {
            if (divisor <= decimal.Zero) throw new ArgumentException(nameof(divisor));

            return new NutritionFacts
            {
                Calories = a.Calories / divisor,
                Carbohydrates = a.Carbohydrates / divisor,
                Protein = a.Protein / divisor,
                Fat = a.Fat / divisor
            };
        }

    }
}