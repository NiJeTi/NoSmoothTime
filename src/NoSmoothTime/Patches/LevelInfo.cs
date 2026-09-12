using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

namespace NoSmoothTime.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[HarmonyPatch(typeof(LevelInfo), "UpdateTimeOfDayLighting")]
internal static class LevelInfo_UpdateTimeOfDayLighting
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Prefix(LevelInfo __instance, ref bool forceImmediate)
    {
        if (!Plugin.SmoothingEnabled.Value)
        {
            return;
        }

        var timeFactor = MissionTimeFactor.Current(__instance);

        if (NightSpeed.Effective(timeFactor, __instance.timeOfDay) < Plugin.SmoothingMinTimeFactor.Value)
        {
            return;
        }

        forceImmediate = true;
    }
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
[HarmonyPatch(typeof(LevelInfo), "UpdateSimulation")]
internal static class LevelInfo_UpdateSimulation
{
    private static float _baseTimeFactor;
    private static bool _scaled;

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Prefix(LevelInfo __instance)
    {
        var baseTimeFactor = MissionTimeFactor.TimeFactorRef(__instance);
        var effective = NightSpeed.Effective(baseTimeFactor, __instance.timeOfDay);

        if (effective.Equals(baseTimeFactor))
        {
            return;
        }

        _baseTimeFactor = baseTimeFactor;
        _scaled = true;
        MissionTimeFactor.TimeFactorRef(__instance) = effective;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void Finalizer(LevelInfo __instance)
    {
        if (!_scaled)
        {
            return;
        }

        MissionTimeFactor.TimeFactorRef(__instance) = _baseTimeFactor;
        _scaled = false;
    }
}