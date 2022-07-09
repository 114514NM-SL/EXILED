// -----------------------------------------------------------------------
// <copyright file="Cassie.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers
{
    using Exiled.Events.EventArgs;
    using Exiled.Events.Features;

    /// <summary>
    /// Cassie related events.
    /// </summary>
    public static class Cassie
    {
        /// <summary>
        /// Invoked before sending a cassie message.
        /// </summary>
        public static readonly Event<SendingCassieMessageEventArgs> SendingCassieMessage = new();

        /// <summary>
        /// Called before sending a cassie message.
        /// </summary>
        /// <param name="ev">The <see cref="SendingCassieMessageEventArgs"/> instance.</param>
        public static void OnSendingCassieMessage(SendingCassieMessageEventArgs ev) => SendingCassieMessage.InvokeSafely(ev);
    }
}
