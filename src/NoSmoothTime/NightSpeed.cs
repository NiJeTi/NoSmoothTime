namespace NoSmoothTime;

internal static class NightSpeed
{
    public static float Effective(float baseTimeFactor, float timeOfDay)
    {
        if (!Plugin.NightEnabled.Value || !IsNight(timeOfDay))
        {
            return baseTimeFactor;
        }

        return baseTimeFactor * Plugin.NightMultiplier.Value;
    }
    
    private static bool IsNight(float timeOfDay)
    {
        var nightRange = Plugin.NightRange.Value;
        var nightRangeFrom = nightRange.x;
        var nightRangeTo = nightRange.y;
        
        return timeOfDay >= nightRangeFrom || timeOfDay < nightRangeTo;
    }
}
