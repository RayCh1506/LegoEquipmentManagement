using EquipmentManagementPlatform.Domain.Interfaces;
using EquipmentManagementPlatform.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EquipmentManagementPlatform.Repository.Bindings
{
    public static class EquipmentRepositoryBindings
    {
        public static void ConfigureBindings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEquipmentRepository,  EquipmentRepository>();
            services.AddDbContext<EquipmentContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("EquipmentConnection")));
        }
    }
}
