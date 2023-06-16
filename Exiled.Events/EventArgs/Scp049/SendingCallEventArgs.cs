// -----------------------------------------------------------------------
// <copyright file="SendingCallEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Scp049
{
    using API.Features;

    using Interfaces;

    /// <summary>
    /// Contains all information before SCP-049 Call is activated.
    /// </summary>
    public class SendingCallEventArgs : IPlayerEvent, IDeniableEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendingCallEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="duration"><inheritdoc cref="Duration"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public SendingCallEventArgs(Player player, float duration, bool isAllowed = true)
        {
            Player = player;
            Duration = duration;
            IsAllowed = isAllowed;
        }

        /// <inheritdoc />
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the duration of the Call Ability.
        /// </summary>
        public float Duration { get; set; }

        /// <inheritdoc />
        public bool IsAllowed { get; set; }
    }
}