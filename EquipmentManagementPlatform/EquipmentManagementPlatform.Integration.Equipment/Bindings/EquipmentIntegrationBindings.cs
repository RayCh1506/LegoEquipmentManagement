using EquipmentManagementPlatform.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EquipmentManagementPlatform.Integration.Equipment.Bindings
{
    public static class EquipmentIntegrationBindings
    {
        public static void ConfigureBindings(IServiceCollection services)
        {
            services.AddScoped<IEquipmentIntegrationService, EquipmentIntegrationService>();
        }
    }
}
