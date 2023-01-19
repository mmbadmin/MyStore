namespace MyStore.Common.Test
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class JsonHelper
    {
        public static string ToJson(object obj)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static object ToObject(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public static T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
