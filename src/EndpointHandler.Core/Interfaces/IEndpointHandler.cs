namespace EndpointHandler.Core.Interfaces
{
    public interface IEndpointHandler<T1, TResponse>
    {
        TResponse Handle(T1 param1);
    }

    // These interfaces will mostly be used for queries
    public interface IEndpointHandler<TResponse>
    {
        TResponse Handle();
    }

    public interface IEndpointHandler<T1, T2, TResponse>
    {
        TResponse Handle(T1 param1, T2 param2);
    }

    public interface IEndpointHandler<T1, T2, T3, TResponse>
    {
        TResponse Handle(T1 param1, T2 param2, T3 param3);
    }

    public interface IEndpointHandler<T1, T2, T3, T4, TResponse>
    {
        TResponse Handle(T1 param1, T2 param2, T3 param3, T4 param4);
    }

    public interface IEndpointHandler<T1, T2, T3, T4, T5, TResponse>
    {
        TResponse Handle(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
    }
}
