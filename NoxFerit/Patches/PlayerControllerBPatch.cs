using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoxFerit.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBCheats
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void patchUpdate(ref float ___sprintMeter)
        {
            // Infinite sprinting
            if (___sprintMeter < 1.0f) {
                //___sprintMeter += 0.0002f;
                ___sprintMeter = 1.0f;
            }
            // Debug
            //NFBase.Logger.LogDebug("Sprint: " + ___sprintMeter.ToString());
        }

        [HarmonyPatch("DamagePlayer")]
        [HarmonyPrefix]
        static void patchDamagePlayer(ref int damageNumber, PlayerControllerB __instance)
        {
            // God Mode for testing
            damageNumber = 0;
            NFBase.Logger.LogInfo("Godmode saved you from taking damage");
        }

        [HarmonyPatch("KillPlayer")]
        [HarmonyPrefix]
        static void patchKillPlayerPrefix(ref bool ___isPlayerDead)
        {
            // God Mode for testing
            ___isPlayerDead = true;
        }
        [HarmonyPatch("KillPlayer")]
        [HarmonyPostfix]
        static void patchKillPlayerPostfix(ref bool ___isPlayerDead)
        {
            // God Mode for testing
            ___isPlayerDead = false;
            NFBase.Logger.LogInfo("Godmode saved you from dying");
        }


        [HarmonyPatch("AllowPlayerDeath")]
        [HarmonyPrefix]
        static bool patchAllowPlayerDeath()
        {
            NFBase.Logger.LogInfo("Godmode saved you from dying");
            return false;
        }

        /*[HarmonyPatch("Jump_performed")]
        [HarmonyPrefix]
        static void patchJump_performed(ref bool ___isJumping, ref CharacterController ___thisController)
        {
            // Double Jump
            if (!___isJumping && ___thisController.isGrounded)
            {
                NFBase.isDoubledJump = false;
            } else if (___isJumping && !NFBase.isDoubledJump)
            {
                NFBase.isDoubledJump = true;
                ___isJumping = false;
            }
        }*/
    }

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        [HarmonyPatch("PlayerJump")]
        [HarmonyPrefix]
        static void patchPlayerJump()
        {
            NFBase.Logger.LogDebug(
                " timeLeft: " + (((NFBase.nightStrikeTime * 0.06666) - NFBase.normalizedTimeOfDay) * 900).ToString() + 
                " \n normalizedTimeOfDay: " + NFBase.normalizedTimeOfDay + 
                " \n currentDatTime / totalTime: " + NFBase.currentDayTime.ToString() + "/" + NFBase.totalTime.ToString() + 
                " \n hasNightStruck: " + NFBase.hasNightStruck + 
                " \n scrapAmountMultiplier: " + NFBase.scrapAmountMultiplier +
                " \n configMultiplier: " + NFBase.Multiplier + 
                " \n configNightStrikeTime: " + NFBase.nightStrikeTime + 
                " \n ventCount: " + NFBase.ventCount + 
                " \n isServer: " + NFBase.isServer
                );
        }
    }
}
