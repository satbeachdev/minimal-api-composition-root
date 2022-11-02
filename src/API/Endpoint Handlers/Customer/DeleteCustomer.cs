using EndpointHandler.Core;
using FluentValidation;
using FluentValidation.Results;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using Models.DTOs;

namespace API.Endpoints.Customer
{
    public class DeleteCustomer : EndpointHandlerBase<Guid, DeleteCustomerCommand, IResult>
    {
        private ICustomerService _service;
        private IValidator<DeleteCustomerCommand> _validator;

        public DeleteCustomer(ICustomerService service, IValidator<DeleteCustomerCommand> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpDelete]
        [Route("/customers/{id}")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Customer")]
        public override IResult Handle(Guid id, [FromBody] DeleteCustomerCommand cmd)
        {
            Validate(id, cmd);

            var dto = _service.DeleteCustomer(cmd);

            return dto != null ? Results.Ok(dto) : Results.NotFound($"Could not find customer with id ({cmd.CustomerId})!");
        }

        protected override void Validate(Guid id, DeleteCustomerCommand cmd)
        {
            var errors = new List<ValidationFailure>();

            // Validate id
            var dto = _service.GetById(id);

            if (dto == null)
            {
                errors.Add(new ValidationFailure(nameof(id), $"A customer with the specified Id ({id}) was not found!"));
            }

            var results = _validator.Validate(cmd);

            if (!results.IsValid)
            {
                errors.AddRange(results.Errors);
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
