using Application.Services;
using Repositories;
using Xunit;

namespace ApplicationTests
{
    public class GetCustomerHandlerTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var service = new CustomerService(new CustomerRepository());

            // Act
            var dto = service.GetById(Guid.NewGuid());

            // Assert
            Assert.NotNull(dto);
        }
    }
}
