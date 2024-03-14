namespace FootballLeague.Configuaration
{
    using FootballLeague.Core.Contracts.Loger;
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Core.Services.Logger;
    using FootballLeague.Core.Services.Match;
    using FootballLeague.Core.Services.Team;
    using FootballLeague.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;

    public static class ApplicationServices
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFilePath = Path.Combine(logDirectory, "log.txt");

            services.AddMemoryCache();
            services.AddScoped<TeamService>();
            services.AddScoped<MatchService>();
            services.AddScoped<ITeamService, TeamDecoratorService>();
            services.AddTransient<IMatchService, MatchDecoratorService>();
            services.AddSingleton<ILoggerService>(provider => new LoggerService(logFilePath));
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
