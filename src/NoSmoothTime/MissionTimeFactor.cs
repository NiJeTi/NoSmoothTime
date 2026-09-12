using HarmonyLib;
using NuclearOption.Networking;

namespace NoSmoothTime;

internal static class MissionTimeFactor
{
    private static readonly Dictionary<float, float> NetworkTimeFactors = new()
    {
        { -2f, 0f },
        { -1f, 0.5f },
        { 0f, 1f },
        { 1f, 10f },
        { 2f, 30f },
        { 3f, 60f },
    };

    public static readonly AccessTools.FieldRef<LevelInfo, float> TimeFactorRef =
        AccessTools.FieldRefAccess<LevelInfo, float>("timeFactor");

    private static bool IsServer
    {
        get
        {
            var manager = NetworkManagerNuclearOption.i;

            return manager != null && manager.Server != null && manager.Server.Active;
        }
    }

    public static float Current(LevelInfo levelInfo)
    {
        if (IsServer)
        {
            return TimeFactorRef(levelInfo);
        }

        var environment = MissionManager.CurrentMission?.environment;

        if (environment == null)
        {
            return TimeFactorRef(levelInfo);
        }

        return Resolve(environment.timeFactor);
    }

    private static float Resolve(float missionTimeFactor)
    {
        return NetworkTimeFactors.GetValueOrDefault(missionTimeFactor, 1f);
    }
}