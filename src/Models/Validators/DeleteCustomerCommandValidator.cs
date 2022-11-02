using FluentValidation;
using Models.Commands;

namespace Models.Validators
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEqual(Guid.Empty);
        }
    }
}
