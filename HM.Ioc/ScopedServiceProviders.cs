namespace HM.Ioc;

public sealed class ScopedServiceProviders<TScope>
    where TScope : notnull
{
    public AppServices this[TScope scope]
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
    private readonly Dictionary<TScope, AppServices> _scopedServiceProvider = new();
    #endregion
}