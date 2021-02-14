using AcrylicWindow.Client.DAL;
using AcrylicWindow.Client.Data;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AcrylicWindow.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoProvider(this IServiceCollection services, string connection, string name)
        {
            /// Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicatinProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return services.AddScoped<IDataProvider, DataProvider>(p =>
                new DataProvider(mapper, connection, name));
        }
    }
}
