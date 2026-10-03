using System.Text.Json;

namespace less1.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            try
            {
                var json = session.GetString(key);

                if (string.IsNullOrEmpty(json))
                {
                    return default;
                }

                var value = JsonSerializer.Deserialize<T>(json);

                return value;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
