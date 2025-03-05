//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using ValheimTwitch.Helpers;

//namespace ValheimTwitch.Patches
//{
//    [HarmonyPatch(typeof(FejdStartup), "Update")]
//    public static class FejdStartupUpdatePatch
//    {
//        public static bool updateUI = false;

//        public static void Postfix()
//        {
//            // TODO use action queue
//            if (updateUI)
//            {
//                FejdStartupStartPatch.UpdateRewardGrid();
//                FejdStartupStartPatch.UpdateMainButonText();
//            }

//            updateUI = false;
//        }
//    }

//    public class Shortcut
//    {
//        public string Name { set; get; }
//        public string Label { set; get; }
//        public KeyCode Code { set; get; } = KeyCode.None;
//    }

//    [HarmonyPatch(typeof(FejdStartup), "Start")]
//    public static class FejdStartupStartPatch
//    {
//        public static GameObject gui;
//        public static AssetBundle guiBundle;
//        public static ValheimTwitchGUIScript guiScript;

//        public static List<Shortcut> shortcuts = new List<Shortcut> { 
//            new Shortcut { Name = "Whistle", Label = "Whistle" },
//            new Shortcut { Name = "ToggleAllRewards", Label = "Toggle all rewards" }
//        };
        
//        public static void LoadShortcuts()
//        {
//            foreach (var shortcut in shortcuts)
//            {
//                shortcut.Code = (KeyCode)PluginConfig.GetInt($"shortcut-{shortcut.Name}");

//                Log.Info($"Load shortcut {shortcut.Name} -> {shortcut.Code}");

//                guiScript.settingsPanel.AddKeyInput(shortcut.Label, shortcut.Code, (object sender, KeyCodeArgs args) => {
//                    PluginConfig.SetInt($"shortcut-{shortcut.Name}", (int)args.Code);
//                    shortcut.Code = args.Code;
//                });
//            }
//        }

//        public static void Postfix(FejdStartup __instance)
//        {
//            var mainGui = __instance.m_mainMenu;
//            guiBundle = guiBundle ?? EmbeddedAsset.LoadAssetBundle("Assets.valheimtwitchgui");
//            var prefab = guiBundle.LoadAsset<GameObject>("Valheim Twitch GUI");

//            gui = UnityEngine.Object.Instantiate(prefab);
//            gui.transform.SetParent(mainGui.transform);

//            guiScript = gui.GetComponent<ValheimTwitchGUIScript>();

//            guiScript.mainButton.OnClick(() => OnMainButtonClick());
//            guiScript.refreshRewardButton.OnClick(() =>
//            {
//                UpdateRewardGrid();
//            });

//            UpdateMainButonText();
//            UpdateRewardGrid();
//            guiScript.updatePanel.SetActive(true);
//            LoadShortcuts();
//        }
//        private static void OnNewRewardSave()
//        {
//            return;
//        }

//        private static void OnRewardSettingschanged()
//        {
//            UpdateRewardGrid();
//        }

//        private static void OnMainButtonClick()
//        {
//            guiScript.mainPanel.ToggleActive();
//        }

//        public static void UpdateMainButonText()
//        {

//            guiScript?.mainButton.SetText("Connexion poop?");
//        }

//        public static void UpdateRewardGrid(string newRewardId = null)
//        {
//            return;
//        }
//    }
//}

