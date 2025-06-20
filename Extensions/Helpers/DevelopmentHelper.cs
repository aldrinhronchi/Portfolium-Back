using Portfolium_Back.Extensions.Middleware;

namespace Portfolium_Back.Extensions.Helpers
{
    public static class DevelopmentHelper
    {
        public static Boolean InDevelopment()
        {
            IConfiguration? appsettings = ServiceLocator.Current.BuscarServico<IConfiguration>();
            if (appsettings != null)
            {
                String? Ambiente = appsettings["Environment"] ?? "Development";
                return !String.IsNullOrWhiteSpace(Ambiente) && Ambiente.Contains("Development");
            }
            return true;
        }
    }
} 