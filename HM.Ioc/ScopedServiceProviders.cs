namespace HM.Ioc;

public sealed class ScopedServiceProviders<TScope>
    where TScope : notnull
{
    public ServiceProvider this[TScope scope]
    {
        get
        {
            if (_scopedServiceProvider.TryGetValue(scope, out var serviceProvider))
            {
                return serviceProvider;
            }
            else
            {
                throw new ServiceScopeNotFoundException(scope?.ToString() ?? string.Empty);
            }
        }

        set => _scopedServiceProvider[scope] = value;
    }

    #region NonPublic
    private readonly Dictionary<TScope, ServiceProvider> _scopedServiceProvider = new();
    #endregion
}