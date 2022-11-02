using EndpointHandler.Core;
using FluentValidation;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Customer
{
    public class GetCustomer : EndpointHandlerBase<Guid, IResult>
    {
        private ICustomerService _service;

        public GetCustomer(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/customers/{id}")]
        [Tags("Customer")]
        public override IResult Handle(Guid id)
        {
            var dto = _service.GetById(id);

            return dto != null ? Results.Ok(dto) : Results.NotFound($"Could not find customer with id ({id})!");
        }

        protected override void Validate(Guid id)
        {
            var errors = new List<FluentValidation.Results.ValidationFailure>();

            if (id == Guid.Empty)
            {
                errors.Add(new FluentValidation.Results.ValidationFailure(nameof(id), $"Id is invalid!"));
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
