// -----------------------------------------------------------------------
// <copyright file="UniversalDamage.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Damage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using PlayerStatsSystem;

    public class UniversalDamage : StandardDamage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalDamage"/> class.
        /// </summary>
        /// <param name="damageHandler">The base <see cref="UniversalDamageHandler"/> class.</param>
        internal UniversalDamage(UniversalDamageHandler damageHandler)
            : base(damageHandler)
        {
            Base = damageHandler;
            Type = DamageTypeExtensions.TranslationIdConversion[Base.TranslationId];
        }

        /// <summary>
        /// Gets the <see cref="UniversalDamageHandler"/> that this class is encapsulating.
        /// </summary>
        public new UniversalDamageHandler Base { get; }

        /// <inheritdoc/>
        public override DamageType Type { get; internal set; }
    }
}
