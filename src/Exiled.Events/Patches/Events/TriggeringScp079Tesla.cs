// -----------------------------------------------------------------------
// <copyright file="TriggeringScp079Tesla.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events
{
#pragma warning disable SA1313
    using Exiled.Events.Handlers;
    using Exiled.Events.Handlers.EventArgs;

    using HarmonyLib;

    using UnityEngine;

    using Console = GameCore.Console;

    /// <summary>
    /// Patches <see cref="Scp079PlayerScript.CallCmdInteract(string, GameObject)"/>.
    /// Adds the <see cref="TriggeringScp079TeslaEventArgs"/> event.
    /// </summary>
    [HarmonyPatch(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.CallCmdInteract))]
    public class TriggeringScp079Tesla
    {
        /// <summary>
        /// Prefix of <see cref="Scp079PlayerScript.CallCmdInteract(string, GameObject)"/>.
        /// </summary>
        /// <param name="__instance">The <see cref="Scp079PlayerScript"/> instance.</param>
        /// <param name="command">The command to be executed.</param>
        /// <param name="target">The target game object.</param>
        /// <returns>Returns a value indicating whether the original method has to be executed or not.</returns>
        public static bool Prefix(Scp079PlayerScript __instance, string command, GameObject target)
        {
            if (!__instance._interactRateLimit.CanExecute() || !__instance.iAm079)
                return false;

            Console.AddDebugLog("SCP079", "Command received from a client: " + command, MessageImportance.LessImportant);

            if (!command.Contains(":"))
                return false;

            string[] strArray = command.Split(':');

            __instance.RefreshCurrentRoom();

            if (!__instance.CheckInteractableLegitness(__instance.currentRoom, __instance.currentZone, target, true))
                return false;

            string s = strArray[0];

            if (s != "TESLA")
                return true;
            float manaFromLabel2 = __instance.GetManaFromLabel("Tesla Gate Burst", __instance.abilities);
            if (manaFromLabel2 > (double)__instance.curMana)
            {
                __instance.RpcNotEnoughMana(manaFromLabel2, __instance.curMana);
                return false;
            }

            GameObject go1 = GameObject.Find(__instance.currentZone + "/" + __instance.currentRoom + "/Gate");
            if (!(go1 != null))
                return false;

            var tesla = go1.GetComponent<TeslaGate>();
            var ev = new TriggeringScp079TeslaEventArgs(Core.API.Features.Player.Get(__instance.gameObject), tesla);

            Player.OnTriggeringScp079Tesla(ev);

            if (!ev.IsAllowed)
                return false;

            tesla.RpcInstantBurst();

            __instance.AddInteractionToHistory(go1, strArray[0], true);
            __instance.Mana -= manaFromLabel2;

            return false;
        }
    }
}