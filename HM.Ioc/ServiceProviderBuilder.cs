namespace HM.Ioc;

public sealed class ServiceProviderBuilder
{
    public object this[Type type]
    {
        set
        {
            if (_registeredServices.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service of `{type}` already added");
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
        this[typeof(TInterface)] = service;
    }

    public void RemoveService<TInterface>()
        where TInterface : notnull
    {
        _registeredServices.Remove(typeof(TInterface));
    }

    public AppServices BuildServiceProvider()
    {
        return new AppServices(_registeredServices);
    }

    #region NonPublic
    private readonly Dictionary<Type, object> _registeredServices = new();
    #endregion
}

internal class Test
{
    private void T()
    {
        ScopedServiceProviders<string> scopedServiceProvider = new()
        {
            ["Storage"] = new ServiceProviderBuilder()
            {
                [typeof(int)] = "",
                [typeof(int)] = "",
                [typeof(int)] = "",
                [typeof(int)] = "",
            }.BuildServiceProvider(),

            ["Platform"] = new ServiceProviderBuilder()
            {
                [typeof(int)] = "",
                [typeof(int)] = "",
                [typeof(int)] = "",
                [typeof(int)] = "",
            }.BuildServiceProvider(),
        };
    }
}