using System.Reflection;
using EndpointHandler.Core.Attributes;
using EndpointHandler.Core.Enums;

namespace EndpointHandler.Core.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddEndpointHandlersFromAssembly(this WebApplicationBuilder builder, Assembly endpointHandlerAssembly)
        {
            foreach (var type in endpointHandlerAssembly.GetTypes())
            {
                if (!type.IsAbstract && type.ImplementsIEndpointHandler())
                {
                    var epl = type.GetCustomAttribute<EndpointLifetimeAttribute>();

                    _ = epl?.Lifetime switch
                    {
                        EndpointLifetimes.Scoped => builder.Services.AddScoped(type),
                        EndpointLifetimes.Singleton => builder.Services.AddSingleton(type),
                        EndpointLifetimes.Transient => builder.Services.AddTransient(type),
                        _ => builder.Services.AddTransient(type),
                    };
                }
            }
        }
    }
}
