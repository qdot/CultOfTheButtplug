using Buttplug.Client;
using HarmonyLib;
using UnityEngine;

namespace CultOfButtplug.Patches;

[HarmonyPatch]
public static class Patches
{


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Health), nameof(Health.DealDamage))]
    public static void Health_DealDamage(ref Health? __instance, ref float Damage, ref GameObject Attacker)
    {
        if (__instance is null) return;

        // Rules:
        // - Only vibrate on damage to player (PlayerTeam) if option is on.
        // - Only vibrate on damage to enemy (Team2) if option is on
        // - Otherwise, just ignore (usually damage to plants/neutral objects)
        if (!((__instance.team == Health.Team.PlayerTeam && Plugin.VibrateOnPlayerDamage) ||
            (__instance.team == Health.Team.Team2 && Plugin.VibrateOnAttack))
        )
        {
            return;
        }

        foreach (var device in Plugin.client.Devices)
        {
            if (device.VibrateAttributes.Count > 0)
            {
                var DamageClone = Damage;
                System.Threading.Tasks.Task.Run(async () =>
                {
                    await device.VibrateAsync(DamageClone / 5.0);
                    await System.Threading.Tasks.Task.Delay(100);
                    await device.Stop();
                });
            }
        }

        Plugin.L($"Deal {Damage} damage!");
    }
}