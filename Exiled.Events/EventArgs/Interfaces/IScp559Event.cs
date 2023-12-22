﻿namespace Exiled.Events.EventArgs.Interfaces
{
    /// <summary>
    /// Interface for all events related to <see cref="Scp559Cake"/>.
    /// </summary>
    public interface IScp559Event : IExiledEvent
    {
        /// <summary>
        /// Gets the <see cref="API.Features.Scp559"/>.
        /// </summary>
        public API.Features.Scp559 Scp559 { get; }
    }
}