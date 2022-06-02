// -----------------------------------------------------------------------
// <copyright file="EffectGrenadePickup.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Pickups.Projectiles
{
    using InventorySystem.Items.ThrowableProjectiles;

    /// <summary>
    /// A wrapper class for EffectGrenade.
    /// </summary>
    public class EffectGrenadePickup : TimeGrenadePickup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrenadePickup"/> class.
        /// </summary>
        /// <param name="pickupBase">The base <see cref="EffectGrenade"/> class.</param>
        public EffectGrenadePickup(EffectGrenade pickupBase)
            : base(pickupBase)
        {
            Base = pickupBase;
        }

        /// <summary>
        /// Gets the <see cref="EffectGrenade"/> that this class is encapsulating.
        /// </summary>
        public new EffectGrenade Base { get; }
    }
}
