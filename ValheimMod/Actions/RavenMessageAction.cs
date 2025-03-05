using ValheimTwitch.Patches;

namespace ValheimTwitch.Events
{
    internal class RavenMessageAction
    {
        internal static void Run()
        {
            var munin = true;

            RavenPatch.Message("You are fat", munin);
        }
    }
}