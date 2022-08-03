// -----------------------------------------------------------------------
// <copyright file="ThrowingItemEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;
    using Exiled.API.Features.Items;

    using InventorySystem.Items.ThrowableProjectiles;

    /// <summary>
    /// Contains all information before a player throws a grenade.
    /// </summary>
    public class ThrowingItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingItemEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="item"><inheritdoc cref="Item"/></param>
        /// <param name="projectile"<inheritdoc cref="Grenade"/>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public ThrowingItemEventArgs(Player player, ThrowableItem item, ThrownProjectile projectile, bool isAllowed = true)
        {
            Player = player;
            Item = (Throwable)API.Features.Items.Item.Get(item);
            Grenade = Pickup.Get(projectile);
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's throwing the grenade.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the item being thrown.
        /// </summary>
        public Throwable Item { get; }

        /// <summary>
        /// Gets the pickup thats will thrown.
        /// </summary>
        public Pickup Grenade { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the grenade can be thrown.
        /// </summary>
        public bool IsAllowed { get; set; } = true;
    }
}
