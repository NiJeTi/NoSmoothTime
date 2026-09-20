using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace NoSmoothTime;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal sealed class Plugin : BaseUnityPlugin
{
    private const string SectionSmoothing = "Smoothing";
    private const string SectionNightSkip = "Night Skip";

    public static ConfigEntry<bool> SmoothingEnabled { get; private set; }
    public static ConfigEntry<float> SmoothingMinTimeFactor { get; private set; }

    public static ConfigEntry<bool> NightEnabled { get; private set; }
    public static ConfigEntry<Vector2> NightRange { get; private set; }
    public static ConfigEntry<float> NightMultiplier { get; private set; }

    private Harmony _harmony;

    private void Awake()
    {
        SmoothingEnabled = Config.Bind(
            SectionSmoothing, "Enabled", true,
            "Master switch. When off, the daylight cycle keeps its stock once-per-second lighting update."
        );

        SmoothingMinTimeFactor = Config.Bind(
            SectionSmoothing, "MinTimeFactor", 10f,
            new ConfigDescription(
                "Smoothing only applies when the mission's time factor is at least this value.",
                new AcceptableValueList<float>(0f, 0.5f, 1f, 10f, 30f, 60f)
            )
        );

        NightEnabled = Config.Bind(
            SectionNightSkip, "Enabled", true,
            "Speeds the clock up at night."
        );

        NightRange = Config.Bind(
            SectionNightSkip, "Range", new Vector2(18f, 6f),
            new ConfigDescription(
                "Night window, in hours.",
                new AcceptableValueList<Vector2>(new Vector2(18f, 6f), new Vector2(19f, 5f))
            )
        );

        NightMultiplier = Config.Bind(
            SectionNightSkip, "Multiplier", 12f,
            new ConfigDescription(
                "How much faster the clock runs at night, relative to the mission's own time factor.",
                new AcceptableValueRange<float>(1f, 100f)
            )
        );

        try
        {
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll();
        }
        catch (Exception e)
        {
            Logger.LogError($"Failed to patch: {e}");
            return;
        }

        Logger.LogInfo("Patch successful");
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}