using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AcrylicWindow.Extensions
{
    public static class AutoMapperExtentions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            /// Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            return services.AddSingleton(mapper);
        }
    }
}
