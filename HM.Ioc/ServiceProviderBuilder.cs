namespace HM.Ioc;

public class ServiceProviderBuilder
{
    public object this[Type type]
    {
        get
        {
            bool valueGet = _registeredServices.TryGetValue(type, out var value);

            if (valueGet)
            {
                return value!;
            }

            throw new ServiceNotFoundException(type.Name);
        }
        set
        {
            if (_registeredServices.ContainsKey(type))
            {
                throw new InvalidOperationException($"Singleton of type `{type}` already registered");
            }

            if (!value.GetType().IsAssignableTo(type))
            {
                throw new InvalidOperationException($"{value.GetType()} is not a implemention of {type.Name}");
            }

            _registeredServices[type] = value;
        }
    }

    public void AddService<TInterface>(TInterface service)
        where TInterface : notnull
    {
        if (_registeredServices.ContainsKey(typeof(TInterface)))
        {
            throw new InvalidOperationException($"Service `{typeof(TInterface)}` already registered");
        }

        _registeredServices[typeof(TInterface)] = service;
    }

    public void RemoveService<TInterface>()
    {
        _registeredServices.Remove(typeof(TInterface));
    }

    public ServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(_registeredServices);
    }

    #region NonPublic
    private readonly Dictionary<Type, object> _registeredServices = new();
    #endregion
}