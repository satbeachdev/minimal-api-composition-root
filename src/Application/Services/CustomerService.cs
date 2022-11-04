using Interfaces;
using Models.Commands;
using Models.Domain;
using Models.DTOs;
using Repositories;
using System.Diagnostics;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _repository;
        private ActivitySource _activitySource;

        public CustomerService(IRepository<Customer> repository, ActivitySource activitySource)
        {
            _repository = repository;
            _activitySource = new ActivitySource(nameof(CustomerService)); // activitySource;
        }

        public IEnumerable<CustomerDto>? GetAll()
        {
            using var a = _activitySource.StartActivity("Get all customers");

            // Do DB stuff here (using the injected repository)
            var customers = _repository.GetAll();

            return customers != null ? customers.Select(c => new CustomerDto(c.Id, c.Name, c.Address, c.City, c.Region, c.PostalCode, c.Country)).ToArray() : null;
        }

        public CustomerDto? GetById(Guid id)
        {
            using var a = _activitySource.StartActivity("Get a specific customer by Id");
            a.AddTag("customerId", id.ToString());

            // Do DB stuff here (using the injected repository)
            var customer = _repository.GetById(id);

            return customer != null ? new CustomerDto(id, customer.Name, customer.Address, customer.City, customer.Region, customer.PostalCode, customer.Country) : null;
        }

        public CustomerDto CreateNewCustomer(CreateCustomerCommand cmd)
        {
            using var a = _activitySource.StartActivity("Create a new customer");

            var newCustomer = new Customer(Guid.NewGuid(), cmd.Name, cmd.Address, cmd.City, "Region found by lookup", cmd.PostalCode, "State found by lookup", "Country found by lookup");

            // Do DB stuff here (using the injected repository)
            var customer = _repository.CreateNew(newCustomer);

            // Transform customer to customerDto
            // dto = customer.ToDto();

            return new CustomerDto(customer.Id, customer.Name, customer.Address, customer.City, customer.Region, customer.PostalCode, customer.Country); 
        }

        public CustomerDto? UpdateCustomer(UpdateCustomerCommand cmd)
        {
            using var a = _activitySource.StartActivity("Update a specific customer");
            a.AddTag("customerId", cmd.Id.ToString());

            var customer = _repository.GetById(cmd.Id);

            if (customer != null)
            {
                var updatedCustomer = new Customer(customer.Id, cmd.Name, cmd.Address, cmd.City, cmd.Region, customer.State, customer.PostalCode, cmd.Country);

                // Do DB stuff here (using the injected repository)
                customer = _repository.Update(updatedCustomer);
            }

            return new CustomerDto(customer.Id, customer.Name, customer.Address, customer.City, customer.Region, customer.PostalCode, customer.Country);
        }

        public CustomerDto? DeleteCustomer(DeleteCustomerCommand cmd)
        {
            using var a = _activitySource.StartActivity("Delete a specific customer");
            a.AddTag("customerId", cmd.CustomerId.ToString());

            var dto = GetById(cmd.CustomerId);

            if (dto != null)
            {
                // Do DB stuff here (using the injected repository)
                var customer = _repository.DeleteById(cmd.CustomerId);

                // Transform customer to customerDto
                // dto = customer.ToDto();
            }

            return dto;
        }
    }
}
