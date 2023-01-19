namespace MyStore.API.Commons
{
    using Microsoft.EntityFrameworkCore;
    using MyStore.Application.Commons;
    using MyStore.Domain.Entities;
    using MyStore.Infrastructure.Persistence;
    using System;
    using System.Linq;

    public static class DbInitializer
    {
        public static void Initialize(SLADbContext context)
        {
            context.Database.Migrate();
            var admin = context.Users.Include(x => x.UserRoles).FirstOrDefault(x => x.LoweredUserName == "admin");
            if (admin is null)
            {
                admin = new User(Guid.NewGuid(),
                                 "Admin",
                                 "Manager",
                                 "MyStore",
                                 PasswordHelper.Hash("12345678"));
                admin.Active();
                context.Users.Add(admin);
            }
            var role = context.Roles.FirstOrDefault(x => x.IsSystemAdmin == true);
            if (role is null)
            {
                role = new Role(Guid.NewGuid(), "Admin", "SystemAdmin");
                role.UpdateIsSystemAdmin(true);
                context.Roles.Add(role);
            }
            admin.AddRole(role.Id);

            context.SaveChanges();
        }
    }
}
