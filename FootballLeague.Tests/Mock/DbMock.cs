namespace FootballLeague.Tests.Mock
{
    using FootballLeague.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class DbMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContext = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new ApplicationDbContext(dbContext);
            }
        }
    }
}
