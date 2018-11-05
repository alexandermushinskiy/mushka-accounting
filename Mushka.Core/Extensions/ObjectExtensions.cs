using System.Text;
using Newtonsoft.Json;

namespace Mushka.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T[] AsArray<T>(this T value) =>
            new[]
            {
                value
            };

        public static byte[] ToByteArray<T>(this T value)
        {
            string json = JsonConvert.SerializeObject(value, Formatting.Indented);
            return Encoding.ASCII.GetBytes(json);
        }

        public static string ParseByteArrayToString(this object byteArray) => Encoding.UTF8.GetString(byteArray as byte[]);
    }
}