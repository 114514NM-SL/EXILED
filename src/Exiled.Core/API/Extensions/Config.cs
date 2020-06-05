﻿// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Core.API.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// A set of extensions for <see cref="YamlConfig"/>.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Extension of <see cref="YamlConfig.GetStringDictionary(string)"/>.
        /// </summary>
        /// <param name="config">The config instance.</param>
        /// <param name="key">The config key.</param>
        /// <param name="defaultValue">The config default value.</param>
        /// <returns>Returns a <see cref="Dictionary{TKey, TValue}"/> from the configs, or the default value if empty.</returns>
        public static Dictionary<string, string> GetStringDictionary(this YamlConfig config, string key, Dictionary<string, string> defaultValue)
        {
            Dictionary<string, string> dictionary = config.GetStringDictionary(key);

            return dictionary?.Count == 0 ? defaultValue : dictionary;
        }

        /// <summary>
        /// Extension of <see cref="YamlConfig.GetStringList(string)"/>.
        /// </summary>
        /// <param name="config">The config instance.</param>
        /// <param name="key">The config key.</param>
        /// <param name="defaultValue">The config default value.</param>
        /// <returns>Returns a <see cref="List{T}"/> from the configs, or the default value if empty.</returns>
        public static List<string> GetStringList(this YamlConfig config, string key, List<string> defaultValue)
        {
            List<string> list = config.GetStringList(key);

            return list?.Count == 0 ? defaultValue : list;
        }
    }
}
