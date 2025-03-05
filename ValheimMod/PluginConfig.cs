using UnityEngine;

namespace ValheimTwitch
{
    static class PluginConfig
    {
        private static string Key(string key)
        {
            return $"{key}";
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(Key(key));
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(Key(key));
        }

        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(Key(key));
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(Key(key), value);
        }

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(Key(key));
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(Key(key), value);
        }
    }
}
