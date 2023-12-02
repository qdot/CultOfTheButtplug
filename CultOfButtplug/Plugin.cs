using BepInEx;
using BepInEx.Logging;
using Buttplug.Client.Connectors.WebsocketConnector;
using HarmonyLib;
using COTL_API.CustomFollowerCommand;
using System.IO;
using Buttplug.Client;
using BepInEx.Configuration;
using COTL_API.CustomSettings;
using System.Threading;
using System.Threading.Tasks;

namespace CultOfButtplug;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("io.github.xhayper.COTL_API")]
[HarmonyPatch]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "bsf.cutofbuttplug";
    private const string PluginName = "Cult of the Buttplug";
    private const string PluginVer = "0.2.0";
    public static ButtplugClient client = new ButtplugClient("Cult of the Buttplug");
    internal static string PluginPath;
    internal static FollowerCommands? FollowerCommand;
    internal static ManualLogSource Log = null!;
    internal static readonly Harmony Harmony = new(PluginGuid);

    private static ConfigEntry<bool>? _allowIntifaceConnection { get; set; }
    public static bool AllowIntifaceConnection => _allowIntifaceConnection?.Value ?? true;


    private static ConfigEntry<bool>? _vibrateOnAttack { get; set; }
    public static bool VibrateOnAttack => _vibrateOnAttack?.Value ?? false;

    private static ConfigEntry<bool>? _vibrateOnPlayerDamage { get; set; }
    public static bool VibrateOnPlayerDamage => _vibrateOnPlayerDamage?.Value ?? true;

    private ConfigEntry<bool>? _allowButtplugInteraction { get; set; }
    public bool AllowButtplugInteraction => _allowButtplugInteraction?.Value ?? true;

    private CancellationTokenSource _connectionTokenSource = new CancellationTokenSource();

    private void Awake()
    {
        PluginPath = Path.GetDirectoryName(Info.Location);
        Log = new ManualLogSource("Cult-of-the-Buttplug");
        BepInEx.Logging.Logger.Sources.Add(Log);
        Plugin.L($"Trying to connect client!");
        Plugin.client.ConnectAsync(new ButtplugWebsocketConnector(new System.Uri("ws://127.0.0.1:12345")));
        Plugin.L($"Client connect called!");
        Harmony.PatchAll();
        Plugin.L($"Custom follower command added!");

        _allowIntifaceConnection = Config.Bind("Cult of the Buttplug", "Connect to Intiface Central", true,
            "Allow connection to Intiface Central for controlling sex toys.");
        _vibrateOnAttack = Config.Bind("Cult of the Buttplug", "Vibrate on Attack", true,
            "Vibrate whenever enemy is damaged from player attack.");
        _vibrateOnPlayerDamage = Config.Bind("Cult of the Buttplug", "Vibrate on Player Damage", true,
            "Vibrate whenever player takes damage.");
        _allowButtplugInteraction = Config.Bind("Cult of the Buttplug", "Allow Fornication Follower Interaction", true,
            "Turn on follower interaction that will increase adoration/loyalty and control toys.");

        CustomSettingsManager.AddBepInExConfig("Cult of the Buttplug", "Allow Initface Central Connection", _allowIntifaceConnection, handleAllowIntifaceConnectionUpdate);
        CustomSettingsManager.AddBepInExConfig("Cult of the Buttplug", "Vibrate On Attack", _vibrateOnAttack);
        CustomSettingsManager.AddBepInExConfig("Cult of the Buttplug", "Vibrate On Player Damage", _vibrateOnPlayerDamage);
        CustomSettingsManager.AddBepInExConfig("Cult of the Buttplug", "Allow Follower Fornication interaction", _allowButtplugInteraction, handleAllowFollowerFornicationUpdate);

        // Run config delegates now in order to establish base settings.
        handleAllowFollowerFornicationUpdate(AllowButtplugInteraction);
        handleAllowIntifaceConnectionUpdate(AllowIntifaceConnection);
    }

    private void handleAllowIntifaceConnectionUpdate(bool value)
    {
        if (value)
        {
            _connectionTokenSource = new CancellationTokenSource();
            var cancellationToken = _connectionTokenSource.Token;
            System.Threading.Tasks.Task.Run(async () =>
            {
                Plugin.L("Starting Intiface Central Connection Loop");
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!client.Connected)
                    {
                        try
                        {
                            await client.ConnectAsync(new ButtplugWebsocketConnector(new System.Uri("ws://127.0.0.1:12345")), cancellationToken);
                        }
                        catch
                        {
                            // Wait a bit and try again
                            await System.Threading.Tasks.Task.Delay(2000);
                        }
                    }
                    else
                    {
                        // Set a cancellation waiter based on the disconnect event.
                        var disconnectWaiter = new TaskCompletionSource<bool>();
                        client.ServerDisconnect += (s, e) => { disconnectWaiter.SetResult(true); };
                        //System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[] { disconnectWaiter.Task };
                        await disconnectWaiter.Task;
                        Plugin.L("Intiface Central disconnected.");
                        //System.Threading.Tasks.Task.WhenAny(disconnectWaiter.Task, cancellationToken);
                    }
                }
                Plugin.L("Stopping Intiface Central Connection Loop");
            });
        }
        else
        {
            if (client.Connected)
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await client.DisconnectAsync();
                });
            }
            _connectionTokenSource.Cancel();
            _connectionTokenSource.Dispose();
        }
    }

    private void handleAllowFollowerFornicationUpdate(bool value)
    {
        if (value)
        {
            Plugin.L("Adding Buttplug Interaction Command.");
            FollowerCommand = CustomFollowerCommandManager.Add(new ButtplugFollowerCommand());
        }
        else
        {
            if (FollowerCommand is not null)
            {
                Plugin.L("Removing Buttplug Interacton Command.");
                // Casting nullability away. Not sure why ! isn't working here. Older C# version?
                CustomFollowerCommandManager.CustomFollowerCommandList.Remove((FollowerCommands)FollowerCommand);
            }
        }
    }

    public static void L(string message)
    {
        Log.LogWarning(message);
    }
}