using MediatR;

namespace Food.Core.Requests.Foods
{

   

    public class GetFood: IRequest<FoodView>
    {
        public int Id { get; set; }
    }

    public class GetFoods : PaginatedRequest<FoodView>
    {
        public string Query { get; set; }
        
    }
}
