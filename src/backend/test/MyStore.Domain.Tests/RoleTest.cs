namespace MyStore.Domain.Tests
{
    using Shouldly;
    using MyStore.Domain.Entities;
    using System;
    using Xunit;

    public class RoleTest
    {
        [Fact]
        public void CreateTest()
        {
            var id = Guid.NewGuid();
            var role = new Role(id, "RoleTitle", "RoleDesc");

            role.Id.ShouldBe(id);
            role.Title.ShouldBe("RoleTitle");
            role.Description.ShouldBe("RoleDesc");
            role.IsSystemAdmin.ShouldBeFalse();
        }

        [Fact]
        public void UpdateTest()
        {
            var id = Guid.NewGuid();
            var role = new Role(id, "RoleTitle", "RoleDesc");

            role.UpdateTitle("RoleTitle Permissionsed");
            role.UpdateDescription("RoleDesc Permissionsed");
            role.UpdateIsSystemAdmin(true);

            role.Id.ShouldBe(id);
            role.Title.ShouldBe("RoleTitle Permissionsed");
            role.Description.ShouldBe("RoleDesc Permissionsed");
            role.IsSystemAdmin.ShouldBeTrue();
        }

        [Fact]
        public void PermissionTest()
        {
            var id = Guid.NewGuid();
            var role = new Role(id, "RoleTitle", "RoleDesc");

            role.AddPermission(1);
            role.AddPermission(2);
            role.AddPermission(3);

            role.RolePermissions.Count.ShouldBe(3);

            role.ClearPermissions();

            role.RolePermissions.Count.ShouldBe(0);
        }

        [Fact]
        public void PermissionDuplicateTest()
        {
            var id = Guid.NewGuid();
            var role = new Role(id, "RoleTitle", "RoleDesc");

            role.AddPermission(1);
            role.AddPermission(1);
            role.AddPermission(2);
            role.AddPermission(2);
            role.AddPermission(3);
            role.AddPermission(3);

            role.RolePermissions.Count.ShouldBe(3);
        }
    }
}
