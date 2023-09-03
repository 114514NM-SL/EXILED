// -----------------------------------------------------------------------
// <copyright file="LeftEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Player
{
    using API.Features;

    // TODO: Remove inherited from JoinedEventArgs

    /// <summary>
    /// Contains all information after a <see cref="Player"/> disconnects from the server. <br>This class is inherited from <see cref="JoinedEventArgs" /></br>
    /// </summary>
    public class LeftEventArgs : JoinedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeftEventArgs"/> class.
        /// </summary>
        /// <param name="player">The player who left the server.</param>
        public LeftEventArgs(Player player)
            : base(player)
        {
        }
    }
}