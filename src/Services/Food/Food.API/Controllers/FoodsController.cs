using System.Collections.Generic;
using System.Threading.Tasks;
using Food.Core.Requests;
using Food.Core.Requests.Foods;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Food.API.Controllers
{
    [Route("api/v1/foods")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PaginatedResult<FoodView>> Get([FromQuery]GetFoods request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
    }
}