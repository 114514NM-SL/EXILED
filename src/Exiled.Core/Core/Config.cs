﻿// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Core
{
    using System;
    using Exiled.Core.API.Enums;
    using Exiled.Core.API.Interfaces;

    /// <summary>
    /// The configs of the loader.
    /// </summary>
    public sealed class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public string Prefix => "exiled_";

        /// <summary>
        /// Gets or sets a value indicating whether outdated plugins should be loaded or not.
        /// </summary>
        public bool ShouldLoadOutdatedPlugins { get; set; }

        /// <summary>
        /// Gets or sets the environment type.
        /// </summary>
        public EnvironmentType Environment { get; set; }

        /// <inheritdoc/>
        public void Reload()
        {
            ShouldLoadOutdatedPlugins = PluginManager.YamlConfig.GetBool($"{Prefix}should_load_outdated_plugins");
            Environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), PluginManager.YamlConfig.GetString($"{Prefix}environment", "Production"));
        }
    }
}
