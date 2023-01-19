namespace MyStore.Common.Test
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public static class TestHelper
    {
        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonHelper.ToJson(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(this HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonHelper.ToObject<T>(stringResponse);

            return result;
        }

        public static async Task<string> GetResponseContent(this HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
