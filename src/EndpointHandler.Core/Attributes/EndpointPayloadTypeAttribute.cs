namespace EndpointHandler.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EndpointPayloadTypeAttribute : Attribute
    {
        public Type ContentType { get; private set; }

        public EndpointPayloadTypeAttribute(Type contentType)
        {
            ContentType = contentType;
        }
    }
}
