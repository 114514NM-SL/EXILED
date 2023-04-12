// -----------------------------------------------------------------------
// <copyright file="ExplosionDamage.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Damage.Attacker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Exiled.API.Enums;
    using PlayerStatsSystem;

    public class ExplosionDamage : AttackerDamage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplosionDamage"/> class.
        /// </summary>
        /// <param name="damageHandler">The base <see cref="ExplosionDamageHandler"/> class.</param>
        public ExplosionDamage(ExplosionDamageHandler damageHandler)
            : base(damageHandler)
        {
            Base = damageHandler;
        }

        /// <summary>
        /// Gets the <see cref="ExplosionDamageHandler"/> that this class is encapsulating.
        /// </summary>
        public new ExplosionDamageHandler Base { get; }

        /// <inheritdoc/>
        public override DamageType Type { get; } = DamageType.Explosion;

    }
}
