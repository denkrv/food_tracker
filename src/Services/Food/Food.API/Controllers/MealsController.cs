using System.Collections.Generic;
using System.Threading.Tasks;
using Food.Core.Requests;
using Food.Core.Requests.Meals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Food.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MealsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<PaginatedResult<MealListItemReadModel>> Get([FromQuery]GetMeals request)
        {
            var result = await _mediator.Send(request);
            return result;
        }


        [HttpGet("{id}")]
        public async Task<MealReadModel> Get(int id)
        {
            return await _mediator.Send(new GetMeal() { Id = id });
        }

        
        [HttpPost]
        public async Task Post([FromBody] CreateMeal command)
        {
            await _mediator.Send(command);
        }

        
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] EditMeal command)
        {
            await _mediator.Send(command);
        }


        [HttpPost("{id}/item")]
        public async Task PostItem(int id, [FromBody] CreateMealItem command)
        {
            command.MealId = id;
            await _mediator.Send(command);
        }


        [HttpDelete("{mealId:int}/item/{foodId:int}")]
        public async Task DeleteItem(int mealId, int foodId)
        {
            await _mediator.Send(new DeleteMealItem { MealId = mealId, FoodId = foodId });
        }


        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _mediator.Send(new DeleteMeal { Id = id });
        }
    }
}
