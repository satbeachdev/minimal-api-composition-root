namespace Models.Commands
{
    public record UpdateCustomerCommand(Guid Id, string Name, string Address, string City, string Region, string PostalCode, string Country);
}
