using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace NoSmoothTime.Patches;

[HarmonyPatch(typeof(LevelInfo), "UpdateTimeOfDayLighting")]
internal static class LevelInfoUpdateTimeOfDayLightingPatch
{
    private static readonly AccessTools.FieldRef<LevelInfo, float> TimeFactorRef =
        AccessTools.FieldRefAccess<LevelInfo, float>("timeFactor");

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Prefix(LevelInfo __instance, ref bool forceImmediate)
    {
        if (!Plugin.Enabled.Value)
        {
            return;
        }

        if (TimeFactorRef(__instance) < Plugin.MinTimeFactor.Value)
        {
            return;
        }

        forceImmediate = true;
    }
}