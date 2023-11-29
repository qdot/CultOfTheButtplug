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
        Plugin.L($"Deal {Damage} damage!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Lunge), typeof(float), typeof(float))]
    public static void PlayerController_Lunge(ref float lungeDuration, ref float lungeSpeed)
    {
        Plugin.L($"Lunge {lungeDuration} {lungeSpeed}!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.DoGrapple), typeof(Interaction_Grapple))]
    public static void PlayerController_DoGraple(ref Interaction_Grapple grapple)
    {
        Plugin.L($"Grapple!");
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