using Microsoft.AspNetCore.Mvc;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Server.Controllers
{
    /// <summary>
    /// CustomerController is responsible for handling customer-related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IGenericService<CustomerModel, string> _customerService;

        public CustomerController(ILogger<CustomerController> logger, IGenericService<CustomerModel, string> customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        /// <summary>
        /// Gets a list of all customers.
        /// </summary>
        /// <returns>A list of CustomerModel objects.</returns>
        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAllCustomers()
        {
            try
            {
                return Ok(await _customerService.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customers.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a specific customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>An CustomerModel object.</returns>
        [HttpGet("GetCustomer/{id}", Name = "GetCustomer")]
        public async Task<ActionResult<CustomerModel>> GetCustomer(string id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting customer with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">The CustomerModel object to create.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPost("CreateCustomer")]
        public async Task<ActionResult> CreateCustomer(CustomerModel customer)
        {
            try
            {
                await _customerService.AddAsync(customer);
                return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an customer.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated CustomerModel object.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("UpdateCustomer/{id}", Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(string id, CustomerModel customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                await _customerService.UpdateAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating customer with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an existing customer.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("DeleteCustomer/{id}", Name = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting customer with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
