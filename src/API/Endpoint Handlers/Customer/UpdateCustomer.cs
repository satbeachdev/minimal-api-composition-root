using EndpointHandler.Core;
using FluentValidation;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;

namespace API.Endpoint_Handlers.Customer
{
    public class UpdateCustomer : EndpointHandlerBase<Guid, UpdateCustomerCommand, IResult>
    {
        private ICustomerService _service;
        private IValidator<UpdateCustomerCommand> _validator;

        public UpdateCustomer(ICustomerService service, IValidator<UpdateCustomerCommand> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpPut]
        [Route("/customers/{id}")]
        [Tags("Customer")]
        public override IResult Handle(Guid id, [FromBody] UpdateCustomerCommand cmd)
        {
            Validate(id, cmd);

            var dto = _service.UpdateCustomer(cmd);

            return dto != null ? Results.Ok(dto) : Results.NotFound($"Could not find customer with id ({cmd.Id})!");
        }

        protected override void Validate(Guid id, UpdateCustomerCommand cmd)
        {
            // Validate id
            var dto = _service.GetById(id);

            if (dto == null)
            {
                throw new ValidationException($"A customer with the specified Id ({id}) was not found!");
            }

            _validator.ValidateAndThrow(cmd);
        }
    }
}
