﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoxFerit.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch
    {
        [HarmonyPatch("MoveTimeOfDay")]
        [HarmonyPostfix]
        static void patchMoveTimeOfDay(ref float ___normalizedTimeOfDay)
        {
            // Night strikes
            // Configured with X * 0.066667, where X is the hours since & including start (8am = 1, 12pm = 5, 6pm = 11, 12am = 15)
            if (___normalizedTimeOfDay >= NFBase.nightStrikeTime * 0.06666 && !NFBase.hasNightStruck) {
                NFBase.Logger.LogInfo("Night strikes...");
                NFBase.hasNightStruck = true;
            }
            // Debug
            //NFBase.Logger.LogDebug(___normalizedTimeOfDay.ToString());
        }
    }
}