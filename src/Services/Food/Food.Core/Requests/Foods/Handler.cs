using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Food.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Requests.Foods
{
    public class FoodHandler :
        IRequestHandler<GetFood, FoodView>,
        IRequestHandler<GetFoods, PaginatedResult<FoodView>>
    {
        private readonly FoodContext _context;

        public FoodHandler(FoodContext context)
        {
            _context = context;
        }


        public async Task<FoodView> Handle(GetFood request, CancellationToken cancellationToken)
        {
            var food = await _context.Foods
                .Where(i => i.Id == request.Id).FirstOrDefaultAsync();

            return new FoodView
            {
                Id = food.Id,
                Name = food.Name,
                Facts = food.Facts,
            };
        }

        public async Task<PaginatedResult<FoodView>> Handle(GetFoods request, CancellationToken cancellationToken)
        {
            var queryable = _context.Foods.AsNoTracking();

            if (request.Query?.Length >= 2)
            {
                var query = GenerateQuery(request.Query);
                queryable = queryable.Where(f => f.SearchVector.Matches(EF.Functions.ToTsQuery("russian", query)))
                    .OrderByDescending(f => f.SearchVector.Rank(EF.Functions.ToTsQuery("russian", query), NpgsqlTsRankingNormalization.DivideByUniqueWordCount));

            }

            return await queryable.Select(food => new FoodView
            {
                Id = food.Id,
                Name = food.Name,
                Facts = food.Facts
            })
            .CreatePaginatedResultAsync(request);
        }

        private Regex removeChars = new Regex(@":|;|!|@|#|\&|\+|\?", RegexOptions.Compiled);

        private string GenerateQuery(string query)
        {
            var terms = removeChars
                .Replace(query, string.Empty)
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" & ", terms.Select(term => term + ":*"));
        }

    }
}
