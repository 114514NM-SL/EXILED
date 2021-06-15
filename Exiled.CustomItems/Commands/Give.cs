// -----------------------------------------------------------------------
// <copyright file="Give.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.CustomItems.Commands
{
    using System;
    using System.Linq;

    using CommandSystem;

    using Exiled.API.Features;
    using Exiled.CustomItems.API.Features;
    using Exiled.Permissions.Extensions;

    /// <summary>
    /// The command to give a player an item.
    /// </summary>
    internal sealed class Give : ICommand
    {
        private Give()
        {
        }

        /// <summary>
        /// Gets the <see cref="Give"/> instance.
        /// </summary>
        public static Give Instance { get; } = new Give();

        /// <inheritdoc/>
        public string Command { get; } = "give";

        /// <inheritdoc/>
        public string[] Aliases { get; } = { "g" };

        /// <inheritdoc/>
        public string Description { get; } = "Gives a custom item.";

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("customitems.give"))
            {
                response = "Permission Denied, required: customitems.give";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "give [Custom item name/Custom item ID] [Nickname/PlayerID/UserID/*]";
                return false;
            }

            if (!CustomItem.TryGet(arguments.At(0), out CustomItem item))
            {
                response = $"Custom item {arguments.At(0)} not found!";
                return false;
            }

            string identifier = string.Join(" ", arguments.Skip(1));

            switch (identifier)
            {
                case "*":
                    var eligiblePlayers = Player.List.Where(CheckEligible).ToList();
                    foreach (var ply in eligiblePlayers)
                    {
                        item.Give(ply);
                    }

                    response = $"Custom item {item.Name} given to all players who can receive them ({eligiblePlayers.Count} players)";
                    return true;
                default:
                    if (!(Player.Get(identifier) is Player player))
                    {
                        response = $"Unable to find player: {identifier}.";
                        return false;
                    }

                    if (!CheckEligible(player))
                    {
                        response = "Player cannot receive custom items!";
                        return false;
                    }

                    item.Give(player);
                    response = $"{item.Name} given to {player.Nickname} ({player.UserId})";
                    return true;
            }
        }

        /// <summary>
        /// Checks if the player is eligible to receive custom items.
        /// </summary>
        private bool CheckEligible(Player player)
        {
            return player.IsAlive && !player.IsScp && !player.IsCuffed && player.Inventory.items.Count < 8;
        }
    }
}
