﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Metadata
{
    /// <summary>
    /// Property and its value.
    /// </summary>
    public interface IPropertyValue
    {
        /// <summary>
        /// Gets property.
        /// </summary>
        IProperty PropertyUntyped { get; }

        /// <summary>
        /// Gets property value.
        /// </summary>
        object ValueUntyped { get; }

        /// <summary>
        /// Gets value source.
        /// </summary>
        ValueSource Source { get; }
    }

    /// <summary>
    /// Strong typed property and value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IPropertyValue<T> : IPropertyValue
    {
        /// <summary>
        /// Gets property.
        /// </summary>
        IProperty<T> Property { get; }

        /// <summary>
        /// Gets property value.
        /// </summary>
        T Value { get; }
    }

    /// <summary>
    /// PropertyValue extensions.
    /// </summary>
    public static class PropertyValueExtensions
    {
        /// <summary>
        /// Returns true if property has value. (Not calculated value but set).
        /// </summary>
        /// <param name="propertyValue">PropertyValue instance.</param>
        /// <returns>true if property has value.</returns>
        public static bool HasValue(this IPropertyValue propertyValue)
        {
            return !IsNullOrNotDefined(propertyValue);
        }

        /// <summary>
        /// PropertyValue is null or in <see cref="ValueSource.NotDefined"/> state.
        /// </summary>
        /// <param name="propertyValue">PropertyValue instance.</param>
        /// <returns>true if PropertyValue is null or in <see cref="ValueSource.NotDefined"/> state.</returns>
        public static bool IsNullOrNotDefined(this IPropertyValue propertyValue)
        {
            return propertyValue == null || propertyValue.Source == ValueSource.NotDefined;
        }
    }
}
