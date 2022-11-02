namespace Models.DTOs
{
    public record CustomerDto(Guid Id, string Name, string Address, string City, string Region, string PostalCode, string Country);
}
