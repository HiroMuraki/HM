using System.Collections.Immutable;

namespace HM.Ioc;

public sealed class ServiceProvider
{
    public bool TryGetService<TInterface>(out TInterface? result)
    {
        _registeredServices.TryGetValue(typeof(TInterface), out var value);

        if (value is not null)
        {
            result = (TInterface)value!;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public TInterface GetService<TInterface>()
    {
        if (TryGetService<TInterface>(out var value))
        {
            return value!;
        }
        else
        {
            throw new ServiceNotFoundException(typeof(TInterface).Name);
        }
    }

    public TInterface[] GetServices<TInterface>()
    {
        var result = new List<TInterface>();

        foreach (var item in _registeredServices.Values)
        {
            if (item is TInterface implement)
            {
                result.Add(implement);
            }
        }

        return result.ToArray();
    }

    public ServiceProvider(IEnumerable<KeyValuePair<Type, object>> services)
    {
        _registeredServices = services.ToImmutableDictionary();
    }

    #region NonPublic
    private readonly IImmutableDictionary<Type, object> _registeredServices;
    #endregion
}
