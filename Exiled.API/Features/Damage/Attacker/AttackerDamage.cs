// -----------------------------------------------------------------------
// <copyright file="AttackerDamage.cs" company="Exiled Team">
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
    using Footprinting;
    using PlayerStatsSystem;

    public class AttackerDamage : StandardDamage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttackerDamage"/> class.
        /// </summary>
        /// <param name="damageHandler">The base <see cref="AttackerDamageHandler"/> class.</param>
        internal AttackerDamage(AttackerDamageHandler damageHandler)
            : base(damageHandler)
        {
            Base = damageHandler;
        }

        /// <summary>
        /// Gets the <see cref="AttackerDamageHandler"/> that this class is encapsulating.
        /// </summary>
        public new AttackerDamageHandler Base { get; }

        /// <inheritdoc/>
        public override DamageType Type { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether .
        /// </summary>
        public bool IgnoreFriendlyFireDetector => Base.IgnoreFriendlyFireDetector;

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public bool IsFriendlyFire
        {
            get => Base.IsFriendlyFire;
            set => Base.IsFriendlyFire = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public bool IsSuicide
        {
            get => Base.IsSuicide;
            set => Base.IsSuicide = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public Footprint AttackerFootprint
        {
            get => Base.Attacker;
            set => Base.Attacker = value;
        }

        /// <summary>
        /// .
        /// </summary>
        public Player Attacker
        {
            get => Player.Get(AttackerFootprint);
            set => Base.Attacker = value.Footprint;
        }
    }
}
