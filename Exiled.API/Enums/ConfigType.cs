// -----------------------------------------------------------------------
// <copyright file="ConfigType.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Enums
{
    using System;

    /// <summary>
    /// The different types of configuration files distribution.
    /// </summary>
    [Obsolete("Use ConfigType inside Exiled.Loader")]
    public enum ConfigType
    {
        /// <summary>
        /// Default distribution, every plugin will share the same config file.
        /// </summary>
        Default,

        /// <summary>
        /// Separated distribution, each plugin will have an individual config file.
        /// </summary>
        Separated,
    }
}