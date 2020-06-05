// -----------------------------------------------------------------------
// <copyright file="RoundStarted.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events
{
#pragma warning disable SA1313
    using Exiled.Events.Handlers;

    using HarmonyLib;

    /// <summary>
    /// Patches <see cref="CharacterClassManager.CmdStartRound"/>.
    /// Adds the RoundStarted event.
    /// </summary>
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CmdStartRound))]
    public class RoundStarted
    {
        /// <summary>
        /// Prefix of <see cref="CharacterClassManager.CmdStartRound"/>.
        /// </summary>
        public static void Prefix() => Server.OnRoundStarted();
    }
}