using System;
using ValheimTwitch.Patches;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using NarcRandomMod;
using ValheimTwitch.Helpers;

namespace ValheimTwitch.Events
{
    internal class PlayerAction
    {
        private static GameObject GO_Meteor;

        private static Projectile P_Meteor;

        private static int ScriptChar_Layermask = LayerMask.GetMask("Default", "static_solid", "Default_small", "piece_nonsolid", "terrain", "vehicle", "piece", "viewblock", "character", "character_net", "character_ghost");

        internal static void Run()
        {
            var names = new List<String> { "puke", "heal", "meteor", "skl" };
            var name = names[UnityEngine.Random.Range(0, names.Count)];

            switch (name)
            {
                case "puke":
                    ConsoleUpdatePatch.AddAction(Puke);
                    break;
                case "heal":
                    ConsoleUpdatePatch.AddAction(Heal);
                    break;
                case "meteor":
                    NarcRandoMod.Instance.Met = true;
                    break;
                case "skl":
                    ConsoleUpdatePatch.AddAction(scal);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static void Puke()
        {
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.ClearFood();
                var type1 = MessageHud.MessageType.Center;
                var msg = $"Puke for me (:<";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }
        }

        static void Heal()
        {
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.Heal(Player.m_localPlayer.GetMaxHealth(), true);

                var type1 = MessageHud.MessageType.Center;
                var msg = $"Here is a heal, now go do some pushups... bitch";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }
        }
        static void scal()
        {
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.Heal(Player.m_localPlayer.GetMaxHealth(), true);

                var type1 = MessageHud.MessageType.Center;
                var msg = $"Wittle Baby";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }
            Player.m_localPlayer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            NarcRandoMod.Instance.Smol = true;
        }

        public static void Meteor()
        {
            Vector3 vector = Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.up * 2f + Player.m_localPlayer.GetLookDir() * 1f;
            GameObject prefab = ZNetScene.instance.GetPrefab("projectile_meteor");
            for (int i = 0; i < 1; i++)
            {
                GO_Meteor = UnityEngine.Object.Instantiate(prefab, new Vector3(vector.x + (float)UnityEngine.Random.Range(-100, 100), vector.y + 250f, vector.z + (float)UnityEngine.Random.Range(-100, 100)), Quaternion.identity);
                P_Meteor = GO_Meteor.GetComponent<Projectile>();
                P_Meteor.name = "Meteor" + i;
                P_Meteor.m_respawnItemOnHit = false;
                P_Meteor.m_spawnOnHit = null;
                P_Meteor.m_ttl = 6f;
                P_Meteor.m_gravity = 0f;
                P_Meteor.m_rayRadius = 0.1f;
                P_Meteor.m_aoe = 8f + 0.04f;
                P_Meteor.transform.localRotation = Quaternion.LookRotation(Player.m_localPlayer.GetAimDir(vector));
                GO_Meteor.transform.localScale = Vector3.zero;
                RaycastHit hitInfo = default(RaycastHit);
                Vector3 position = Player.m_localPlayer.transform.position;
                Vector3 target = position;
                target.x += UnityEngine.Random.Range(-8, 8);
                target.y += UnityEngine.Random.Range(-8, 8);
                target.z += UnityEngine.Random.Range(-8, 8);
                HitData hitData = new HitData();
                hitData.m_damage.m_fire = UnityEngine.Random.Range(30f + 0.5f, 50f);
                hitData.m_damage.m_blunt = UnityEngine.Random.Range(15f + 0.25f, 30f + 0.5f);
                hitData.m_pushForce = 10f;
                Vector3 vector2 = Vector3.MoveTowards(GO_Meteor.transform.position, target, 1f);
                P_Meteor.Setup(Player.m_localPlayer, (vector2 - GO_Meteor.transform.position) * UnityEngine.Random.Range(78f, 86f), -1f, hitData, null, null);
            }
        }
        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        public static class PlayerFUAdd
        {
            public static void Postfix(Player __instance)
            {
                if (NarcRandoMod.Instance.Met & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.Met = false;
                    NarcRandoMod.Instance.MetLock = false;
                }
                if (NarcRandoMod.Instance.Met & NarcRandoMod.Instance.delay % 3 < 0.2 & !NarcRandoMod.Instance.MetLock)
                {
                    ConsoleUpdatePatch.AddAction(Meteor);
                    NarcRandoMod.Instance.MetLock = true;
                }
                if (NarcRandoMod.Instance.Met & NarcRandoMod.Instance.delay % 3 > 1)
                {
                    NarcRandoMod.Instance.MetLock = false;
                }
            }
        }
    }
}