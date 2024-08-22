
using Infrastructure.Queries;
using Infrastructure.Queries.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IlustraApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IAlumnoQuery, AlumnoQuery>();

            return services;
        }

    }
}
