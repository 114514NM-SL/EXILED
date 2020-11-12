// -----------------------------------------------------------------------
// <copyright file="AddingTargetEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs
{
    using System;

    using Exiled.API.Features;

    using Scp096 = PlayableScps.Scp096;

    /// <summary>
    /// Contains all informations before adding a target to SCP-096.
    /// </summary>
    public class AddingTargetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddingTargetEventArgs"/> class.
        /// </summary>
        /// <param name="scp096"><see cref="Player"/> who is SCP-096.</param>
        /// <param name="player">The player who is playing as SCP-096.</param>
        /// <param name="target"><see cref="Player"/> who is the target to be added.</param>
        /// <param name="ahpToAdd"><see cref="int"/> amount of temporary health to add to <see cref="Scp096"/>.</param>
        /// <param name="enrageTimeToAdd"><see cref="float"/> amount of time to add to <see cref="Scp096"/>'s enrage timer. Note: This does not affect anything if he doesn't already have any targets before this event is called.</param>
        public AddingTargetEventArgs(Scp096 scp096, Player player, Player target, int ahpToAdd, float enrageTimeToAdd)
        {
            Player = player;
            Scp096 = scp096;
            Target = target;
            AhpToAdd = ahpToAdd;
            EnrageTimeToAdd = enrageTimeToAdd;
        }

        /// <summary>
        /// Gets the player who is playing SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the Scp096 object.
        /// </summary>
        public Scp096 Scp096 { get; }

        /// <summary>
        /// Gets the player being added as a target.
        /// </summary>
        public Player Target { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the target is allowed to be added.
        /// </summary>
        public bool IsAllowed { get; set; } = true;

        /// <summary>
        /// Gets or sets the amount of AHP to add to 096 if <see cref="IsAllowed"/> is true.
        /// </summary>
        public int AhpToAdd { get; set; }

        /// <summary>
        /// Gets or sets how much time is added to <see cref="Scp096"/>'s enrage timer if <see cref="IsAllowed"/> is true.
        /// </summary>
        public float EnrageTimeToAdd { get; set; }
    }
}
