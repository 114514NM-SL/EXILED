// -----------------------------------------------------------------------
// <copyright file="MicroHIDPickup.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Pickups
{
    using Exiled.API.Features.Items;

    using BaseMicroHID = InventorySystem.Items.MicroHID.MicroHIDPickup;

    /// <summary>
    /// A wrapper class for MicroHID.
    /// </summary>
    public class MicroHIDPickup : Pickup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MicroHIDPickup"/> class.
        /// </summary>
        /// <param name="pickupBase">The base <see cref="BaseMicroHID"/> class.</param>
        public MicroHIDPickup(BaseMicroHID pickupBase)
            : base(pickupBase)
        {
            Base = pickupBase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroHIDPickup"/> class.
        /// </summary>
        internal MicroHIDPickup()
           : this((BaseMicroHID)new Pickup(ItemType.MicroHID).Base)
        {
        }

        /// <summary>
        /// Gets the <see cref="BaseMicroHID"/> that this class is encapsulating.
        /// </summary>
        public new BaseMicroHID Base { get; }

        /// <summary>
        /// Gets or sets the MicroHID Energy.
        /// </summary>
        public float Energy
        {
            get => Base.NetworkEnergy;
            set => Base.NetworkEnergy = value;
        }

        /// <summary>
        /// Returns the MicroHIDPickup in a human readable format.
        /// </summary>
        /// <returns>A string containing MicroHIDPickup related data.</returns>
        public override string ToString() => $"{Type} ({Serial}) [{Weight}] *{Scale}* |{Energy}|";
    }
}
