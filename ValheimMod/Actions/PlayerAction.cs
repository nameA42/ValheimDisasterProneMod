using System;
using ValheimTwitch.Patches;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using NarcRandomMod;
using ValheimTwitch.Helpers;
using System.Threading.Tasks;

namespace ValheimTwitch.Events
{
    class PlayerAction
    {
        private static GameObject GO_Meteor;
        private static GameObject GO_Meteor2;

        private static Projectile P_Meteor;
        private static Projectile B_Meteor;

        private static int ScriptChar_Layermask = LayerMask.GetMask("Default", "static_solid", "Default_small", "piece_nonsolid", "terrain", "vehicle", "piece", "viewblock", "character", "character_net", "character_ghost");
        private static int Warp_Layermask = LayerMask.GetMask("Default", "static_solid", "Default_small", "piece_nonsolid", "terrain", "vehicle", "piece", "viewblock", "Water", "character", "character_net", "character_ghost");

        private static float warpDistance = 5.0f;
        internal static void Run(string act = "puke")
        {
            switch (act)
            {
                case "puke":
                    Log.Info("startPuke");
                    NarcRandoMod.Instance.Vommiting = true;
                    break;
                case "heal":
                    ConsoleUpdatePatch.AddAction(Heal);
                    break;
                case "meteor":
                    NarcRandoMod.Instance.Met = true;
                    break;
                case "baby":
                    ConsoleUpdatePatch.AddAction(baby);
                    break;
                case "regeneration":
                    Log.Info("HealSur");
                    NarcRandoMod.Instance.HS = true;
                    doMSG("Regeneration for others");
                    break;
                case "warp":
                    Log.Info("Warping");
                    NarcRandoMod.Instance.Warping = true;
                    doMSG("Warping");
                    break;
                case "moongrav":
                    Log.Info("Graving");
                    ConsoleUpdatePatch.AddAction(Moon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void doMSG(string s)
        {
            if (Player.m_localPlayer != null)
            {
                var type1 = MessageHud.MessageType.Center;

                Player.m_localPlayer.Message(type1, $"{s}\n");
            }
        }

        public static void Puke()
        {
            Log.Info("Puke");
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.ClearFood();
                var type1 = MessageHud.MessageType.Center;
                var msg = $"Puke for me (:<";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }
        }
        public static void Moon()
        {
            Log.Info("Moon");
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.m_jumpForce = 20f;
                NarcRandoMod.Instance.falldmg = false;
                var type1 = MessageHud.MessageType.Center;
                var msg = $"TO THE MOON";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }
            Task.Delay(120000).ContinueWith(t => NarcRandoMod.Instance.falldmg = true);
            Task.Delay(120000).ContinueWith(t => Player.m_localPlayer.m_jumpForce = 10f);
        }
        public static void Heal()
        {
            Log.Info("Heal");
            if (Player.m_localPlayer != null)
            {
                Player.m_localPlayer.Heal(Player.m_localPlayer.GetMaxHealth(), true);

                var type1 = MessageHud.MessageType.Center;
                var msg = $"Here is a heal, now go do some pushups... bitch";

                Player.m_localPlayer.Message(type1, $"{msg}\n");
            }
        }

        public static void HealSurroundings()
        {
            Log.Info("HealSurroundings");
            List<Character> list = new List<Character>();
            list.Clear();
            Character.GetCharactersInRange(Player.m_localPlayer.transform.position, 20f, list);
            foreach (Character item in list)
            {
                if (item != Player.m_localPlayer)
                {
                    item.Heal(item.GetMaxHealth());
                }
            }
        }
        public static void baby()
        {
            Log.Info("baby");
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
            Log.Info("Meteor");
            Vector3 vector = Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.up * 2f + Player.m_localPlayer.GetLookDir() * 1f;
            GameObject prefab = ZNetScene.instance.GetPrefab("projectile_meteor");
            for (int i = 0; i < 1; i++)
            {
                var x = vector.x + (float)UnityEngine.Random.Range(-100, 100);
                var z = vector.z + (float)UnityEngine.Random.Range(-100, 100);
                GO_Meteor = UnityEngine.Object.Instantiate(prefab, new Vector3(x, vector.y + 250f, z), Quaternion.identity);
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
                GO_Meteor2 = UnityEngine.Object.Instantiate(prefab, new Vector3(x, vector.y + 250f, z), Quaternion.identity);
                B_Meteor = GO_Meteor2.GetComponent<Projectile>();
                B_Meteor.name = "Meteor" + i;
                B_Meteor.m_respawnItemOnHit = false;
                B_Meteor.m_spawnOnHit = null;
                B_Meteor.m_ttl = 6f;
                B_Meteor.m_gravity = 0f;
                B_Meteor.m_rayRadius = 0.1f;
                B_Meteor.m_aoe = 8f + 0.04f;
                B_Meteor.transform.localRotation = Quaternion.LookRotation(Player.m_localPlayer.GetAimDir(vector));
                GO_Meteor2.transform.localScale = Vector3.zero;
                Vector3 position = Player.m_localPlayer.transform.position;
                Vector3 target = position;
                target.x += UnityEngine.Random.Range(-8, 8);
                target.y += UnityEngine.Random.Range(-8, 8);
                target.z += UnityEngine.Random.Range(-8, 8);
                HitData hitData = new HitData();
                var dmgMod = 0.5f;
                hitData.m_damage.m_fire = UnityEngine.Random.Range(20f + 0.5f, 30f) * dmgMod;
                hitData.m_damage.m_blunt = UnityEngine.Random.Range(15f + 0.25f, 30f + 0.5f) * dmgMod;
                hitData.m_pushForce = 10f;
                Vector3 vector2 = Vector3.MoveTowards(GO_Meteor.transform.position, target, 1f);
                var velmod = UnityEngine.Random.Range(78f, 86f);
                P_Meteor.Setup(Player.m_localPlayer, (vector2 - GO_Meteor.transform.position) * velmod, -1f, hitData, null, null);
                HitData hitData1 = new HitData();
                var pdmgMod = 1f;
                hitData1.m_damage.m_fire = UnityEngine.Random.Range(10f, 15f) * pdmgMod;
                hitData1.m_pushForce = 30f;
                B_Meteor.Setup(new Character(), (vector2 - GO_Meteor.transform.position) * velmod, -1f, hitData1, null, null);
            }
        }

        public static void Warp()
        {
            Log.Info("Warp");
            var player = Player.m_localPlayer;
            RaycastHit hitInfo2 = default(RaycastHit);
            Vector3 eyePoint = player.GetEyePoint();
            Vector3 LookDir = player.GetLookDir() + Vector3.up * UnityEngine.Random.Range(0f, 360f);
            Vector3 target = ((!Physics.Raycast(player.GetEyePoint(), LookDir, out hitInfo2, float.PositiveInfinity, Warp_Layermask) || !hitInfo2.collider) ? (eyePoint + player.GetLookDir() * 1000f) : hitInfo2.point);
            float magnitude = (hitInfo2.point - eyePoint).magnitude;
            Log.Info("mag" + magnitude);
            float MaxMag = (warpDistance * LookDir).magnitude;
            if (magnitude > MaxMag)
            {
                magnitude = MaxMag;
            }
            Vector3 vector3 = Vector3.MoveTowards(player.transform.position, target, magnitude);
            Console.Log("Warping to " + vector3);
            player.transform.position = vector3 + Vector3.up*10;
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
                if (NarcRandoMod.Instance.Met & NarcRandoMod.Instance.delay % 3 < 0.5 & !NarcRandoMod.Instance.MetLock)
                {
                    ConsoleUpdatePatch.AddAction(Meteor);
                    NarcRandoMod.Instance.MetLock = true;
                }
                if (NarcRandoMod.Instance.Met & NarcRandoMod.Instance.delay % 3 > 1)
                {
                    NarcRandoMod.Instance.MetLock = false;
                }
                if (NarcRandoMod.Instance.HS & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.HS = false;
                    NarcRandoMod.Instance.HSLock = false;
                }
                if (NarcRandoMod.Instance.HS & NarcRandoMod.Instance.delay % 10 < 0.5 & !NarcRandoMod.Instance.HSLock)
                {
                    ConsoleUpdatePatch.AddAction(HealSurroundings);
                    NarcRandoMod.Instance.HSLock = true;
                }
                if (NarcRandoMod.Instance.HS & NarcRandoMod.Instance.delay % 10 > 1)
                {
                    NarcRandoMod.Instance.HSLock = false;
                }
                if (NarcRandoMod.Instance.Warping & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.Warping = false;
                    NarcRandoMod.Instance.WarpingLock = false;
                }
                if (NarcRandoMod.Instance.Warping & NarcRandoMod.Instance.delay % 5 < 0.5 & !NarcRandoMod.Instance.WarpingLock)
                {
                    ConsoleUpdatePatch.AddAction(Warp);
                    NarcRandoMod.Instance.WarpingLock = true;
                }
                if (NarcRandoMod.Instance.Warping & NarcRandoMod.Instance.delay % 5 > 1)
                {
                    NarcRandoMod.Instance.WarpingLock = false;
                }
                if (NarcRandoMod.Instance.Vommiting & NarcRandoMod.Instance.delay >= 115)
                {
                    NarcRandoMod.Instance.Vommiting = false;
                    NarcRandoMod.Instance.VomitLock = false;
                }
                if (NarcRandoMod.Instance.Vommiting & NarcRandoMod.Instance.delay % 20 < 0.5 & !NarcRandoMod.Instance.VomitLock)
                {
                    ConsoleUpdatePatch.AddAction(Puke);
                    NarcRandoMod.Instance.VomitLock = true;
                }
                if (NarcRandoMod.Instance.Vommiting & NarcRandoMod.Instance.delay % 20 > 1)
                {
                    NarcRandoMod.Instance.VomitLock = false;
                }
            }
        }
    }
}