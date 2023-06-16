// -----------------------------------------------------------------------
// <copyright file="ReloadingWeaponEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Player
{
    using API.Features;
    using API.Features.Items;

    using Interfaces;

    /// <summary>
    ///     Contains all information before a player's weapon is reloaded.
    /// </summary>
    public class ReloadingWeaponEventArgs : IPlayerEvent, IFirearmEvent, IDeniableEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ReloadingWeaponEventArgs" /> class.
        /// </summary>
        /// <param name="player">
        ///     <inheritdoc cref="Player" />
        /// </param>
        /// <param name="isAllowed">
        ///     <inheritdoc cref="IsAllowed" />
        /// </param>
        public ReloadingWeaponEventArgs(Player player, bool isAllowed = true)
        {
            Firearm = player.CurrentItem as Firearm;
            Player = player;
            IsAllowed = isAllowed;
        }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }

        /// <inheritdoc />
        public Firearm Firearm { get; }

        /// <inheritdoc />
        public Item Item => Firearm;

        /// <inheritdoc />
        public Player Player { get; }
    }
}