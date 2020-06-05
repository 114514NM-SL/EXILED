﻿// -----------------------------------------------------------------------
// <copyright file="EnragingScp096EventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers.EventArgs
{
    using System;

    using Exiled.Core.API.Features;

    using PlayableScps;

    /// <summary>
    /// Contains all informations before SCP-096 gets enraged.
    /// </summary>
    public class EnragingScp096EventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnragingScp096EventArgs"/> class.
        /// </summary>
        /// <param name="scp096"><inheritdoc cref="Scp096"/></param>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public EnragingScp096EventArgs(Scp096 scp096, Player player, bool isAllowed = true)
        {
            Scp096 = scp096;
            Player = player;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp096 { get; private set; }

        /// <summary>
        /// Gets the player who's controlling SCP-096.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event can be executed or not.
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}