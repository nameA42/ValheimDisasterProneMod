using BepInEx.Logging;
using NarcRandomMod;

namespace ValheimTwitch
{
    class Log
    {
        private static readonly ManualLogSource logger = Logger.CreateLogSource(NarcRandoMod.pluginName);

        public static void Debug(string message)
        {
            logger.LogDebug(message);
        }

        public static void Info(string message)
        {
            logger.LogInfo(message);
        }

        public static void Warning(string message)
        {
            logger.LogWarning(message);
        }

        public static void Error(string message)
        {
            logger.LogError(message);
        }
    }
}
