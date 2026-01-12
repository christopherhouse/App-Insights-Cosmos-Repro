using Newtonsoft.Json;

namespace CosmosCustomerApi.Models;

public class Customer
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonProperty("address")]
    public Address Address { get; set; } = new();

    [JsonProperty("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [JsonProperty("accountCreatedDate")]
    public DateTime AccountCreatedDate { get; set; } = DateTime.UtcNow;

    [JsonProperty("isActive")]
    public bool IsActive { get; set; } = true;

    [JsonProperty("loyaltyPoints")]
    public int LoyaltyPoints { get; set; }

    [JsonProperty("totalOrderValue")]
    public decimal TotalOrderValue { get; set; }
}

public class Address
{
    [JsonProperty("street")]
    public string Street { get; set; } = string.Empty;

    [JsonProperty("city")]
    public string City { get; set; } = string.Empty;

    [JsonProperty("state")]
    public string State { get; set; } = string.Empty;

    [JsonProperty("postalCode")]
    public string PostalCode { get; set; } = string.Empty;

    [JsonProperty("country")]
    public string Country { get; set; } = string.Empty;
}
