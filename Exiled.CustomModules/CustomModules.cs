// -----------------------------------------------------------------------
// <copyright file="CustomModules.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.CustomModules
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.DynamicEvents;
    using Exiled.CustomModules.API.Enums;
    using Exiled.CustomModules.API.Features;
    using Exiled.CustomModules.EventHandlers;

    /// <summary>
    /// Handles all custom role API functions.
    /// </summary>
    public class CustomModules : Plugin<Config>
    {
        /// <summary>
        /// Gets a static reference to the plugin's instance.
        /// </summary>
        public static CustomModules Instance { get; private set; }

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.Last;

        /// <summary>
        /// Gets the <see cref="EventHandlers.PlayerHandler"/>.
        /// </summary>
        internal PlayerHandler PlayerHandler { get; private set; }

        /// <summary>
        /// Gets the <see cref="EventHandlers.ServerHandler"/>.
        /// </summary>
        internal ServerHandler ServerHandler { get; private set; }

        /// <summary>
        /// Gets the <see cref="EventHandlers.RegistrationHandler"/>.
        /// </summary>
        internal RegistrationHandler RegistrationHandler { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the specified module is loaded.
        /// </summary>
        /// <param name="module">The module to check.</param>
        /// <returns><see langword="true"/> if the module is loaded; otherwise, <see langword="false"/>.</returns>
        public static bool IsModuleLoaded(UUModuleType module) => Instance.Config.Modules.Contains(module.Name);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            base.OnEnabled();

            CustomModule.LoadAll();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            base.OnDisabled();

            CustomModule.UnloadAll();
        }

        /// <inheritdoc/>
        protected override void SubscribeEvents()
        {
            PlayerHandler = new();
            ServerHandler = new();
            Log.Error($"Config is null? {Config is null}");
            RegistrationHandler = new(Config);

            Exiled.Events.Handlers.Player.ChangingItem += PlayerHandler.OnChangingItem;
            Exiled.Events.Handlers.Server.RoundStarted += ServerHandler.OnRoundStarted;

            DynamicEventManager.CreateFromTypeInstance(RegistrationHandler);
        }

        /// <inheritdoc/>
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingItem -= PlayerHandler.OnChangingItem;
            Exiled.Events.Handlers.Server.RoundStarted -= ServerHandler.OnRoundStarted;

            DynamicEventManager.DestroyFromTypeInstance(RegistrationHandler);

            PlayerHandler = null;
            ServerHandler = null;
            RegistrationHandler = null;
        }
    }
}