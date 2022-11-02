namespace Models.Commands
{
    public record CreateCustomerCommand(string Name, string Address, string City, string Region, string PostalCode, string Country);
}
