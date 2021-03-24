using InventoryManagement.App.Services.Repositories;
using InventoryManagment.App.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.App.Services
{
    public static class Bootstrapper
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddTransient<IStoreManagementRepository, InventoryManagementRepository>();
        }
    }
}