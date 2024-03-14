namespace FootballLeague.Infrastructure.Seeding
{
    using FootballLeague.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class DbSeeder
    {
        public static async Task EnsureDatabaseSeeded(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await MigrateDatabaseAsync(context);

                if (!context.Teams.Any())
                {
                    await SeedDatabaseAsync(context);
                }
            }
        }

        private static async Task MigrateDatabaseAsync(ApplicationDbContext context)
        {
            if (await context.Database.EnsureCreatedAsync())
            {
                await context.Database.MigrateAsync();
            }
        }

        private static async Task SeedDatabaseAsync(ApplicationDbContext context)
        {
            await TeamSeeder.SeedDataAsync(context);
        }
    }
}
