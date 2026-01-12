using CosmosCustomerApi.Models;
using CosmosCustomerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CosmosCustomerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICosmosDbService cosmosDbService, ILogger<CustomersController> logger)
    {
        _cosmosDbService = cosmosDbService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        try
        {
            var customers = await _cosmosDbService.GetCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return StatusCode(500, "An error occurred while retrieving customers.");
        }
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<Customer>> GetCustomer(string customerId)
    {
        try
        {
            var customer = await _cosmosDbService.GetCustomerAsync(customerId);
            return Ok(customer);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Customer with ID {customerId} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while retrieving the customer.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
    {
        try
        {
            var createdCustomer = await _cosmosDbService.CreateCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { customerId = createdCustomer.CustomerId }, createdCustomer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, "An error occurred while creating the customer.");
        }
    }

    [HttpPut("{customerId}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(string customerId, [FromBody] Customer customer)
    {
        try
        {
            var updatedCustomer = await _cosmosDbService.UpdateCustomerAsync(customerId, customer);
            return Ok(updatedCustomer);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Customer with ID {customerId} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while updating the customer.");
        }
    }

    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
        try
        {
            await _cosmosDbService.DeleteCustomerAsync(customerId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Customer with ID {customerId} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {CustomerId}", customerId);
            return StatusCode(500, "An error occurred while deleting the customer.");
        }
    }
}
