using EndpointHandler.Core.Interfaces;

namespace EndpointHandler.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool ImplementsIEndpointHandler(this Type type)
        {
            return type.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Select(i => i.GetGenericTypeDefinition())
                    .Any(
                        i => i.Equals(typeof(IEndpointHandler<>)) ||
                        i.Equals(typeof(IEndpointHandler<,>)) || 
                        i.Equals(typeof(IEndpointHandler<,,>)) ||
                        i.Equals(typeof(IEndpointHandler<,,,>)) ||
                        i.Equals(typeof(IEndpointHandler<,,,,>))
                        );
        }
        public static Type? GetEndpointHandlerInterface(this Type type)
        {
            return type.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Select(i => i.GetGenericTypeDefinition())
                    .SingleOrDefault(
                        i => i.Equals(typeof(IEndpointHandler<>)) ||
                        i.Equals(typeof(IEndpointHandler<,>)) ||
                        i.Equals(typeof(IEndpointHandler<,,>)) ||
                        i.Equals(typeof(IEndpointHandler<,,,>)) ||
                        i.Equals(typeof(IEndpointHandler<,,,,>))
                        );
        }
    }
}
