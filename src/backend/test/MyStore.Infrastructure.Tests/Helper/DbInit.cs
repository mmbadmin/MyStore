namespace MyStore.Infrastructure.Tests.Helper
{
    using Microsoft.EntityFrameworkCore;
    using MyStore.Infrastructure.Persistence;
    using System;

    public static class DbInit
    {
        public static SLADbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<SLADbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            var context = new SLADbContext(options, null);

            context.Database.EnsureDeleted();

            // SeedSampleData(context);
            return context;
        }
    }
}
