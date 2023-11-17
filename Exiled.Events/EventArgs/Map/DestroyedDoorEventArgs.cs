// -----------------------------------------------------------------------
// <copyright file="DoorDamagingEventArgs.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using Interactables.Interobjects.DoorUtils;

namespace Exiled.Events.EventArgs.Map
{
    public class DoorDestroyedEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DoorDestroyedEventArgs" /> class.
        /// </summary>
        /// <param name="doorVariant">
        ///     <inheritdoc cref="DoorVariant" />
        /// </param>
        public DoorDestroyedEventArgs(DoorVariant doorVariant)
        {
            Door = API.Features.Doors.Door.Get(doorVariant);
        }

        /// <summary>
        ///     Gets a value indicating the door that was destroyed
        /// </summary>
        public API.Features.Doors.Door Door { get; }
    }
}
