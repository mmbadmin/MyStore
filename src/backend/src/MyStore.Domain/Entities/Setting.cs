namespace MyStore.Domain.Entities
{
    using MyStore.Common.Domain;
    using MyStore.Domain.Enums;

    public class Setting : BaseEntity<int>, IAggregateRoot, IAuditableEntity, IDeletedEntity
    {
        protected Setting()
        {
        }

        public Setting(string title, string name, string value, SettingValueType type)
        {
            Title = title;
            Name = name;
            Value = value;
            Type = type;
        }

        public string Title { get; private set; }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public SettingValueType Type { get; private set; }

        public void UpdateValue(string value)
        {
            Value = value;
        }
    }
}
