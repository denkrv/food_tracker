using System.Collections.Generic;
using System.Threading.Tasks;
using Food.Core.Requests;
using Food.Core.Requests.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Food.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
       
        [HttpGet]
        public async Task<PaginatedResult<ProductReadModel>> Get([FromQuery]GetProducts request)
        {
            var result = await _mediator.Send(request);
            return result;
        }
        
        [HttpGet("{id}")]
        public async Task<ProductReadModel> Get(int id)
        {
            return await _mediator.Send(new GetProduct() { Id = id });
        }
        
        [HttpPost]
        public async Task Post([FromBody] CreateProduct command)
        {
            await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] EditProduct command)
        {
            await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _mediator.Send(new DeleteProduct { Id = id });
        }
    }
}
