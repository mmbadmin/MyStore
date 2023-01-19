namespace MyStore.Common.Application.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class AppAccessAttribute : Attribute
    {
        public AppAccessAttribute(string groupTitle, string groupName, string name)
        {
            GroupTitle = groupTitle;
            GroupName = groupName;
            Name = name;
        }

        public string GroupTitle { get; set; }

        public string GroupName { get; set; }

        public string Name { get; set; }

        public bool Ignore { get; set; }

        public bool AllAccess { get; set; }
    }
}
