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
        foreach (var device in Plugin.client.Devices) {
            if (device.VibrateAttributes.Count > 0) {
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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Lunge), typeof(float), typeof(float))]
    public static void PlayerController_Lunge(ref float lungeDuration, ref float lungeSpeed)
    {
        Plugin.L($"Lunge {lungeDuration} {lungeSpeed}!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.DoIslandDash), typeof(Vector3))]
    public static void PlayerController_DoIslandDash(ref PlayerController __instance)
    {
        Plugin.L($"Dodge DoIslandDash {__instance.DodgeSpeed}!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerFarming), nameof(PlayerFarming.DodgeRoll))]
    public static void PlayerFarming_DodgeRoll(ref PlayerFarming __instance)
    {
        // This is constant
        //Plugin.L($"Dodge DodgeRoll {__instance.playerController.DodgeSpeed}!");
    }
}