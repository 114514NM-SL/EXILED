// -----------------------------------------------------------------------
// <copyright file="Decontaminating.cs" company="Exiled Team">
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

    using LightContainmentZoneDecontamination;

    /// <summary>
    /// Patches <see cref="DecontaminationController.FinishDecontamination"/>.
    /// Adds the <see cref="Map.Decontaminating"/> event.
    /// </summary>
    [HarmonyPatch(typeof(DecontaminationController), nameof(DecontaminationController.FinishDecontamination))]
    public class Decontaminating
    {
        /// <summary>
        /// Prefix of <see cref="DecontaminationController.FinishDecontamination"/>.
        /// </summary>
        /// <param name="__instance">The <see cref="DecontaminationController"/> instance.</param>
        /// <returns>Returns a value indicating whether the original method has to be executed or not.</returns>
        public static bool Prefix(DecontaminationController __instance)
        {
            var ev = new DecontaminatingEventArgs();

            Map.OnDecontaminating(ev);

            return ev.IsAllowed;
        }
    }
}