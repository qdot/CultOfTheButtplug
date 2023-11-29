using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CultOfButtplug.Patches;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace CultOfButtplug;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "bsf.cutofbuttplug";
    private const string PluginName = "Cult of the Buttplug";
    private const string PluginVer = "0.1.0";

    internal static ManualLogSource Log = null!;
    internal static readonly Harmony Harmony = new(PluginGuid);
    
    private void Awake()
    {
        Log = new ManualLogSource("Cult-of-the-Buttplug");
        BepInEx.Logging.Logger.Sources.Add(Log);
        Harmony.PatchAll();
    }

    public static void L(string message)
    {
        Log.LogWarning(message);
    }
}