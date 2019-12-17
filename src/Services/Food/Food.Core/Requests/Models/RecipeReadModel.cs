using Food.Core.Domain;

namespace Food.Core.Requests
{
    public class RecipeReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public NutritionFacts Facts { get; set; }
    }
}