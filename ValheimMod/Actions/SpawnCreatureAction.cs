using ValheimTwitch.Helpers;
using ValheimTwitch.Patches;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using NarcRandomMod;
using static MeleeWeaponTrail;
using System.Threading.Tasks;

namespace ValheimTwitch.Events
{
    internal class SpawnCreatureAction
    {
        public static List<string> untameableCreatures = new List<string>
    {
        "Crow", "Deer", "Fish1", "Fish2", "Fish3", "Fish4_cave", "Gull", "Leviathan", "Odin", "Eikthyr",
        "gd_king", "Bonemass", "Dragon", "GoblinKing"
    };

        public static List<string> creatures = new List<string>
    {
        "Blob", "BlobElite", "Boar", "Crow", "Deathsquito", "Deer", "Draugr", "Draugr_Elite", "Draugr_Ranged", "Fenring",
        "FireFlies", "Fish1", "Fish2", "Fish3", "Ghost", "Goblin", "GoblinArcher", "GoblinBrute", "GoblinShaman", "Greydwarf",
        "Greydwarf_Elite", "Greydwarf_Shaman", "Greyling", "Hatchling", "Leech", "Leviathan", "Lox", "Neck", "Seagal", "Serpent",
        "Skeleton", "Skeleton_NoArcher", "Skeleton_Poison", "StoneGolem", "Surtling", "Troll", "Wolf", "Wraith", "Eikthyr", "gd_king",
        "Bonemass", "Dragon", "GoblinKing"
    };

        public static List<List<int>> sets = new List<List<int>> {
            new List<int> { 11, 1, 5, 10 },
            new List<int> { 35, 1, 3, 10 },
            new List<int> { 30, 1, 3, 10 }
        };
        internal static void Run()
        {

            var set = sets[Random.Range(0, sets.Count)];
            var creature = set[0];
            var level = set[1];
            var count = set[2];
            var offset = set[3];
            var tamed = false;

            var type = creatures[creature];


            if (Player.m_localPlayer != null)
            {
                var type1 = MessageHud.MessageType.Center;
                var msg = $"Here is a {type} just for you (:";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }

            if (creature == 11)
            {
                NarcRandoMod.Instance.fishig = true;
            }
            if (creature == 30)
            {
                EnvMan.instance.m_debugEnv = "DeepForest Mist";
                Task.Delay(2 * 60000).ContinueWith(t => EnvMan.instance.m_debugEnv = "");
                NarcRandoMod.Instance.skel = true;

            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    ConsoleUpdatePatch.AddAction(() => Prefab.Spawn(type, level, offset, tamed));
                }
            }
        }

        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        public static class PlayerFUAdd
        {
            public static void Postfix(Player __instance)
            {
                if (NarcRandoMod.Instance.fishig & NarcRandoMod.Instance.delay >= NarcRandoMod.Instance.Fulldelay -1)
                {
                    NarcRandoMod.Instance.fishig = false;
                    NarcRandoMod.Instance.fishigLock = false;
                    ItemDrop[] array2 = UnityEngine.Object.FindObjectsOfType<ItemDrop>();
                    for (int i = 0; i < array2.Length; i++)
                    {
                        ZNetView component = array2[i].GetComponent<ZNetView>();
                        if (component)
                        {
                            component.Destroy();
                        }
                    }
                    Log.Info("Items Cleared");
                }
                if (NarcRandoMod.Instance.fishig & NarcRandoMod.Instance.delay% 2 < 0.2 & !NarcRandoMod.Instance.fishigLock)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        ConsoleUpdatePatch.AddAction(() => Prefab.Spawn("Fish1", 1, 10, false, above:true));
                    }
                    NarcRandoMod.Instance.fishigLock = true;
                }
                if (NarcRandoMod.Instance.fishig & NarcRandoMod.Instance.delay % 2 > 1)
                {
                    NarcRandoMod.Instance.fishigLock = false;
                }
                if (NarcRandoMod.Instance.skel & NarcRandoMod.Instance.delay >= 110)
                {
                    NarcRandoMod.Instance.skel = false;
                    NarcRandoMod.Instance.skelLock = false;
                }
                if (NarcRandoMod.Instance.skel & NarcRandoMod.Instance.delay % 20 < 0.2 & !NarcRandoMod.Instance.skelLock)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ConsoleUpdatePatch.AddAction(() => Prefab.Spawn("Skeleton", 1, 3, false));
                    }
                    NarcRandoMod.Instance.skelLock = true;
                }
                if (NarcRandoMod.Instance.skel & NarcRandoMod.Instance.delay % 20 > 1)
                {
                    NarcRandoMod.Instance.skelLock = false;
                }
            }
        }
    }
}