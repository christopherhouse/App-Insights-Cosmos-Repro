using CosmosCustomerApi.Models;

namespace CosmosCustomerApi.Services;

public interface ICosmosDbService
{
    Task<Customer> GetCustomerAsync(string customerId);
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(string customerId, Customer customer);
    Task DeleteCustomerAsync(string customerId);
}
