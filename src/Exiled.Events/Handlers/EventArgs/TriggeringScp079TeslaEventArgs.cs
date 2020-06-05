﻿// -----------------------------------------------------------------------
// <copyright file="TriggeringScp079TeslaEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers.EventArgs
{
    using System;
    using Exiled.Core.API.Features;

    /// <summary>
    /// Contains all informations before a player triggers a tesla through SCP-079.
    /// </summary>
    public class TriggeringScp079TeslaEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggeringScp079TeslaEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="tesla"><inheritdoc cref="Tesla"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public TriggeringScp079TeslaEventArgs(Player player, TeslaGate tesla, bool isAllowed = true)
        {
            Player = player;
            Tesla = tesla;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the player who's triggering the tesla through SCP-079.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets the tesla game object, that SCP-079 is triggering.
        /// </summary>
        public TeslaGate Tesla { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}