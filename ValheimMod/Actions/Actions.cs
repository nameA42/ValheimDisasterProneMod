﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ValheimTwitch.Events
{
    // 0 None
    // 1 RavenMessage
    // 2 SpawnCreature
    // 3 HUD message
    // 4 Start event
    // 5 Environement
    // 6 Player action (puke, heal, ...)

    public class Act
    {
        public string act { get; set; }
        public float odds { get; set; }
        public string type { get; set; }
        public Act()
        {
            this.act = "default";
            this.odds = 0f;
            this.type = "default";

        }
        public Act(string act, float odds, string type)
        {
            this.act = act;
            this.odds = odds;
            this.type = type;
        }
        public Act(string act, double odds, string type)
        {
            this.act = act;
            this.odds = (float)odds;
            this.type = type;
        }
        public Act(string act, int odds, string type)
        {
            this.act = act;
            this.odds = (float)odds;
            this.type = type;
        }
    }

    public class ActionsHelper
    {

        public List<Act> actions = new List<Act>
        {
            new Act("puke", 1f, "player"), new Act("heal", 1f, "player"), new Act("meteor", 1f, "player"), 
            new Act("baby", 1f, "player"), new Act("HS", 1f, "player"), new Act("warp", 1f, "player"), 
            new Act("moongrav", 1f, "player"),
            new Act("fish", 1f, "spawn"), new Act("skeleton", 1f, "spawn"), new Act("troll", 1.2f, "spawn"), new Act("dragon", 0.1f, "spawn"),
            new Act("thunderstorm", 1f, "weather"), new Act("blizzard", 0.6f, "weather")
        };
        public Act PickAction()
        {
            float probSum = actions.Sum(p => p.odds);
            float probDecision = UnityEngine.Random.Range(0, probSum);
            Act choice = new Act();
            foreach (Act a in actions)
            {
                probDecision -= a.odds;

                if(probDecision <= 0)
                {
                    choice = a;
                    break;
                }
            }
            return choice;
        }
    }

    public static class Actions
    {
        public static void writeToFile(string s)
        {
            string path = Directory.GetCurrentDirectory() + "/DisasterProne.txt";
            Log.Warning("Wrote to path: " + path);
            File.WriteAllLines(path, new string[]{s});
        }
        internal static void RunAction(string name = "")
        {
            try
            {
                var help = new ActionsHelper();
                var typ = help.PickAction();
                if (name != "")
                {
                    typ = help.actions.Find(x => x.act == name);
                }
                writeToFile($"We Got {typ.act}");
                switch (typ.type)
                {
                    case "spawn":
                        SpawnCreatureAction.Run(typ.act);
                        Log.Info("ran Spawn");
                        break;
                    case "weather":
                        ChangeEnvironmentAction.Run(typ.act);
                        Log.Info("ran Enviro");
                        break;
                    case "player":
                        PlayerAction.Run(typ.act);
                        Log.Info("ran Player");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception ex)
            {
                Log.Error("RunAction Error >>> " + ex.ToString());
            }
        }
    }
}
