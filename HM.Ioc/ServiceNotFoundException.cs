using System.Runtime.Serialization;

namespace HM.Ioc;

[Serializable]
public class ServiceNotFoundException : Exception
{
    public string ServiceName { get; } = string.Empty;

    public ServiceNotFoundException()
    {

    }
    public ServiceNotFoundException(string serviceName) : base($"Requested service `{serviceName}` not found")
    {
        ServiceName = serviceName;
    }
    public ServiceNotFoundException(string serviceName, string message) : base(message)
    {
        ServiceName = serviceName;
    }
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
