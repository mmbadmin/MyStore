namespace MyStore.Application.Commons
{
    using MyStore.Common.Application.Interfaces;
    using MyStore.Domain.Entities;
    using RExtension;
    using System.Threading.Tasks;

    public static class Extension
    {
        public static async Task<int> GetIntAsync(this IBaseRepository<Setting, int> repo, string title, int defaultValue = 0)
        {
            var item = await repo.FindAsync(predicate: x => x.Title.ToLower() == title.ToLower());
            return item?.Value?.ToInt() ?? defaultValue;
        }

        public static async Task<bool> GetBooleanAsync(this IBaseRepository<Setting, int> repo, string title, bool defaultValue = false)
        {
            var item = await repo.FindAsync(predicate: x => x.Title.ToLower() == title.ToLower());
            if (item?.Value is null)
            {
                return defaultValue;
            }
            return bool.Parse(item.Value);
        }
    }
}
