//using UnityEngine;
//using ValheimTwitch.Patches;
//using System.Collections.Generic;

//namespace ValheimTwitch.Helpers
//{
//    public class Shortcut
//    {
//        public string Name { set; get; }
//        public string Label { set; get; }
//        public KeyCode Code { set; get; } = KeyCode.None;
//    }
//    static class CustomInput
//    {
//        public static List<Shortcut> shortcuts = new List<Shortcut> { 
//            new Shortcut { Name = "Whistle", Label = "Whistle", Code = KeyCode.Equals },
//            new Shortcut { Name = "Toggle", Label = "Toggle", Code = KeyCode.Minus }
//        };
//        public static KeyCode GetCode(string name)
//        {
//            return shortcuts.Find(shortcut => shortcut.Name == name).Code;
//        }

//        public static bool GetKey(string name)
//        {
//            return Input.GetKey(GetCode(name));
//        }

//        public static bool GetKeyDown(string name)
//        {
//            return Input.GetKeyDown(GetCode(name));
//        }
//    }
//}
