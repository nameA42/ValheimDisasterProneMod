using System;

namespace ValheimTwitch.Patches
{
    public static class RavenPatch
    {
        static int messageCount = 0;

        public static void Message(string text, bool munin)
        {
            try
            {
                Log.Info($"Message -> user: text:{text}");

                Raven.RavenText ravenText = new Raven.RavenText();

                var key = "" + messageCount++;
                var label = "loser";
                var topic = "retarded";

                Log.Info($"AddTempText -> {key}");

                Raven.AddTempText(key, topic, text, label, munin);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
