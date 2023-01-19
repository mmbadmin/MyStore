namespace MyStore.API.Commons
{
    using MyStore.Application.Commons;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class Extension
    {
        public static ExcelRangeBase LoadFromCollectionFiltered<T>(this ExcelRangeBase @this, IEnumerable<T> collection)
    where T : class
        {
            MemberInfo[] membersToInclude = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore)))
                .ToArray();

            return @this.LoadFromCollection<T>(collection,
                                               true,
                                               OfficeOpenXml.Table.TableStyles.None,
                                               BindingFlags.Instance | BindingFlags.Public,
                                               membersToInclude);
        }
    }
}
