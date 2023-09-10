﻿// -----------------------------------------------------------------------
// <copyright file="LessThanAttribute.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Attributes.Validators
{
    using System;

    using Exiled.API.Interfaces;

    /// <summary>
    /// An attribute to easily manage config values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LessThanAttribute : Attribute, IValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LessThanAttribute"/> class.
        /// </summary>
        /// <param name="number">A number the value should be less.</param>
        /// <param name="isIncluded">Whether or not <param name="number"></param> is included.</param>
        public LessThanAttribute(int number, bool isIncluded = false)
        {
            Number = number;
            IsIncluded = isIncluded;
        }

        /// <summary>
        /// Gets the number.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Gets a value indicating whether or not <see cref="Number"/> is included.
        /// </summary>
        public bool IsIncluded { get; }

        /// <inheritdoc/>
        public bool Validate(object value) => value is int number && (IsIncluded ? number <= Number : number < Number);
    }
}