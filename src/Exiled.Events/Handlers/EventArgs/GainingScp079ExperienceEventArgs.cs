﻿// -----------------------------------------------------------------------
// <copyright file="GainingScp079ExperienceEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers.EventArgs
{
    using System;

    using Exiled.Core.API.Features;

    /// <summary>
    /// Contains all informations before SCP-079 gains experience.
    /// </summary>
    public class GainingScp079ExperienceEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GainingScp079ExperienceEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="gainType"><inheritdoc cref="GainType"/></param>
        /// <param name="amount"><inheritdoc cref="Amount"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public GainingScp079ExperienceEventArgs(Player player, ExpGainType gainType, float amount, bool isAllowed = true)
        {
            Player = player;
            GainType = gainType;
            Amount = amount;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's controlling SCP-079.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets the experience gain type.
        /// </summary>
        public ExpGainType GainType { get; private set; }

        /// <summary>
        /// Gets or sets the amount of experience to be gained.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}