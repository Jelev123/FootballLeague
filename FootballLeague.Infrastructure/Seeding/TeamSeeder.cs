namespace FootballLeague.Infrastructure.Seeding
{
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TeamSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext data)
        {
            var teams = new List<Team>()
            {
                new Team()
                {
                    Name = "Real Madrid",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Manchester United",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Barselona",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Inter",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Arsenal",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Manchester City",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Milan",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
            };

           await data.Teams.AddRangeAsync(teams);
           await data.SaveChangesAsync();

        }
    }
}
