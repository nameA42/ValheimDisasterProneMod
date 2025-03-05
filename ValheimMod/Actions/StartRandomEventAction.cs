using UnityEngine;
using ValheimTwitch.Patches;

// blobs
// foresttrolls
// skeletons
// surtlings
// wolves

// army_bonemass
// army_eikthyr
// army_goblin
// army_moder
// army_theelder

// boss_bonemass
// boss_eikthyr
// boss_gdking
// boss_goblinking
// boss_moder

namespace ValheimTwitch.Events
{
    internal class StartRandomEventAction
    {
        internal static void Run()
        {
            ConsoleUpdatePatch.AddAction(() => StartRandomEvent());
        }

        private static void StartRandomEvent()
        {
            var eventName = "foresttrolls";
            var distance = 1;
            var duration = 1;

            Vector3 b = Random.insideUnitSphere * distance;
            var position = Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up + b;

            RandomEventSystemStartPatch.StartEvent(eventName, position, duration);
        }
    }
}