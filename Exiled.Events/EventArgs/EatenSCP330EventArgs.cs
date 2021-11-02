// -----------------------------------------------------------------------
// <copyright file="EatenSCP330EventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;

    using InventorySystem.Items.Usables.Scp330;

    /// <summary>
    /// Contains all informations after a player eaten SCP330.
    /// </summary>
    public class EatenScp330EventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EatenScp330EventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/>.</param>
        /// <param name="candy"><inheritdoc cref="ICandy"/>.</param>
        public EatenScp330EventArgs(Player player, ICandy candy)
        {
            Player = player;
            Candy = candy;
        }

        /// <summary>
        /// Gets the player who's eaten SCP330.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="ICandy"/> that the player has eaten.
        /// </summary>
        public ICandy Candy { get; }
    }
}
