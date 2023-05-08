// -----------------------------------------------------------------------
// <copyright file="ColorConverter.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Loader.Features.Configs.CustomConverters
{
    extern alias Yaml;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    using Exiled.API.Features.Pools;

    using Serialization;

    using UnityEngine;

    using Yaml::YamlDotNet.Core.Events;
    using Yaml::YamlDotNet.Serialization;

    using IEmitter = Yaml::YamlDotNet.Core.IEmitter;
    using IParser = Yaml::YamlDotNet.Core.IParser;
    using MappingEnd = Yaml::YamlDotNet.Core.Events.MappingEnd;
    using MappingStart = Yaml::YamlDotNet.Core.Events.MappingStart;

    /// <summary>
    /// Converts <see cref="Color"/> to Yaml configs and vice versa.
    /// </summary>
    public sealed class ColorConverter : IYamlTypeConverter
    {
        /// <inheritdoc cref="IYamlTypeConverter" />
        public bool Accepts(Type type) => type == typeof(Color);

        /// <inheritdoc cref="IYamlTypeConverter" />
        public object ReadYaml(IParser parser, Type type)
        {
            if (!Yaml::YamlDotNet.Core.ParserExtensions.TryConsume<MappingStart>(parser, out _))
                throw new InvalidDataException($"Cannot deserialize object of type {type.FullName}");

            List<object> coordinates = ListPool<object>.Pool.Get(4);
            int i = 0;

            while (!Yaml::YamlDotNet.Core.ParserExtensions.TryConsume<MappingEnd>(parser, out _))
            {
                if (i++ % 2 == 0)
                {
                    parser.MoveNext();
                    continue;
                }

                if (!Yaml::YamlDotNet.Core.ParserExtensions.TryConsume(parser, out Scalar scalar) || !float.TryParse(scalar.Value, NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out float coordinate))
                {
                    ListPool<object>.Pool.Return(coordinates);
                    throw new InvalidDataException("Invalid float value.");
                }

                coordinates.Add(coordinate);
            }

            object color = Activator.CreateInstance(type, coordinates.ToArray());

            ListPool<object>.Pool.Return(coordinates);

            return color;
        }

        /// <inheritdoc cref="IYamlTypeConverter" />
        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            Dictionary<string, float> coordinates = DictionaryPool<string, float>.Pool.Get();

            if (value is Color color)
            {
                coordinates["r"] = color.r;
                coordinates["g"] = color.g;
                coordinates["b"] = color.b;
                coordinates["a"] = color.a;
            }

            emitter.Emit(new MappingStart());

            foreach (KeyValuePair<string, float> coordinate in coordinates)
            {
                emitter.Emit(new Scalar(coordinate.Key));
                emitter.Emit(new Scalar(coordinate.Value.ToString(CultureInfo.GetCultureInfo("en-US"))));
            }

            DictionaryPool<string, float>.Pool.Return(coordinates);
            emitter.Emit(new MappingEnd());
        }
    }
}