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
        private const string modVersion = "1.1.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static NFBase Instance;

        internal new static ManualLogSource Logger;

        internal new static bool hasNightStruck;
        internal new static float normalizedTimeOfDay;
        internal new static float scrapAmountMultiplier;
        internal new static bool isDoubledJump;

        internal new static float scrapMultiplier;
        internal new static float nightStrikeTime;
        internal new static int extraEnemyPowerBase;
        internal new static int extraEnemyPower;


        void Awake()
        {
            hasNightStruck = false;

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

            var configScrapMultiplier = base.Config.Bind(
                "General",
                "Scrap Multiplier",
                3f,
                "Add this much to the multiplier of scrap spawned on moons (1 = +100%, 2.5 = +250%)."
                );
            var configNightStrikeTime = base.Config.Bind(
                "General",
                "Night Strikes Time",
                7f,
                "Roughly how many hours have past (including 8 am) before Night Strikes (Range between 1 for 8am, and 15 for 12am)."
                );
            var configEnemyPower = base.Config.Bind(
                "General",
                "Enemy Power",
                10,
                "Modify the spike of enemies spawned when Night Strikes."
                );

            scrapMultiplier = configScrapMultiplier.Value;
            nightStrikeTime = configNightStrikeTime.Value;
            extraEnemyPowerBase = configEnemyPower.Value;
            extraEnemyPower = extraEnemyPowerBase;

            if (nightStrikeTime > 15 || nightStrikeTime < 1)
            {
                nightStrikeTime = 11;
            }

        }
    }
}
