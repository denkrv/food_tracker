using System.Collections.Generic;
using Food.Core.Domain;
using MediatR;

namespace Food.Core.Requests.Recipes
{
    public class CreateDish : IRequest<int>
    {
        public string Name { get; set; }

        public NutritionFacts Facts { get; set; }
    }

    public class EditDish : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public NutritionFacts Facts { get; set; }
    }

    public class DeleteDish : IRequest
    {
        public int Id { get; set; }
    }


    public class GetDish : IRequest<RecipeReadModel>
    {
        public int Id { get; set; }
    }

    public class GetDishes : IRequest<IList<RecipeReadModel>>
    {
        public int Page { get; set; }
    }
}
