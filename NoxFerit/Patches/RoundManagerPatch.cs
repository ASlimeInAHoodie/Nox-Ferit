using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoxFerit.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        private static RoundManager instance;


        [HarmonyPatch("SpawnScrapInLevel")]
        [HarmonyPrefix]
        static void patchSpawnScrapInLevel(ref float ___scrapAmountMultiplier)
        {
            // Do nothing if not host
            if (!NFBase.isServer) { return; }
            //
            NFBase.hasNightStruck = false;
            NFBase.hasMonstersSpawned = false;
            NFBase.scrapAmountMultiplier = ___scrapAmountMultiplier;
            //log.AddLog(new EventLogMessage
            //{
            //    ObjectName = $"{playerName} ({instance.playerClientId})",
            //    Message = message,
            //    DetailsTitle = action,
            //    Details = details,
            //    Type = EventLogType.Player
            //})
        }


        [HarmonyPatch(typeof(RoundManager), "Update")]
        [HarmonyPostfix]
        static void updatePostPatch(RoundManager __instance)
        {
            // Do nothing if not host
            if (!NFBase.isServer) { return; }
            //
            if (NFBase.ventCount != __instance.allEnemyVents.Length)
            {
                NFBase.ventCount = __instance.allEnemyVents.Length;
                NFBase.Logger.LogDebug("allEnemyVents populated with count: " + NFBase.ventCount);
            }

            if (NFBase.hasNightStruck)
            {
                if (!NFBase.hasMonstersSpawned)
                {
                    NFBase.hasMonstersSpawned = true;
                    __instance.currentMaxInsidePower += NFBase.ventCount * NFBase.Multiplier;
                }
                if (__instance.currentEnemyPower < __instance.currentMaxInsidePower)
                {
                    __instance.PlotOutEnemiesForNextHour();
                    // Open all vents
                    for (int i = 0; i < __instance.allEnemyVents.Length; i++)
                    {
                        if (__instance.allEnemyVents[i].occupied)
                        {
                            __instance.SpawnEnemyFromVent(__instance.allEnemyVents[i]);
                            NFBase.Logger.LogDebug("Found enemy vent: " + __instance.allEnemyVents[i].gameObject.name + ". Spawning " + __instance.allEnemyVents[i].enemyType.enemyName + " from vent.");
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(RoundManager), "Start")]
        [HarmonyPostfix]
        static void StartPostPatch(RoundManager __instance)
        {
            NFBase.isServer = __instance.IsServer;
            // Do nothing if not host
            if (!NFBase.isServer) { return; }
            //
            // set scrap at start of game
            __instance.scrapAmountMultiplier += NFBase.Multiplier;

            // Possible scaling?
            //NFBase.extraEnemyPower += 1;
            //NFBase.scrapMultiplier += 0.1f;
        }
    }
}
