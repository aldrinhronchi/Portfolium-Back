namespace Portfolium_Back.Extensions.Middleware
{
    public class ServiceLocator
    {
        private ServiceProvider _currentServiceProvider;
        private static ServiceProvider? _serviceProvider;

        public ServiceLocator(ServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ServiceLocator Current
        {
            get
            {
                return new ServiceLocator(_serviceProvider!);
            }
        }

        public static void IncluirServico(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Object? BuscarServico(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType);
        }

        public TService? BuscarServico<TService>()
        {
            return _currentServiceProvider.GetService<TService>();
        }
    }
} 