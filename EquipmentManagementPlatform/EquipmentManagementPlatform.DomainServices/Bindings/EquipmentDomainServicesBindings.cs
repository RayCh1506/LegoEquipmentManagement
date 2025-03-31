using EquipmentManagementPlatform.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EquipmentManagementPlatform.DomainServices.Bindings
{
    public static class EquipmentDomainServicesBindings
    {
        public static void ConfigureBindings(IServiceCollection services)
        {
            services.AddScoped<IEquipmentService, EquipmentService>();
        }
    }
}
