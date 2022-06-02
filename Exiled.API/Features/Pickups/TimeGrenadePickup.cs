// -----------------------------------------------------------------------
// <copyright file="TimeGrenadePickup.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Pickups
{
    using InventorySystem.Items.ThrowableProjectiles;

    /// <summary>
    /// A wrapper class for TimeGrenade.
    /// </summary>
    public class TimeGrenadePickup : ProjectilePickup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeGrenadePickup"/> class.
        /// </summary>
        /// <param name="itemBase">The base <see cref="TimeGrenade"/> class.</param>
        internal TimeGrenadePickup(TimeGrenade itemBase)
            : base(itemBase)
        {
            Base = itemBase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeGrenadePickup"/> class.
        /// </summary>
        /// <param name="type">The <see cref="ItemType"/> of the pickup.</param>
        internal TimeGrenadePickup(ItemType type)
            : base(type)
        {
            Base = (TimeGrenade)((Pickup)this).Base;
        }

        /// <summary>
        /// Gets the <see cref="TimeGrenade"/> that this class is encapsulating.
        /// </summary>
        public new TimeGrenade Base { get; }

        /// <summary>
        /// Gets or sets a value indicating whether if the grenade have already explode.
        /// </summary>
        public bool IsAlreadyDetonated
        {
            get => Base._alreadyDetonated;
            set => Base._alreadyDetonated = value;
        }

        /// <summary>
        /// Gets or sets the time to explode before it's start.
        /// </summary>
        public float FuseTime
        {
            get => Base._fuseTime;
            set => Base._fuseTime = value;
        }

        /// <summary>
        /// Gets or sets how long it going to takes to explode.
        /// </summary>
        public float TargetTime
        {
            get => Base.TargetTime;
            set => Base.TargetTime = value;
        }
    }
}
