using EndpointHandler.Core.Interfaces;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace EndpointHandler.Core
{
    public abstract class EndpointHandlerBase<TResponse> : IEndpointHandler<TResponse>
    {
        public abstract TResponse Handle();
    }

    public abstract class EndpointHandlerBase<T1, TResponse> : IEndpointHandler<T1, TResponse>
    {
        private IValidator<T1>? _validator;

        public EndpointHandlerBase()
        {
        }

        public EndpointHandlerBase(IValidator<T1> validator)
        {
            _validator = validator;
        }

        public abstract TResponse Handle(T1 param1);

        protected virtual void Validate(T1 param1)
        {
            if (_validator != null)
            {
                var result = _validator.Validate(param1);

                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors);
                }
            }
        }
    }

    public abstract class EndpointHandlerBase<T1, T2, TResponse> : IEndpointHandler<T1, T2, TResponse>
    {
        public abstract TResponse Handle(T1 param1, T2 param2);
        protected abstract void Validate(T1 param1, T2 param2);
    }

    public abstract class EndpointHandlerBase<T1, T2, T3, TResponse> : IEndpointHandler<T1, T2, T3, TResponse>
    {
        public abstract TResponse Handle(T1 param1, T2 param2, T3 param3);
        internal abstract void Validate(T1 param1, T2 param2, T3 param3);
    }

    public abstract class EndpointHandlerBase<T1, T2, T3, T4, TResponse> : IEndpointHandler<T1, T2, T3, T4, TResponse>
    {
        public abstract TResponse Handle(T1 param1, T2 param2, T3 param3, T4 param4);
        internal abstract void Validate(T1 param1, T2 param2, T3 param3, T4 param4);
    }

    public abstract class EndpointHandlerBase<T1, T2, T3, T4, T5, TResponse> : IEndpointHandler<T1, T2, T3, T4, T5, TResponse>
    {
        public abstract TResponse Handle(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
        internal abstract void Validate(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
    }
}
