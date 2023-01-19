namespace MyStore.Domain.Tests
{
    using Shouldly;
    using MyStore.Domain.Entities;
    using MyStore.Domain.Enums;
    using Xunit;

    public class SettingTest
    {
        [Fact]
        public void CreateTest()
        {
            var setting = new Setting("SettingTitle", "SettingName", "SettingValue", SettingValueType.Int);

            setting.Title.ShouldBe("SettingTitle");
            setting.Name.ShouldBe("SettingName");
            setting.Value.ShouldBe("SettingValue");
            setting.Type.ShouldBe(SettingValueType.Int);
        }

        [Fact]
        public void UpdateTest()
        {
            var setting = new Setting("SettingTitle", "SettingName", "SettingValue", SettingValueType.Int);

            setting.UpdateValue("SettingValue Permissionsed");

            setting.Title.ShouldBe("SettingTitle");
            setting.Name.ShouldBe("SettingName");
            setting.Value.ShouldBe("SettingValue Permissionsed");
            setting.Type.ShouldBe(SettingValueType.Int);
        }
    }
}
