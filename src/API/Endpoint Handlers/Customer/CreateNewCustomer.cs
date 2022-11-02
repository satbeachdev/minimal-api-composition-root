using EndpointHandler.Core;
using FluentValidation;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;

namespace API.Endpoints.Customer
{
    public class CreateNewCustomer : EndpointHandlerBase<CreateCustomerCommand, IResult>
    {
        private ICustomerService _service;

        public CreateNewCustomer(ICustomerService service, IValidator<CreateCustomerCommand> validator) : base(validator)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/customers")]
        [Tags("Customer")]
        public override IResult Handle([FromBody] CreateCustomerCommand cmd)
        {
            // This method will throw an 
            // exception on a validation error
            Validate(cmd);

            var dto = _service.CreateNewCustomer(cmd);

            return Results.Ok(dto);
        }
    }
}
