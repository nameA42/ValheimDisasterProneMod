using System;
using System.Collections.Generic;

namespace ValheimTwitch.Events
{
    // 0 None
    // 1 RavenMessage
    // 2 SpawnCreature
    // 3 HUD message
    // 4 Start event
    // 5 Environement
    // 6 Player action (puke, heal, ...)

    public static class Actions
    {
        internal static void RunAction(int typ = 2, bool ran = true)
        {
            try
            {
                var type = typ;

                var types = new List<int> { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2 };

                if(ran)
                {
                    type = types[UnityEngine.Random.Range(0, types.Count)];
                }

                switch (type)
                {
                    case 0:
                        SpawnCreatureAction.Run();
                        Log.Info("ran Spawn");
                        break;
                    case 1:
                        ChangeEnvironmentAction.Run();
                        Log.Info("ran Enviro");
                        break;
                    case 2:
                        PlayerAction.Run();
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
