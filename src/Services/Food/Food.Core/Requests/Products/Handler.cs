using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Food.Core.Data;
using Food.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Requests.Products
{
    public class IngridientsHandler : IRequestHandler<CreateProduct, int>,
        IRequestHandler<EditProduct>,
        IRequestHandler<DeleteProduct>,
        IRequestHandler<GetProduct, ProductReadModel>,
        IRequestHandler<GetProducts, PaginatedResult<ProductReadModel>>
    {
        private readonly FoodContext _context;

        public IngridientsHandler(FoodContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var ingridient = new Product();
            ingridient.Name = request.Name;
            ingridient.Facts = request.Facts;

            _context.Products.Add(ingridient);
            await _context.SaveChangesAsync();
            return ingridient.Id;
        }

        public async Task<Unit> Handle(EditProduct request, CancellationToken cancellationToken)
        {
            var ingridient = await _context.Products.FindAsync(request.Id);
            ingridient.Name = request.Name;
            ingridient.Facts = request.Facts;

            await _context.SaveChangesAsync();
            return default;

        }

        public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var ingridient = await _context.Products.FindAsync(request.Id);
            _context.Products.Remove(ingridient);
            await _context.SaveChangesAsync();
            return default;
        }

        public async  Task<ProductReadModel> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            var result = await _context.Products.AsNoTracking()
                .Where(i=>i.Id == request.Id)
                .Select(i => new ProductReadModel
            {
                Id = i.Id,
                Name = i.Name,
                Facts = i.Facts
            }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<PaginatedResult<ProductReadModel>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var queryable = _context.Products.AsNoTracking().Select(i=> new ProductReadModel
            {
                Id = i.Id,
                Name = i.Name,
                Facts = i.Facts
            });

            return await queryable.CreatePaginatedResultAsync(request);
        }
    }
}
