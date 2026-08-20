using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using NoSmoothTime.Patches;

namespace NoSmoothTime;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal sealed class Plugin : BaseUnityPlugin
{
    public static ConfigEntry<bool> Enabled { get; private set; }
    public static ConfigEntry<float> MinTimeFactor { get; private set; }

    private Harmony _harmony;

    private void Awake()
    {
        Enabled = Config.Bind(
            "General", "Enabled", true,
            "Master switch. When off, the daylight cycle keeps its stock once-per-second lighting update."
        );

        MinTimeFactor = Config.Bind(
            "General", "MinTimeFactor", 10f,
            new ConfigDescription(
                "Smoothing only applies when the mission's time factor is at least this value.",
                new AcceptableValueList<float>(0f, 0.5f, 1f, 10f, 30f, 60f)
            )
        );

        try
        {
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(typeof(LevelInfoUpdateTimeOfDayLightingPatch));
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to patch LevelInfo.UpdateTimeOfDayLighting: {e}");
            return;
        }

        Logger.LogInfo(
            $"{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION} is loaded."
        );
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}