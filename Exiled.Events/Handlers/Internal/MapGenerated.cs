// -----------------------------------------------------------------------
// <copyright file="MapGenerated.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Handlers.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.API.Structs;

    using global::Scp914;

    using InventorySystem.Items.Firearms.Attachments.Components;

    using MapGeneration;
    using MapGeneration.Distributors;

    using MEC;

    using NorthwoodLib.Pools;

    using UnityEngine;

    using Broadcast = Broadcast;
    using Camera = Exiled.API.Features.Camera;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Handles <see cref="Handlers.Map.Generated"/> event.
    /// </summary>
    internal static class MapGenerated
    {
        /// <summary>
        /// Called once the map is generated.
        /// </summary>
        /// <remarks>
        /// This fixes an issue where
        /// all those extensions that
        /// require calling the central
        /// property of the Map class in
        /// the API were corrupted due to
        /// a missed call, such as before
        /// getting the elevator type.
        /// </remarks>
        public static void OnMapGenerated()
        {
            Map.ClearCache();
            Timing.CallDelayed(0.25f, GenerateCache);
        }

        private static void GenerateCache()
        {
            Warhead.Controller = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();
            Warhead.SitePanel = Object.FindObjectOfType<AlphaWarheadNukesitePanel>();
            Warhead.OutsitePanel = Object.FindObjectOfType<AlphaWarheadOutsitePanel>();
            Server.Host = new Player(PlayerManager.localPlayer);
            Server.Broadcast = PlayerManager.localPlayer.GetComponent<Broadcast>();
            Server.BanPlayer = PlayerManager.localPlayer.GetComponent<BanPlayer>();
            Scp914.Scp914Controller = Object.FindObjectOfType<Scp914Controller>();
            GenerateRooms();
            GenerateWindow();
            GenerateLifts();
            GeneratePocketTeleports();
            GenerateAttachments();
            GenerateLockers();
            Map.AmbientSoundPlayer = PlayerManager.localPlayer.GetComponent<AmbientSoundPlayer>();
            Handlers.Map.OnGenerated();
            Timing.CallDelayed(0.1f, Handlers.Server.OnWaitingForPlayers);
        }

        private static void GenerateRooms()
        {
            // Get bulk of rooms with sorted.
            List<RoomIdentifier> roomIdentifiers = ListPool<RoomIdentifier>.Shared.Rent(Object.FindObjectsOfType<RoomIdentifier>());

            // If no rooms were found, it means a plugin is trying to access this before the map is created.
            if (roomIdentifiers.Any())
                throw new InvalidOperationException("Plugin is trying to access Rooms before they are created.");

            foreach (RoomIdentifier roomIdentifier in roomIdentifiers)
                Room.RoomIdentifierToRoom.Add(roomIdentifier, Room.CreateComponent(roomIdentifier.gameObject));

            ListPool<RoomIdentifier>.Shared.Return(roomIdentifiers);
        }

        private static void GenerateWindow()
        {
            foreach (BreakableWindow breakableWindow in Object.FindObjectsOfType<BreakableWindow>())
                Window.Get(breakableWindow);
        }

        private static void GenerateLifts()
        {
            foreach (global::Lift lift in Object.FindObjectsOfType<global::Lift>())
                Lift.LiftsValue.Add(new Lift(lift));
        }

        private static void GeneratePocketTeleports() => Map.TeleportsValue.AddRange(Object.FindObjectsOfType<PocketDimensionTeleport>());

        private static void GenerateLockers() => Map.LockersValue.AddRange(Object.FindObjectsOfType<Locker>());

        private static void GenerateAttachments()
        {
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (!type.IsWeapon(false))
                    continue;

                Item item = Item.Create(type);
                if (item is not Firearm firearm)
                    continue;

                Firearm.FirearmInstances.Add(firearm);
                uint code = 1;
                List<AttachmentIdentifier> attachmentIdentifiers = new();
                foreach (Attachment att in firearm.Attachments)
                {
                    attachmentIdentifiers.Add(new AttachmentIdentifier(code, att.Name, att.Slot));
                    code *= 2U;
                }

                Firearm.AvailableAttachmentsValue.Add(type, attachmentIdentifiers.ToArray());
            }
        }
    }
}