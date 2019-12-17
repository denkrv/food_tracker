using System;
using MediatR;

namespace Food.Core.Requests.Meals
{
    public class CreateMeal : IRequest<int>
    {
        public MealItemDto[] Items { get; set; } = Array.Empty<MealItemDto>();

    }

    public class CreateMealItem : IRequest
    {
        public int MealId { get; set; }

        public int FoodId { get; set; }

        public decimal Amount { get; set; }

    }

    public class DeleteMealItem : IRequest
    {
        public int MealId { get; set; }

        public int FoodId { get; set; }
    }

    public class EditMealItem : IRequest
    {
        public int MealId { get; set; }
        
        public int FoodId { get; set; }

        public decimal Amount { get; set; }

    }

    public class EditMeal : IRequest
    {
        public int Id { get; set; }

        public MealItemDto[] Items { get; set; } = Array.Empty<MealItemDto>();
    }

    public class DeleteMeal : IRequest
    {
        public int Id { get; set; }
    }


    public class GetMeal : IRequest<MealReadModel>
    {
        public int Id { get; set; }
    }

    public class GetMeals : PaginatedRequest<MealListItemReadModel>
    {
        public DateTime? Start { get; set; }
        
        public DateTime? End { get; set; }
    }
}
