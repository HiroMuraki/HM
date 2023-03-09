using System.Runtime.Serialization;

namespace HM.Ioc;

[Serializable]
public class ServiceScopeNotFoundException : Exception
{
    public string Scope { get; } = string.Empty;

    public ServiceScopeNotFoundException()
        : this(string.Empty, string.Empty, null!) { }

    public ServiceScopeNotFoundException(string scope)
        : this(scope, $"Scope `{scope}` not found", null!) { }

    public ServiceScopeNotFoundException(string scope, string message)
        : this(scope, message, null!) { }

    public ServiceScopeNotFoundException(string scope, string message, Exception inner) : base(message, inner)
    {
        Scope = scope;
    }

    protected ServiceScopeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
