using AcrylicWindow.Client.DAL;
using AcrylicWindow.Client.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace AcrylicWindow.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoProvider(this IServiceCollection services)
        {
            var connection = ConfigurationManager.ConnectionStrings["acrylicdb"];

            return services.AddScoped<IDataProvider, DataProvider>(p =>
                new DataProvider(connection.ConnectionString, connection.Name));
        }
    }
}
