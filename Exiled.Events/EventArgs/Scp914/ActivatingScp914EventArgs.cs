// -----------------------------------------------------------------------
// <copyright file="ActivatingScp914EventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Scp914
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Interfaces;

    /// <summary>
    ///     Contains all information before a player activates SCP-914.
    /// </summary>
    public class ActivatingScp914EventArgs : IPlayerEvent, IDeniableEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivatingScp914EventArgs"/> class.
        /// </summary>
        /// <param name="player">
        ///     <inheritdoc cref="Player"/>
        /// </param>
        /// <param name="isAllowed">
        ///     <inheritdoc cref="IsAllowed"/>
        /// </param>
        public ActivatingScp914EventArgs(Player player, bool isAllowed = true)
        {
            Player = player;
            IsAllowed = isAllowed;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not SCP-914 can be activated.
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        ///     Gets the player who's activating SCP-914.
        /// </summary>
        public Player Player { get; }
    }
}