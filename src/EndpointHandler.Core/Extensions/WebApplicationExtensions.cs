using System.Reflection;
using EndpointHandler.Core.Enums;

namespace EndpointHandler.Core.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            foreach (var type in Assembly.GetCallingAssembly().GetTypes())
            {
                var handlerInterface = type.GetEndpointHandlerInterface();

                if (!type.IsAbstract && handlerInterface != null)
                {
                    // Should only be the Handle method being returned
                    var methods = handlerInterface.GetMethods(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                    if (methods?.Length == 1)
                    {
                        var method = type.GetMethod(methods[0].Name);
                        var handler = app.Services.CreateScope().ServiceProvider.GetService(type);

                        if (method != null && handler != null)
                        {
                            var verb = method.GetHttpVerbFromAttribute();
                            var route = method.GetRouteFromAttribute();

                            if (verb != null && route != null)
                            {
                                _ = verb switch
                                {
                                    EndpointVerbs.GET => app.MapGet(route, method.CreateRouteHandlerDelegate(handler)),
                                    EndpointVerbs.POST => app.MapPost(route, method.CreateRouteHandlerDelegate(handler)),
                                    EndpointVerbs.PUT => app.MapPut(route, method.CreateRouteHandlerDelegate(handler)),
                                    EndpointVerbs.DELETE => app.MapDelete(route, method.CreateRouteHandlerDelegate(handler)),
                                    _ => throw new NotImplementedException()
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}
