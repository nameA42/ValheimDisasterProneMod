using HarmonyLib;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using ValheimTwitch.Helpers;
using NarcRandomMod;

namespace ValheimTwitch.Patches
{
    [HarmonyPatch(typeof(Player), "Load")]
    public static class PlayerLoadPatch
    {
        public static AudioSource audioSource;
        public static AudioClip whistleClip;

        public static void Postfix(Player __instance, ref HashSet<string> ___m_shownTutorials)
        {
            // Remove old custom tutorial messages
            int count = ___m_shownTutorials.Count;

            ___m_shownTutorials.RemoveWhere(hash => hash.StartsWith("dev.skarab42.valheim.twitch"));

            UnityEngine.Debug.Log($"Removed {count - ___m_shownTutorials.Count} messages");

            audioSource = __instance.gameObject.AddComponent<AudioSource>();
            whistleClip = EmbeddedAsset.LoadAudioClip("Assets.calling-whistle.wav");

            Log.Info($"Audio time {audioSource.time}");
        }

        public static void Whistle(float volume = 0.5f)
        {
            audioSource.PlayOneShot(whistleClip, volume);
        }
    }
    
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public static class PlayerOnSpawnedPatch
    {
        public static void Postfix(ref HashSet<string> ___m_shownTutorials)
        {
            if (NarcRandoMod.Instance.isHuginIntroShown)
                return;

            RavenPatch.Message($"You Are Fat Bald and Retarded", false);


            NarcRandoMod.Instance.isHuginIntroShown = true;
        }
    }

    [HarmonyPatch(typeof(Player), "Update")]
    public static class PlayerUpdatePatch
    {
        public static void Prefix(Player __instance)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                new Thread(() =>
                {
                    NarcRandoMod.Instance.Toggle();
                }).Start();
            }

            if (!NarcRandoMod.Instance.isInGame || !Input.GetKeyDown(KeyCode.L))
                return;

            PlayerLoadPatch.Whistle(AudioMan.GetSFXVolume());

            foreach (var character in CharacterAwakePatch.tamedCharacters)
            {
                var znview = character.GetComponent<ZNetView>();

                if (znview == null)
                    continue;

                var zdo = znview.GetZDO();

                if (zdo == null)
                    continue;

                var customName = zdo.GetString($"{NarcRandoMod.pluginGUID}-name");

                if (customName.Length == 0)
                    return;

                var ai = character.GetComponent<MonsterAI>();

                ai.SetFollowTarget(__instance.gameObject);
            }
        }
    }
}
