// -----------------------------------------------------------------------
// <copyright file="IConfig.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Interfaces
{
    using System.ComponentModel;

    /// <summary>
    /// Defines the contract for basic config features.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the plugin is enabled or not.
        /// </summary>
        [Description("Indicates whether the plugin is enabled or not")]
        bool IsEnabled { get; set; }
    }
}
