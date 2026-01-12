using CosmosCustomerApi.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosCustomerApi.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<Customer> GetCustomerAsync(string customerId)
    {
        try
        {
            var response = await _container.ReadItemAsync<Customer>(customerId, new PartitionKey(customerId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        }
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        var query = _container.GetItemQueryIterator<Customer>(
            new QueryDefinition("SELECT * FROM c"));

        var results = new List<Customer>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        // Ensure CustomerId is set - if not provided, use the auto-generated Id
        // This allows the same value to be used as both the document id and partition key
        if (string.IsNullOrEmpty(customer.CustomerId))
        {
            customer.CustomerId = customer.Id;
        }

        // Set creation timestamp if not already set
        if (customer.AccountCreatedDate == default(DateTime))
        {
            customer.AccountCreatedDate = DateTime.UtcNow;
        }

        var response = await _container.CreateItemAsync(customer, new PartitionKey(customer.CustomerId));
        return response.Resource;
    }

    public async Task<Customer> UpdateCustomerAsync(string customerId, Customer customer)
    {
        try
        {
            // Ensure both Id and CustomerId match for document consistency
            customer.Id = customerId;
            customer.CustomerId = customerId;

            // Use ReplaceItemAsync to update only if exists
            var response = await _container.ReplaceItemAsync(customer, customerId, new PartitionKey(customerId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        }
    }

    public async Task DeleteCustomerAsync(string customerId)
    {
        try
        {
            await _container.DeleteItemAsync<Customer>(customerId, new PartitionKey(customerId));
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        }
    }
}
