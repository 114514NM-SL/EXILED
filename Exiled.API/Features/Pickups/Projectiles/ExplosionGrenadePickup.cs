// -----------------------------------------------------------------------
// <copyright file="ExplosionGrenadePickup.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Pickups.Projectiles
{
    using Exiled.API.Enums;

    using InventorySystem.Items.ThrowableProjectiles;

    using UnityEngine;

    /// <summary>
    /// A wrapper class for ExplosionGrenade.
    /// </summary>
    public class ExplosionGrenadePickup : EffectGrenadePickup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionGrenadePickup"/> class.
        /// </summary>
        /// <param name="pickupBase">The base <see cref="ExplosionGrenade"/> class.</param>
        public ExplosionGrenadePickup(ExplosionGrenade pickupBase)
            : base(pickupBase)
        {
            Base = pickupBase;
        }

        /// <summary>
        /// Gets the <see cref="ExplosionGrenade"/> that this class is encapsulating.
        /// </summary>
        public new ExplosionGrenade Base { get; }

        /// <summary>
        /// Gets or sets the maximum raduis of the ExplosionGrenade can affected.
        /// </summary>
        public float MaxRaduis
        {
            get => Base._maxRadius;
            set => Base._maxRadius = value;
        }

        /// <summary>
        /// Gets or sets the minimum duration of player can take the effect.
        /// </summary>
        public float MinimalDurationEffect
        {
            get => Base._minimalDuration;
            set => Base._minimalDuration = value;
        }

        /// <summary>
        /// Gets or sets the maximum duration of the <see cref="EffectType.Burned"/> effect.
        /// </summary>
        public float BurnedDuration
        {
            get => Base._burnedDuration;
            set => Base._burnedDuration = value;
        }

        /// <summary>
        /// Gets or sets the maximum duration of the <see cref="EffectType.Deafened"/> effect.
        /// </summary>
        public float DeafenedDuration
        {
            get => Base._deafenedDuration;
            set => Base._deafenedDuration = value;
        }

        /// <summary>
        /// Gets or sets the maximum duration of the <see cref="EffectType.Concussed"/> effect.
        /// </summary>
        public float ConcussedDuration
        {
            get => Base._concussedDuration;
            set => Base._concussedDuration = value;
        }

        /// <summary>
        /// Gets or sets the damage of the <see cref="Team.SCP"/> going to get.
        /// </summary>
        public float ScpDamageMultiplier
        {
            get => Base._scpDamageMultiplier;
            set => Base._scpDamageMultiplier = value;
        }

        /// <summary>
        /// Instant explosion of the grenade.
        /// </summary>
        public void Explode() => ExplosionGrenade.Explode(Base.PreviousOwner, Base.transform.position, Base);

        /// <summary>
        /// Instant explosion of the grenade.
        /// </summary>
        public void Explode(Player player) => ExplosionGrenade.Explode(player.Footprint, Base.transform.position, Base);

        /// <summary>
        /// Instant explosion of the grenade.
        /// </summary>
        public void Explode(Player player, Vector3 position) => ExplosionGrenade.Explode(player.Footprint, position, Base);

        /// <summary>
        /// Returns the ExplosionGrenadePickup in a human readable format.
        /// </summary>
        /// <returns>A string containing ExplosionGrenadePickup-related data.</returns>
        public override string ToString() => $"{Type} ({Serial}) [{Weight}] *{Scale}* |{Position}| -{Locked}- ={InUse}=";
    }
}
