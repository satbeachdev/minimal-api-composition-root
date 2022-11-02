using EndpointHandler.Core.Enums;

namespace EndpointHandler.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EndpointLifetimeAttribute : Attribute
    {
        public EndpointLifetimes Lifetime { get; private set; }
        public EndpointLifetimeAttribute(EndpointLifetimes lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
