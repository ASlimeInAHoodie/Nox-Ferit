using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NoxFerit.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoxFerit
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class NFBase : BaseUnityPlugin
    {
        private const string modGUID = "aslimeinahoodie.NoxFerit";
        private const string modName = "Nox Ferit";
        private const string modVersion = "1.2.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static NFBase Instance;

        internal new static ManualLogSource Logger;

        internal static bool hasNightStruck;
        internal static float normalizedTimeOfDay;
        internal static float currentDayTime;
        internal static float totalTime;
        internal static float scrapAmountMultiplier;
        internal static bool isDoubledJump;
        internal static bool isServer;
        internal static bool hasMonstersSpawned;

        internal static float Multiplier;
        internal static float nightStrikeTime;
        internal static int ventCount;


        void Awake()
        {
            hasNightStruck = false;
            hasMonstersSpawned = false;

            if (Instance == null)
            {
                Instance = this;
            }

            Logger = base.Logger;

            Logger.LogInfo("The Nox Oculus has opened...");

            harmony.PatchAll(typeof(NFBase));
            //harmony.PatchAll(typeof(PlayerControllerBCheats)); // <- Enable for god mode, infinite sprint, double jump
            //harmony.PatchAll(typeof(PlayerControllerBPatch)); // <- Enable for debugging
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(TimeOfDayPatch));

            var configMultiplier = base.Config.Bind(
                "General",
                "Multiplier",
                1.5f,
                "A multiplier to change how much scrap spawns, as well as how many enemies spawn. (1 = +100%, 2.5 = +250%)."
                );
            var configNightStrikeTime = base.Config.Bind(
                "General",
                "Night Strikes Time",
                6f,
                "Roughly how many hours have past (including 8 am) before Night Strikes (Range between 1 for 8am, 6 for 1pm, and 15 for 12am)."
                );

            Multiplier = configMultiplier.Value;
            nightStrikeTime = configNightStrikeTime.Value;

            if (nightStrikeTime > 15 || nightStrikeTime < 1)
            {
                nightStrikeTime = 11;
            }

        }
    }
}
