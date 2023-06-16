// -----------------------------------------------------------------------
// <copyright file="GainingExperienceEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Scp079
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Interfaces;

    using PlayerRoles.PlayableScps.Scp079;

    /// <summary>
    ///     Contains all information before SCP-079 gains experience.
    /// </summary>
    public class GainingExperienceEventArgs : IPlayerEvent, IDeniableEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GainingExperienceEventArgs" /> class.
        /// </summary>
        /// <param name="player">
        ///     <inheritdoc cref="Player" />
        /// </param>
        /// <param name="gainType">
        ///     <inheritdoc cref="GainType" />
        /// </param>
        /// <param name="amount">
        ///     <inheritdoc cref="Amount" />
        /// </param>
        /// <param name="isAllowed">
        ///     <inheritdoc cref="IsAllowed" />
        /// </param>
        public GainingExperienceEventArgs(Player player, Scp079HudTranslation gainType, int amount, bool isAllowed = true)
        {
            Player = player;
            GainType = gainType;
            Amount = amount;
            IsAllowed = isAllowed;
        }

        /// <summary>
        ///     Gets the experience gain type.
        /// </summary>
        public Scp079HudTranslation GainType { get; }

        /// <summary>
        ///     Gets or sets the amount of experience to be gained.
        /// </summary>
        public int Amount { get; set; }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }

        /// <inheritdoc />
        public Player Player { get; }
    }
}