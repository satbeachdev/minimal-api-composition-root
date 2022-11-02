namespace Models.Domain
{
    public record Customer(Guid Id, string Name, string Address, string City, string Region, string State, string PostalCode, string Country);
}
