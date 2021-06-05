using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Example.Api.Models;
using Example.Domain.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.Api.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(
            IMapper mapper,
            ILogger<CustomerController> logger,
            ICustomerService customerService)
        {
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                IList<Customer> customers = await _customerService.ListAsync();
                IList<CustomerModel> customerModels = _mapper.Map<IList<CustomerModel>>(customers);
                return Ok(customerModels);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route(("{id:int}"))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Customer customer = await _customerService.GetAsync(id);

                if (customer == null)
                    return NotFound();

                CustomerModel customerModel = _mapper.Map<CustomerModel>(customer);
                return Ok(customerModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CustomerModel customerModel)
        {
            try
            {
                Customer newCustomer = _mapper.Map<Customer>(customerModel);
                newCustomer = await _customerService.CreateAsync(newCustomer);
                CustomerModel newCustomerModel = _mapper.Map<CustomerModel>(newCustomer);

                return CreatedAtAction(nameof(GetById), new {id = newCustomerModel.Id}, newCustomerModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route(("{id:int}"))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerModel customerModel)
        {
            try
            {
                Customer existingCustomer = await _customerService.GetAsync(id);

                if (existingCustomer == null)
                    return NotFound();

                Customer updateCustomer = _mapper.Map<Customer>(customerModel);
                updateCustomer.Id = id;
                updateCustomer = await _customerService.UpdateAsync(updateCustomer);

                CustomerModel updatedCustomerModel = _mapper.Map<CustomerModel>(updateCustomer);
                return Ok(updatedCustomerModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route(("{id:int}"))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Customer existingCustomer = await _customerService.GetAsync(id);

                if (existingCustomer == null)
                    return NotFound();

                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
    }
}
