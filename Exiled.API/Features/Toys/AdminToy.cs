// -----------------------------------------------------------------------
// <copyright file="AdminToy.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Toys
{
    using System.Linq;

    using AdminToys;

    using Enums;
    using Exiled.API.Features.Core;
    using Exiled.API.Interfaces;
    using Footprinting;
    using Mirror;

    using UnityEngine;

    /// <summary>
    /// A wrapper class for <see cref="AdminToys.AdminToyBase"/>.
    /// </summary>
    public abstract class AdminToy : GameEntity, IWorldSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminToy"/> class.
        /// </summary>
        /// <param name="toyAdminToyBase">The <see cref="AdminToys.AdminToyBase"/> to be wrapped.</param>
        /// <param name="type">The <see cref="AdminToyType"/> of the object.</param>
        internal AdminToy(AdminToyBase toyAdminToyBase, AdminToyType type)
            : base(toyAdminToyBase.gameObject)
        {
            AdminToyBase = toyAdminToyBase;
            ToyType = type;

            Map.ToysValue.Add(this);
        }

        /// <summary>
        /// Gets the original <see cref="AdminToys.AdminToyBase"/>.
        /// </summary>
        public AdminToyBase AdminToyBase { get; }

        /// <summary>
        /// Gets the <see cref="AdminToyType"/>.
        /// </summary>
        public AdminToyType ToyType { get; }

        /// <summary>
        /// Gets or sets who spawn the Primitive AdminToy.
        /// </summary>
        public Player Player
        {
            get => Player.Get(Footprint);
            set => Footprint = value.Footprint;
        }

        /// <summary>
        /// Gets or sets the Footprint of the player who spawned the AdminToy.
        /// </summary>
        public Footprint Footprint
        {
            get => AdminToyBase.SpawnerFootprint;
            set => AdminToyBase.SpawnerFootprint = value;
        }

        /// <summary>
        /// Gets or sets the scale of the toy.
        /// </summary>
        public Vector3 Scale
        {
            get => Transform.localScale;
            set => Transform.localScale = value;
        }

        /// <summary>
        /// Gets or sets the movement smoothing value of the toy.
        /// <para>
        /// Higher values reflect smoother movements.
        /// <br /> - 60 is an ideal value.
        /// </para>
        /// </summary>
        public byte MovementSmoothing
        {
            get => AdminToyBase.MovementSmoothing;
            set => AdminToyBase.NetworkMovementSmoothing = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsStatic.
        /// </summary>
        public bool IsStatic
        {
            get => AdminToyBase.IsStatic;
            set => AdminToyBase.IsStatic = value;
        }

        /// <summary>
        /// Gets the <see cref="AdminToy"/> belonging to the <see cref="AdminToys.AdminToyBase"/>.
        /// </summary>
        /// <param name="adminToyBase">The <see cref="AdminToys.AdminToyBase"/> instance.</param>
        /// <returns>The corresponding <see cref="AdminToy"/> instance.</returns>
        public static AdminToy Get(AdminToyBase adminToyBase) => Map.Toys.FirstOrDefault(x => x.AdminToyBase == adminToyBase);

        /// <summary>
        /// Spawns the toy into the game. Use <see cref="UnSpawn"/> to remove it.
        /// </summary>
        public void Spawn() => NetworkServer.Spawn(AdminToyBase.gameObject);

        /// <summary>
        /// Removes the toy from the game. Use <see cref="Spawn"/> to bring it back.
        /// </summary>
        public void UnSpawn() => NetworkServer.UnSpawn(AdminToyBase.gameObject);

        /// <summary>
        /// Destroys the toy.
        /// </summary>
        public void Destroy()
        {
            Map.ToysValue.Remove(this);
            NetworkServer.Destroy(AdminToyBase.gameObject);
        }
    }
}