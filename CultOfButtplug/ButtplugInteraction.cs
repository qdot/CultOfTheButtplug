using COTL_API.CustomFollowerCommand;
using COTL_API.Helpers;
using UnityEngine;
using System.IO;

namespace CultOfButtplug
{
    internal class ExampleFollowerCommand : CustomFollowerCommand
    {
        public override string InternalName => "Example_Follower_Command_2";
        public override string GetTitle(Follower follower) { return "Example Follower Command 2"; }
        public override string GetDescription(Follower follower) { return "This is an example follower command 2"; }
        //public override Sprite CommandIcon => TextureHelper.CreateSpriteFromPath(Path.Combine(Plugin.PluginPath, "Assets", "placeholder.png"));

        public override void Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
        {
            interaction.StartCoroutine(interaction.FrameDelayCallback(delegate
            {
                interaction.eventListener.PlayFollowerVO(interaction.generalAcknowledgeVO);
                interaction.follower.Brain.HardSwapToTask(new FollowerTask_Vomit());
            })); 
            interaction.follower.Brain.MakeOld();
        }
    }
    internal class ButtplugInteraction
    {
    }
}
