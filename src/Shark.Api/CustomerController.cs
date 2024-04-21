using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shark.Domain.CustomerManagement;
using Shark.Application.CustomerManagement;

namespace Shark.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(IMediator mediator
        ,ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDto customer)
        {
            try{
                var command = new InsertCustomerCommand(customer);
                await _mediator.Send(command);
                return Ok();
            }
            catch(Exception ex){
                _logger.LogError("exception throwed when trying to create customer {ex}",ex);
                return StatusCode(500);
            }
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, CustomerDto customer)
        {
            if (customerId != customer.CustomerId)
            {
                return BadRequest();
            }

            // var command = new UpdateCustomerCommand { Customer = customer };
            // await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            throw new NotImplementedException();
            // var query = new GetCustomersQuery { PageNumber = pageNumber, PageSize = pageSize };
            // var customers = await _mediator.Send(query);
            // return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            throw new NotImplementedException();
            // var query = new GetCustomerByIdQuery { CustomerId = customerId };
            // var customer = await _mediator.Send(query);
            // if (customer == null)
            // {
            //     return NotFound();
            // }
            // return Ok(customer);
        }
    }
}
