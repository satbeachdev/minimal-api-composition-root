using Models.Commands;
using Models.DTOs;

namespace Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<CustomerDto> GetAll();
        CustomerDto? GetById(Guid id);
        CustomerDto CreateNewCustomer(CreateCustomerCommand cmd);
        CustomerDto? UpdateCustomer(UpdateCustomerCommand cmd);
        CustomerDto? DeleteCustomer(DeleteCustomerCommand cmd);
    }
}