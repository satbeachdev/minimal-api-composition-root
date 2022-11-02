using Models.Domain;

namespace Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private IDictionary<Guid, Customer> _customers = new Dictionary<Guid, Customer>()
        {
            {Guid.Parse("47584949-15b0-4270-9b3e-3cb697af3561"), new Customer(Guid.Parse("47584949-15b0-4270-9b3e-3cb697af3561"), "IBM", "120 W. Big Blue St.", "Armonk", "NorthEast", "NY", "093832", "USA") },
            {Guid.Parse("5bc4942a-a832-4894-b232-82a54ea495ec"), new Customer(Guid.Parse("5bc4942a-a832-4894-b232-82a54ea495ec"), "Microsoft", "1 Microsoft Way", "Bellevue", "NorthWest", "WA", "843822", "USA") },
        };

        public IList<Customer> GetAll()
        {
            return _customers.Values.ToList();
        }

        public Customer? GetById(Guid id)
        {
            if (_customers.ContainsKey(id))
            {
                return _customers[id];
            }

            return null;
        }

        public Customer? CreateNew(Customer newCustomer)
        {
            _customers.Add(newCustomer.Id, newCustomer);

            return newCustomer;
        }

        public Customer? DeleteById(Guid id)
        {
            var customer = default(Customer);

            if (_customers.ContainsKey(id))
            {
                customer = _customers[id];

                _customers.Remove(id);
            }

            return customer;
        }

        public Customer? Update(Customer updatedCustomer)
        {
            if (_customers.ContainsKey(updatedCustomer.Id))
            {
                _customers[updatedCustomer.Id] = updatedCustomer;
            }

            return updatedCustomer;
        }
    }
}
