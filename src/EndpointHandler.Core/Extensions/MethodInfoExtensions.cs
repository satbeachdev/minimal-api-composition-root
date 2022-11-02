using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using EndpointHandler.Core.Enums;

namespace EndpointHandler.Core.Extensions
{
    internal static class MethodInfoExtensions
    {
        internal static EndpointVerbs? GetHttpVerbFromAttribute(this MethodInfo method)
        {
            var httpMethodAttrib = method.GetCustomAttribute(typeof(HttpMethodAttribute), true);

            EndpointVerbs? verb = default;

            if (httpMethodAttrib != null)
            {
                var httpMethodType = httpMethodAttrib.GetType();

                if (httpMethodType.IsAssignableTo(typeof(HttpGetAttribute)))
                {
                    verb = EndpointVerbs.GET;
                }

                if (httpMethodType.IsAssignableTo(typeof(HttpPostAttribute)))
                {
                    verb = EndpointVerbs.POST;
                }

                if (httpMethodType.IsAssignableTo(typeof(HttpPutAttribute)))
                {
                    verb = EndpointVerbs.PUT;
                }

                if (httpMethodType.IsAssignableTo(typeof(HttpDeleteAttribute)))
                {
                    verb = EndpointVerbs.DELETE;
                }
            }

            return verb;
        }

        internal static string? GetRouteFromAttribute(this MemberInfo method)
        {
            var routeAttrib = (RouteAttribute)method.GetCustomAttribute(typeof(RouteAttribute));

            string? route = null;

            if (routeAttrib != null)
            {
                route = routeAttrib.Template;
            }

            return route;
        }

        internal static Delegate CreateRouteHandlerDelegate(this MethodInfo method, object handler)
        {
            var functionParameters = method.GetParameters().Select(p => p.ParameterType).ToList();

            functionParameters.Add(method.ReturnType);

            var delegateType = Expression.GetDelegateType(functionParameters.ToArray());

            return method.CreateDelegate(delegateType, handler);
        }
    }
}
