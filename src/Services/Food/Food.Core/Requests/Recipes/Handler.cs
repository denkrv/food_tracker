using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Food.Core.Data;
using Food.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Requests.Recipes
{
    public class DishesHandler : IRequestHandler<CreateDish, int>,
        IRequestHandler<EditDish>,
        IRequestHandler<DeleteDish>,
        IRequestHandler<GetDish, RecipeReadModel>,
        IRequestHandler<GetDishes, IList<RecipeReadModel>>
    {
        private readonly FoodContext _context;

        public DishesHandler(FoodContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateDish request, CancellationToken cancellationToken)
        {
            var dish = new Recipe();
            dish.Name = request.Name;
            dish.Facts = request.Facts;

            _context.Recipes.Add(dish);
            await _context.SaveChangesAsync(cancellationToken);
            return dish.Id;
        }

        public async Task<Unit> Handle(EditDish request, CancellationToken cancellationToken)
        {
            var dish = await _context.Recipes.FindAsync(request.Id, cancellationToken);
            dish.Name = request.Name;
            dish.Facts = request.Facts;

            await _context.SaveChangesAsync(cancellationToken);
            return default;

        }

        public async Task<Unit> Handle(DeleteDish request, CancellationToken cancellationToken)
        {
            var dish = await _context.Recipes.FindAsync(request.Id, cancellationToken);
            _context.Recipes.Remove(dish);
            await _context.SaveChangesAsync(cancellationToken);
            return default;
        }

        public async  Task<RecipeReadModel> Handle(GetDish request, CancellationToken cancellationToken)
        {
            var result = await _context.Recipes.AsNoTracking()
                .Where(i=>i.Id == request.Id)
                .Select(i => new RecipeReadModel
                {
                Id = i.Id,
                Name = i.Name,
                Facts = i.Facts
            }).FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        public async Task<IList<RecipeReadModel>> Handle(GetDishes request, CancellationToken cancellationToken)
        {
            var result = await _context.Recipes.AsNoTracking().Select(i=> new RecipeReadModel
            {
                Id = i.Id,
                Name = i.Name,
                Facts = i.Facts
            }).ToListAsync(cancellationToken);
            return result;
        }
    }
}
