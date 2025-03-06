using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimTwitch.Events
{
    internal class EnviroActions
    {
        public string Name { get; set; }
        public string Command { get; set; }

        public EnviroActions() { }

        public EnviroActions(string name, string command)
        {
            Name = name;
            Command = command;
        }
    }

    internal class ChangeEnvironmentAction
    {
        private static List<EnviroActions> enviroActions = new List<EnviroActions>
            {
                new EnviroActions("blizzard", "SnowStorm"),
                new EnviroActions("thunderstorm", "ThunderStorm")
            };

        internal static void Run(string act)
        {
            var duration = 1;

            if (EnvMan.instance == null)
                return;

            //Log.Info($"Env -> {name} {duration}");

            int index = UnityEngine.Random.Range(0, enviroActions.Count);
            EnviroActions selectedAction = enviroActions[index];
            if (!string.IsNullOrEmpty(act))
            {
                selectedAction = enviroActions.Find(x => x.Name == act);
            }
            EnvMan.instance.m_debugEnv = selectedAction.Command;
            Log.Info(selectedAction.Name);

            try
            {
                if (Player.m_localPlayer != null)
                {
                    var type = MessageHud.MessageType.Center;
                    var msg = $"There is bad weather incoming.";

                    Player.m_localPlayer.Message(type, $"{msg}\n{selectedAction.Name}");
                }

                if (duration > 0)
                {
                    Task.Delay(duration * 60000).ContinueWith(t => EnvMan.instance.m_debugEnv = "");
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex.StackTrace);
            }
        }
    }
}