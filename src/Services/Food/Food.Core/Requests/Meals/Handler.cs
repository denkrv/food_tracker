using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Food.Core.Data;
using Food.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Requests.Meals
{
    public class MealsHandler : IRequestHandler<CreateMeal, int>,
        IRequestHandler<EditMeal>,
        IRequestHandler<DeleteMeal>,
        IRequestHandler<GetMeal, MealReadModel>,
        IRequestHandler<GetMeals, PaginatedResult<MealListItemReadModel>>
    {
        private readonly FoodContext _context;

        public MealsHandler(FoodContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateMeal request, CancellationToken cancellationToken)
        {
            var meal = new Meal()
            {
                IntakeTime = DateTime.UtcNow
            };
            
            await ReassignItems(meal, request.Items);
            
            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();
            return meal.Id;
        }

        public async Task<Unit> Handle(EditMeal request, CancellationToken cancellationToken)
        {
            var meal = await _context.Meals
                .Include(p=>p.Items)
                .FirstOrDefaultAsync(m=>m.Id == request.Id);

            await ReassignItems(meal, request.Items);

            await _context.SaveChangesAsync();
            return default;

        }

        private async Task ReassignItems(Meal meal, MealItemDto[] assigned)
        {
            var foodIds = assigned.Select(i => i.FoodId).ToHashSet();

            var existing = meal.Items.ToDictionary(i => i.FoodId);

            var foods = await _context.Foods.Where(f => foodIds.Contains(f.Id)).ToDictionaryAsync(i => i.Id);

            foreach (var item in assigned)
            {
                if (!foods.TryGetValue(item.FoodId, out var food))
                    throw new KeyNotFoundException($"ingridient or recipe  with id {item.FoodId} not found");

                if (!existing.TryGetValue(item.FoodId, out var mealItem))
                {

                    mealItem = new MealItem
                    {
                        Food = food,
                        FoodId = food.Id,
                        Meal = meal
                    };
                    meal.Items.Add(mealItem);
                }
                mealItem.Amount = item.Amount;
                mealItem.Facts = food.Facts * (item.Amount / 100);
            }

            foreach (var pair in existing)
            {
                if (!foodIds.Contains(pair.Key))
                {
                    meal.Items.Remove(pair.Value);
                }
            }

            meal.Facts = meal.Items.Aggregate(new NutritionFacts(), (c, i) => c + i.Facts);
            
        }

        public async Task<Unit> Handle(DeleteMeal request, CancellationToken cancellationToken)
        {
            var meal = await _context.Meals.FindAsync(request.Id);
            
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            return default;
        }

        public async  Task<MealReadModel> Handle(GetMeal request, CancellationToken cancellationToken)
        {
            var meal = await _context.Meals
                .Include(i=>i.Items).ThenInclude(i=>i.Food)
                .Where(i=>i.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            return new MealReadModel
            {
                Id = meal.Id,
                Facts = meal.Facts,
                IntakeTime = meal.IntakeTime,
                Items = meal.Items.Select(i=>new MealReadModel.Item
                {
                    Amount = i.Amount,
                    Facts = i.Facts,
                    Name = i.Food.Name,
                    FoodId = i.FoodId,
                    MealId = i.MealId
                }).ToArray()
            };
        }

        public async Task<PaginatedResult<MealListItemReadModel>> Handle(GetMeals request, CancellationToken cancellationToken)
        {
            var queryable = _context.Meals.AsNoTracking();


            if (request.Start.HasValue)
            {
                queryable = queryable.Where(m => m.IntakeTime >= request.Start.Value.ToUniversalTime());
            }
            
            if (request.End.HasValue)
            {
                queryable = queryable.Where(m => m.IntakeTime <= request.End.Value.ToUniversalTime());
            }

            return await queryable
                .Select(i => new MealListItemReadModel
                {
                    Id = i.Id,
                    IntakeTime = i.IntakeTime,
                    Facts = i.Facts
                })
                .CreatePaginatedResultAsync(request, cancellationToken);
        }
    }
}
