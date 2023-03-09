using System.Runtime.Serialization;

namespace HM.Ioc;

[Serializable]
public class ServiceNotFoundException : Exception
{
    public string ServiceName { get; } = string.Empty;

    public ServiceNotFoundException()
        : this(string.Empty, string.Empty, null!) { }

    public ServiceNotFoundException(string serviceName)
        : this(serviceName, $"Requested service `{serviceName}` not found", null!) { }

    public ServiceNotFoundException(string serviceName, string message)
        : this(serviceName, message, null!) { }

    public ServiceNotFoundException(string serviceName, string message, Exception inner) : base(message, inner)
    {
        ServiceName = serviceName;
    }

    #region NonPublic
    protected ServiceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endregion
}
