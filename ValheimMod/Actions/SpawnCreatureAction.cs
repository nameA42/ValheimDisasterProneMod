using ValheimTwitch.Helpers;
using ValheimTwitch.Patches;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using NarcRandomMod;
using static MeleeWeaponTrail;
using System.Threading.Tasks;
using System.Collections;

namespace ValheimTwitch.Events
{
    internal class SpawnCreatureConfig
    {
        public string act;
        public string creature;
        public int level;
        public int count;
        public int offset;
        public int RampInd;

        public SpawnCreatureConfig()
        {
            this.act = "";
            this.creature = "";
            this.level = 0;
            this.count = 0;
            this.offset = 0;
            this.RampInd = -1;
        }

        public SpawnCreatureConfig(string act, string creature, int level, int count, int offset)
        {
            this.act = act;
            this.creature = creature;
            this.level = level;
            this.count = count;
            this.offset = offset;
            this.RampInd = -1;
        }

        public SpawnCreatureConfig(string act, string creature, int level, int count, int offset, int ind = -1)
        {
            this.act = act;
            this.creature = creature;
            this.level = level;
            this.count = count;
            this.offset = offset;
            this.RampInd = ind;
        }
    }
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

        public static List<SpawnCreatureConfig> sets = new List<SpawnCreatureConfig> {
            new SpawnCreatureConfig("fish", "Fish1", 1, 5, 10),
            new SpawnCreatureConfig("troll", "Troll", 1, 1, 10, 0),
            new SpawnCreatureConfig("skeleton", "Skeleton", 1, 3, 10, 1),
            new SpawnCreatureConfig("dragon","Dragon", 1, 100, 10),
            new SpawnCreatureConfig("logs", "beech_log_half", 1, 3, 10),
        };

        public static List<List<int>> Ramping = new List<List<int>> {
            new List<int> {1, 2, 4, 8, 10, 12, 14},
            new List<int> {1, 2, 4, 8, 16, 24, 24},
        };

        public float level = 1f;
        public float count = 1f;
        public float offset = 1f;
        internal static void Run(string act = "")
        {
            var almost = false;
            var set = sets[Random.Range(0, sets.Count)];
            if(act != "")
            {
                set = sets.Find(x => x.act == act);
            }
            var creature = set.creature;
            if (creature == "Dragon")
            {
                almost = true;
                set = sets[Random.Range(0, sets.Count)];
                creature = set.creature;
            }
            var level = set.level;
            var count = set.count;
            var offset = set.offset;
            var tamed = false;
            if(set.RampInd != -1)
            {
                Log.Warning("ramped to " + Ramping[set.RampInd][NarcRandoMod.Instance.worldLevel] + " for " + creature);
                count = Ramping[set.RampInd][NarcRandoMod.Instance.worldLevel];
            }

            if (Player.m_localPlayer != null)
            {
                var type1 = MessageHud.MessageType.Center;
                var msg = $"Here is a {creature} just for you (:\n";
                if(almost & creature != "Dragon")
                {
                    msg += "\n but it could have been worse (:<\n";
                }

                Player.m_localPlayer.Message(type1, $"{msg}");
            }

            if (set.act == "fish")
            {
                NarcRandoMod.Instance.fishig = true;
            }
            if (set.act == "logs")
            {
                NarcRandoMod.Instance.Logging = true;
            }
            if (set.act == "skeleton")
            {
                EnvMan.instance.m_debugEnv = "Misty";
                Task.Delay(2 * 60000).ContinueWith(t => EnvMan.instance.m_debugEnv = "");
                NarcRandoMod.Instance.skel = true;

            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Prefab.Spawn(creature, level, offset, tamed);
                }
            }
        }

        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        public static class PlayerFUAdd
        {
            public static void Postfix(Player __instance)
            {
                if (NarcRandoMod.Instance.fishig & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.fishig = false;
                    NarcRandoMod.Instance.fishigLock = false;
                }
                if (NarcRandoMod.Instance.fishig & NarcRandoMod.Instance.delay% 2 < 0.5 & !NarcRandoMod.Instance.fishigLock)
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
                if (NarcRandoMod.Instance.skel & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.skel = false;
                    NarcRandoMod.Instance.skelLock = false;
                }
                if (NarcRandoMod.Instance.skel & NarcRandoMod.Instance.delay % 20 < 0.5 & !NarcRandoMod.Instance.skelLock)
                {
                    for (int i = 0; i < Ramping[1][NarcRandoMod.Instance.worldLevel]; i++)
                    {
                            ConsoleUpdatePatch.AddAction(() => Prefab.Spawn("Skeleton", 1, 10, false));
                    }
                    NarcRandoMod.Instance.skelLock = true;
                }
                if (NarcRandoMod.Instance.skel & NarcRandoMod.Instance.delay % 10 > 1)
                {
                    NarcRandoMod.Instance.skelLock = false;
                }
                if (NarcRandoMod.Instance.Logging & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.Logging = false;
                    NarcRandoMod.Instance.LogLock = false;
                }
                if (NarcRandoMod.Instance.Logging & NarcRandoMod.Instance.delay % 5 < 0.5 & !NarcRandoMod.Instance.LogLock)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        ConsoleUpdatePatch.AddAction(() => Prefab.Spawn("beech_log_half", 1, 5, false, above:true));
                    }
                    NarcRandoMod.Instance.LogLock = true;
                }
                if (NarcRandoMod.Instance.Logging & NarcRandoMod.Instance.delay % 10 > 1)
                {
                    NarcRandoMod.Instance.LogLock = false;
                }
                if (NarcRandoMod.Instance.delay >= 115)
                {
                    if (NarcRandoMod.Instance.currentMobs.Count > 0)
                    {
                        Log.Info("Running Clear on " + NarcRandoMod.Instance.currentMobs.Count);
                        foreach (Character Mob in NarcRandoMod.Instance.currentMobs)
                        {
                            try
                            {
                                Log.Info(Mob.gameObject.name);
                                Mob.GetComponent<ZNetView>().Destroy();
                            }
                            catch
                            {
                                Log.Info("Failed to Destroy");
                            }
                        }
                        NarcRandoMod.Instance.currentMobs.Clear();
                        Log.Info("Enemies Cleared");
                    }
                }
            }
        }
    }
}