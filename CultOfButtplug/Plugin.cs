using BepInEx;
using BepInEx.Logging;
using Buttplug.Client.Connectors.WebsocketConnector;
using HarmonyLib;
using COTL_API.CustomFollowerCommand;
using System.IO;
using COTL_API.Debug;
using Buttplug.Client;

namespace CultOfButtplug;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API")]
[HarmonyPatch]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "bsf.cutofbuttplug";
    private const string PluginName = "Cult of the Buttplug";
    private const string PluginVer = "0.1.0";
    public static ButtplugClient client = new ButtplugClient("Cult of the Buttplug");
    internal static string PluginPath;
    internal static FollowerCommands FollowerCommand;
    internal static FollowerCommands FollowerCommand2;
    internal static ManualLogSource Log = null!;
    internal static readonly Harmony Harmony = new(PluginGuid);
    
    private void Awake()
    {
        PluginPath = Path.GetDirectoryName(Info.Location);
        Log = new ManualLogSource("Cult-of-the-Buttplug");
        BepInEx.Logging.Logger.Sources.Add(Log);
        Plugin.L($"Trying to connect client!");
        Plugin.client.ConnectAsync(new ButtplugWebsocketConnector(new System.Uri("ws://127.0.0.1:12345")));
        Plugin.L($"Client connect called!");
        Harmony.PatchAll();
        FollowerCommand = CustomFollowerCommandManager.Add(new ButtplugFollowerCommand());
        Plugin.L($"Custom follower command added!");
    }

    public static void L(string message)
    {
        Log.LogWarning(message);
    }
}