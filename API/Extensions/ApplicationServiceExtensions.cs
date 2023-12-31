using API.Data;
using API.Helpers;
using API.interfaces;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions//static enables us to use the methods without instantiating a new instance of this class
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            //to use DataContext class features throughout
            // services.AddDbContext<DataContext>(opt =>
            // {
            //     opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            // });//moved to program class
            services.AddCors();
            // builder.Services.AddEndpointsApiExplorer();
            services.AddScoped<ITokenService, TokenService>();
            //builder.serveice.(lifetime)
            //Transient-> short lived and not standard for http
            //Scoped-> Https services//Scoped to https request
            //Singleton-> from application start till aplication closed down e.g caching service save cache for other responses

            // services.AddScoped<IUserRepository, UserRepository>();//UnitOfWork

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            // services.AddScoped<ILikesRepository, LikesRepository>();
            // services.AddScoped<IMessageRepository, MessageRepository>();injecting repos in conrollers here // now injecting UnitOfWork instead

            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            
            return services;

        }
    }
}