using EndpointHandler.Core;
using FluentValidation;
using FluentValidation.Results;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace API.Endpoints.Customer
{
    public class GetAllCustomers : EndpointHandlerBase<int?, int?, IResult>
    {
        private ICustomerService _service;

        public GetAllCustomers(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("/customers")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Tags("Customer")]
        public override IResult Handle([FromQuery(Name = "offset")]int? offset, [FromQuery(Name = "count")]int? count)
        {
            Validate(offset, count);

            var dtos = _service.GetAll();

            return dtos != null ? Results.Ok(dtos) : Results.NoContent();
        }

        protected override void Validate(int? offset, int? count)
        {
            var errors = new List<ValidationFailure>();

            if (offset < 0)
            {
                errors.Add(new ValidationFailure(nameof(offset), $"Offset cannot be negative!"));
            }

            if (count < 1)
            {
                errors.Add(new ValidationFailure(nameof(count), $"Count must be greater than zero!"));
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
