using System;
using Food.Core.Domain;

namespace Food.Core.Requests
{
    public class MealListItemReadModel
    {
        public int Id { get; set; }

        public DateTime IntakeTime { get; set; }

        public NutritionFacts Facts { get; set; }
    }
}