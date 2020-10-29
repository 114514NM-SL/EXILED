// -----------------------------------------------------------------------
// <copyright file="ActivatingEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;

    /// <summary>
    /// Contains all informations before a player activates SCP-914.
    /// </summary>
    public class ChangingMuteStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivatingEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public ChangingMuteStatusEventArgs(Player player, bool status, bool isAllowed = true)
        {
            Player = player;
            Status = status;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's activating SCP-914.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// Gets a value indicating whether the player is being muted or unmuted.
        /// </summary>
        public bool Status { get; }
    }
}
