using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using ValheimTwitch;
using ValheimTwitch.Events;
using ValheimTwitch.Helpers;
using ValheimTwitch.Patches;
using System.Collections.Generic;
using static Interpolate;
using UnityEngine.PlayerLoop;

namespace NarcRandomMod
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class NarcRandoMod : BaseUnityPlugin
    {
        public bool isHuginIntroShown = false;
        public const string pluginGUID = "narc.Random.Mod";
        public const string pluginName = "NarcRandomMod";
        public const string pluginVersion = "1.0.0";

        public bool isInGame = false;
        public bool isEnabled = false;
        public bool timeshifted = false;
        public static bool isRewardUpdating = false;
        public float timscal = 1.0f;

        public float Fulldelay = 120f;
        public float delay = 0f;

        public bool fishig = false;
        public bool fishigLock = false;
        public bool skel = false;
        public bool skelLock = false;
        public bool Met = false;
        public bool MetLock = false;
        public bool Smol = false;
        public bool timMsgLock = false;
        public bool HS = false;
        public bool HSLock = false;
        public bool Warping = false;
        public bool WarpingLock = false;
        public bool Vommiting = false;
        public bool VomitLock = false;
        public bool falldmg = true;

        public int worldLevel = 1;

        public List<Character> currentMobs = new List<Character>();

        private static NarcRandoMod instance;

        public static NarcRandoMod Instance
        {
            get => instance;
            private set { instance = value; }
        }

        private readonly Harmony HarmonyInstance = new Harmony(pluginGUID);

        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);

        public void Awake()
        {
            EmbeddedAsset.LoadAssembly("Assets.ValheimTwitchGUI.dll");
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }

            instance = this;

            NarcRandoMod.logger.LogInfo("Thank you for using my mod!");

            SceneManager.activeSceneChanged += OnSceneChanged;

            Assembly assembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(assembly);
            Log.Info("Awakened");
        }

        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        public static class PlayerFUAdd
        {
            public static void Postfix(Player __instance)
            {
                //if (Input.GetKeyDown(KeyCode.LeftBracket))
                //{
                //    Game.m_timeScale *= 2.0f;
                //    Log.Info("Zoom" + Game.m_timeScale);
                //}
                //if (Input.GetKeyDown(KeyCode.RightBracket))
                //{
                //    Game.m_instance.m_timeScale /= 2.0f;
                //    Log.Info("Zome" + Game.m_timeScale);
                //}
                if (Input.GetKeyDown(KeyCode.Backslash))
                {
                    ConsoleUpdatePatch.AddAction(PlayerAction.Meteor);
                    Log.Info("Meteor");
                }
                if (Input.GetKeyDown(KeyCode.KeypadDivide))
                {
                    Actions.RunAction();
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    PlayerAction.Warp();
                    Log.Info("Warp");
                }
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    ConsoleUpdatePatch.AddAction(PlayerAction.HealSurroundings);
                    Log.Info("HS");
                }
                if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    Log.Info("MoonJump");
                    ConsoleUpdatePatch.AddAction(PlayerAction.Moon);
                }
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    SpawnCreatureAction.Run();
                    Log.Info("Spawn");
                }
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    PlayerAction.Run();
                    Log.Info("Pact");
                }
                if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    PlayerAction.doMSG("Increasing World Level");
                    Actions.incrementUp();
                    Log.Info("LevelUp");
                }
                if (Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    Actions.incrementDown();
                    Log.Info("LevelDown");
                }
                if (Input.GetKeyDown(KeyCode.KeypadMultiply))
                {
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
                //Time.timeScale = Instance.timscal;
                //Log.Info("updating");
                if (instance.isInGame & instance.isEnabled)
                {
                    instance.delay += Time.deltaTime;
                    //Log.Info("Ingame" + instance.delay);
                }
                if (instance.delay >= instance.Fulldelay)
                {
                    Actions.RunAction();
                    instance.delay = 0f;
                    if(instance.Smol)
                    {
                        Player.m_localPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
                        instance.Smol = false;
                    }
                }

                if (!instance.timMsgLock & Player.m_localPlayer != null & instance.delay%10 > 9.95)
                {
                    var type1 = MessageHud.MessageType.TopLeft;
                    var msg = $"Time Till next Disaster:" + (int)(120f - instance.delay);

                    Player.m_localPlayer.Message(type1, $"{msg}\n");
                    instance.timMsgLock = true;
                }
                if (instance.timMsgLock & Player.m_localPlayer != null & instance.delay % 10 < 0.5)
                {
                    instance.timMsgLock = false;
                }
            }
        }
        private void OnSceneChanged(Scene current, Scene next)
        {
            isInGame = next.name == "main";

            Toggle(isInGame);
        }
        public void Toggle(bool enable = true)
        {
            isEnabled = enable;
            var type1 = MessageHud.MessageType.TopLeft;
            var msg = $"Timer is " + isEnabled;

            Player.m_localPlayer.Message(type1, $"{msg}\n");
        }

        public void Toggle()
        {
            isEnabled = !isEnabled;
            var type1 = MessageHud.MessageType.TopLeft;
            var msg = $"Timer is " + isEnabled;

            Player.m_localPlayer.Message(type1, $"{msg}\n");
        }

    }
}