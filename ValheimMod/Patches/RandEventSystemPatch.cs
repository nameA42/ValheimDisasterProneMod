using HarmonyLib;
using System;
using UnityEngine;
using ValheimTwitch.Helpers;

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

namespace ValheimTwitch.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public class RandomEventSystemStartPatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(RandEventSystem), "SetRandomEvent", new Type[] { typeof(RandomEvent), typeof(Vector3) })]
        public static void PublicSetRandomEvent(RandEventSystem instance, RandomEvent ev, Vector3 pos)
        {
            return;
        }

        public static RandomEvent GetEventByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach (RandomEvent randomEvent in RandEventSystem.instance.m_events)
            {
                if (randomEvent.m_name == name)
                {
                    return randomEvent.Clone();
                }
            }

            return null;
        }

        public static void StartEvent(string name, Vector3 position, int duration)
        {
            var randomEvent = GetEventByName(name);

            if (randomEvent == null)
                return;

            randomEvent.m_enabled = true;
            randomEvent.m_nearBaseOnly = false;
            randomEvent.m_duration = duration * 60f;
            randomEvent.m_pauseIfNoPlayerInArea = false;

            randomEvent.m_startMessage = $"raid with a horde of {name.ToUpper()}!\nRun for your life!\n"; 
            randomEvent.m_endMessage = $"You survived the raid!\n";

            PublicSetRandomEvent(RandEventSystem.instance, randomEvent, position);
        }
    }
}

