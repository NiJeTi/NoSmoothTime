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

    private static ManualLogSource _logger;
    private Harmony _harmony;

    private void Awake()
    {
        _logger = Logger;

        Enabled = Config.Bind(
            "General", "Enabled", true,
            "Master switch. When off, the daylight cycle keeps its stock once-per-second lighting update."
        );

        try
        {
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(typeof(LevelInfoUpdateTimeOfDayLightingPatch));
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to patch LevelInfo.UpdateTimeOfDayLighting: {e}");
            return;
        }

        _logger.LogInfo(
            $"{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION} is loaded."
        );
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}