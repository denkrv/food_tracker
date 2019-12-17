using Food.Core.Domain;
using MediatR;

namespace Food.Core.Requests.Products
{
    public class CreateProduct : IRequest<int>
    {
        public string Name { get; set; }

        public NutritionFacts Facts { get; set; }
    }

    public class EditProduct : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public NutritionFacts Facts { get; set; }
    }

    public class DeleteProduct : IRequest
    {
        public int Id { get; set; }
    }


    public class GetProduct : IRequest<ProductReadModel>
    {
        public int Id { get; set; }
    }

    public class GetProducts : PaginatedRequest<ProductReadModel>
    {
    }
}
