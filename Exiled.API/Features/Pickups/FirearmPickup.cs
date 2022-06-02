// -----------------------------------------------------------------------
// <copyright file="FirearmPickup.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Pickups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using InventorySystem.Items.Firearms;

    using BaseFirearm = InventorySystem.Items.Firearms.FirearmPickup;

    /// <summary>
    /// A wrapper class for Firearm.
    /// </summary>
    public class FirearmPickup : Pickup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirearmPickup"/> class.
        /// </summary>
        /// <param name="itemBase">The base <see cref="BaseFirearm"/> class.</param>
        internal FirearmPickup(BaseFirearm itemBase)
            : base(itemBase)
        {
            Base = itemBase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirearmPickup"/> class.
        /// </summary>
        /// <param name="type">The <see cref="ItemType"/> of the pickup.</param>
        internal FirearmPickup(ItemType type)
            : base(type)
        {
            Base = (BaseFirearm)((Pickup)this).Base;
        }

        /// <summary>
        /// Gets the <see cref="BaseFirearm"/> that this class is encapsulating.
        /// </summary>
        public new BaseFirearm Base { get; }

        /// <summary>
        /// Gets or sets a value indicating whether if the pickup are already distrubuted.
        /// </summary>
        public bool IsDistributed
        {
            get => Base.Distributed;
            set => Base.Distributed = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="FirearmStatus"/>.
        /// </summary>
        public FirearmStatus Status
        {
            get => Base.NetworkStatus;
            set => Base.NetworkStatus = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether how many ammo have this Firearm.
        /// </summary>
        public byte Ammo
        {
            get => Base.NetworkStatus.Ammo;
            set => Base.NetworkStatus = new FirearmStatus(value, Base.NetworkStatus.Flags, Base.NetworkStatus.Attachments);
        }

        /// <summary>
        /// Gets or sets a value indicating whether Flags have this Firearm.
        /// </summary>
        public FirearmStatusFlags Flags
        {
            get => Base.NetworkStatus.Flags;
            set => Base.NetworkStatus = new FirearmStatus(Base.NetworkStatus.Ammo, value, Base.NetworkStatus.Attachments);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Attachement code.
        /// </summary>
        public uint Attachments
        {
            get => Base.NetworkStatus.Attachments;
            set => Base.NetworkStatus = new FirearmStatus(Base.NetworkStatus.Ammo, Base.NetworkStatus.Flags, value);
        }

        /// <summary>
        /// Returns the AmmoPickup in a human readable format.
        /// </summary>
        /// <returns>A string containing AmmoPickup related data.</returns>
        public override string ToString() => $"{Type} ({Serial}) [{Weight}] *{Scale}* |{IsDistributed}| -{Ammo}-";
    }
}
