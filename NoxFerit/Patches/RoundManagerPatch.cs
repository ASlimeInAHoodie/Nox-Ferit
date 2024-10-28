using HarmonyLib;
using System;
using System.Collections.Generic;
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
            NFBase.hasNightStruck = false;
            NFBase.extraEnemyPower = NFBase.extraEnemyPowerBase;
            NFBase.scrapAmountMultiplier = ___scrapAmountMultiplier;
        }
        

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void patchUpdate(ref float ___currentMaxOutsidePower, ref float ___currentOutsideEnemyPower, ref EnemyVent[] ___allEnemyVents, ref float ___currentEnemyPower, ref float ___currentMaxInsidePower)
        {
            if (NFBase.hasNightStruck)
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<RoundManager>();
                } else
                {
                    if (NFBase.extraEnemyPower > 0)
                    {
                        ___currentMaxInsidePower += NFBase.extraEnemyPower;
                        NFBase.extraEnemyPower = 0;
                    }
                    if (___currentEnemyPower < ___currentMaxInsidePower)
                    {
                        instance.PlotOutEnemiesForNextHour();
                        // Open all vents
                        for (int i = 0; i < ___allEnemyVents.Length; i++)
                        {
                            if (___allEnemyVents[i].occupied)
                            {
                                instance.SpawnEnemyFromVent(___allEnemyVents[i]);
                                NFBase.Logger.LogDebug("Found enemy vent: " + ___allEnemyVents[i].gameObject.name + ". Spawning " + ___allEnemyVents[i].enemyType.enemyName + " from vent.");
                            }
                        }
                    }
                        


                }
            }
        }

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPatch(ref float ___scrapAmountMultiplier)
        {
            // set scrap at start of game
            ___scrapAmountMultiplier += NFBase.scrapMultiplier;

            // Possible scaling?
            //NFBase.extraEnemyPower += 1;
            //NFBase.scrapMultiplier += 0.1f;
        }

        //DespawnPropsAtEndOfRound
        [HarmonyPatch("UnloadSceneObjectsEarly")]
        [HarmonyPostfix]
        static void UnloadSceneObjectsEarlyPatch()
        {
            instance = null;
        }
    }
}
