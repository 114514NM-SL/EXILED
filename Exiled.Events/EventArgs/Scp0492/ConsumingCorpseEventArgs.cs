// -----------------------------------------------------------------------
// <copyright file="ConsumingCorpseEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Scp0492
{
    using API.Features;
    using Exiled.API.Features.Roles;
    using Interfaces;

    using PlayerRoles.PlayableScps.Scp049.Zombies;
    using PlayerRoles.Ragdolls;

    /// <summary>
    /// Contains all information before zombie consumes a ragdoll.
    /// </summary>
    public class ConsumingCorpseEventArgs : IScp0492Event, IRagdollEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumingCorpseEventArgs"/> class.
        /// </summary>
        /// <param name="player"> <inheritdoc cref="Player"/></param>
        /// <param name="ragDoll"> <inheritdoc cref="Ragdoll"/> </param>
        /// <param name="isAllowed"> <inheritdoc cref="IsAllowed"/> </param>
        /// <remarks> See <see cref="ZombieConsumeAbility.ConsumedRagdolls"/> for all ragdolls consumed.</remarks>
        public ConsumingCorpseEventArgs(ReferenceHub player, BasicRagdoll ragDoll, bool isAllowed = true)
        {
            Player = Player.Get(player);
            Scp0492 = Player.Role.As<Scp0492Role>();
            Ragdoll = Ragdoll.Get(ragDoll);
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who is controlling SCP-049-2.
        /// </summary>
        public Player Player { get; }

        /// <inheritdoc />
        public Scp0492Role Scp0492 { get; }

        /// <summary>
        /// Gets the ragdoll to be consumed.
        /// </summary>
        public Ragdoll Ragdoll { get; }

        /// <summary>
        /// Gets or sets a value indicating whether 049-2 can consume a corpse.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}