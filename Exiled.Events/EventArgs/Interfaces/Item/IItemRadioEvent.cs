﻿// -----------------------------------------------------------------------
// <copyright file="IItemRadioEvent.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Interfaces.Item
{
    using Exiled.API.Features.Items;

    /// <summary>
    ///     Event args used for all <see cref="API.Features.Items.Radio" /> related events.
    /// </summary>
    public interface IItemRadioEvent : IExiledEvent
    {
        /// <summary>
        ///     Gets the <see cref="API.Features.Items.Radio" /> triggering the event.
        /// </summary>
        public Radio Radio { get; }
    }
}
