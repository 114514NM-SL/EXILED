// -----------------------------------------------------------------------
// <copyright file="AutoUpdateFiles.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Loader
{
    using System;

    /// <summary>
    /// Class automatically update with Reference use for generating Exiled.
    /// </summary>
    public static class AutoUpdateFiles
    {
        /// <summary>
        /// Gets SCPSL Version use to generate Exiled.
        /// </summary>
        public static readonly Version RequiredSCPSLVersion = new(12, 0, 2, 0);
    }
}