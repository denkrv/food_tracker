using Food.Core.Domain;

namespace Food.Core.Requests
{
    public class FoodView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public NutritionFacts Facts { get; set; }
    }

    //public class PaginatedResult<T>
    //{

    //    public int CurrentPage { get; }

    //    public int TotalCount { get; }

    //    public static PaginatedResult<T> Create(IQueryable<T> queryable)
    //    {
    //        return new PaginatedResult<T>();

    //    }
    //}
}