using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using UnityEngine;
using System.IO;

namespace CultOfButtplug
{
    internal class ButtplugFollowerCommand : CustomFollowerCommand
    {
        public override string InternalName => "Buttplug_Follower_Command";
        public override string GetTitle(Follower follower) => "Fornicate With Follower";
        public override string GetDescription(Follower follower) => "Bring follower Closer To God. Biblically. With Haptics.";
        
        public override Sprite CommandIcon => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "buttplug.png"));
        //public override string CommandStringIcon() => "<sprite name=\"icon_Poop\">";

        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            System.Threading.Tasks.Task.Run(async () =>
            {
                    foreach (var device in Plugin.client.Devices)
                    {
                        if (device.VibrateAttributes.Count > 0)
                        {
                            await System.Threading.Tasks.Task.Delay(500);
                            await device.VibrateAsync(1.0);
                            await System.Threading.Tasks.Task.Delay(500);
                            await device.Stop();
                        }
                    }
            });
            interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
            {
                interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);
                interaction.follower.Brain.HardSwapToTask(new FollowerTask_Vomit());
            }));
            interaction.Close(true, reshowMenu: false);
        }
    }
    internal class ButtplugInteraction
    {
    }
}
