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
    using Exiled.Events.EventArgs.Cassie;
    using Exiled.Events.Extensions;

    using static Events;

    /// <summary>
    ///     Cassie related events.
    /// </summary>
    public static class Cassie
    {
        /// <summary>
        /// Gets or sets the event invoked before sending a cassie message.
        ///     Invoked before sending a cassie message.
        /// </summary>
        public static Event<SendingCassieMessageEventArgs> SendingCassieMessage { get; set; } = new();

        /// <summary>
        ///     Called before sending a cassie message.
        /// </summary>
        /// <param name="ev">The <see cref="SendingCassieMessageEventArgs" /> instance.</param>
        public static void OnSendingCassieMessage(SendingCassieMessageEventArgs ev)
        {
            SendingCassieMessage.InvokeSafely(ev);
        }
    }
}
