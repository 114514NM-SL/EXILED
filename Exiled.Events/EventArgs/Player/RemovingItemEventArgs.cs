﻿namespace Exiled.Events.EventArgs.Player
{
    using System;
    using System.Linq;

    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs.Interfaces;
    using InventorySystem.Items;

    /// <summary>
    /// Contains all information before item is removed from player's inventory.
    /// </summary>
    public class RemovingItemEventArgs : IPlayerEvent, IDeniableEvent, IItemEvent
    {
        private Item item;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovingItemEventArgs"/> class.
        /// </summary>
        /// <param name="player"><inheritdoc cref="Player"/></param>
        /// <param name="itemBase"><inheritdoc cref="Item"/></param>
        /// <param name="isAllowed"><inheritdoc cref="IsAllowed"/></param>
        public RemovingItemEventArgs(Player player, ItemBase itemBase, bool isAllowed = true)
        {
            Player = player;
            Item = Item.Get(itemBase);
            IsAllowed = isAllowed;
        }

        /// <inheritdoc/>
        public Player Player { get; }

        /// <inheritdoc/>
        public bool IsAllowed { get; set; }

        /// <inheritdoc/>
        public Item Item
        {
            get => item;
            set
            {
                if (!Player.Items.Contains(value))
                    throw new InvalidOperationException("ev.Item cannot be set to item which player don't have in his inventory.");

                item = value;
            }
        }
    }
}