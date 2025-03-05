using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ValheimTwitch.Events
{
    internal class ChangeEnvironmentAction
    {
        internal static void Run()
        {
            var duration = 1;


            if (EnvMan.instance == null)
                return;

            //Log.Info($"Env -> {name} {duration}");

            List<string> envs = new List<string>
            {
                "SnowStorm", "ThunderStorm"
            };
            
            int index = UnityEngine.Random.Range(0, envs.Count);
            EnvMan.instance.m_debugEnv = envs[index];
            Log.Info(envs[index]);

            try
            {
                if (Player.m_localPlayer != null)
                {
                    var type = MessageHud.MessageType.Center;
                    var msg = $"There is bad weather incoming.";

                    Player.m_localPlayer.Message(type, $"{msg}\n{envs[index]}");
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