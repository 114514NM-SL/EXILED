﻿// -----------------------------------------------------------------------
// <copyright file="FallingIntoAbyss.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
    using Exiled.Events.EventArgs;
    using HarmonyLib;
    using UnityEngine;

    /// <summary>
    /// Patches <see cref="CheckpointKiller.OnTriggerEnter"/>.
    /// Adds the <see cref="Handlers.Player.OnFallingIntoAbyss"/> event.
    /// </summary>
    [HarmonyPatch(typeof(CheckpointKiller), nameof(CheckpointKiller.OnTriggerEnter))]
    internal static class FallingIntoAbyss
    {
        private static bool Prefix(Collider other)
        {
            FallingIntoAbyssEventArgs ev = new FallingIntoAbyssEventArgs(API.Features.Player.Get(other.gameObject));
            Handlers.Player.OnFallingIntoAbyss(ev);

            return ev.IsAllowed;
        }
    }
}
