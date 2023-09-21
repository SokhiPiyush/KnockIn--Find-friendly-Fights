using API.Data;
using API.interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions//static enables us to use the methods without instantiating a new instance of this class
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            //to use DataContext class features throughout
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            // builder.Services.AddEndpointsApiExplorer();
            services.AddScoped<ITokenService, TokenService>();
            //builder.serveice.(lifetime)
            //Transient-> short lived and not standard for http
            //Scoped-> Https services//Scoped to https request
            //Singleton-> from application start till aplication closed down e.g caching service save cache for other responses
            
            return services;

        }
    }
}