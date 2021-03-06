﻿// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using MicroElements.Functional;

namespace MicroElements.Metadata
{
    /// <summary>
    /// Strong typed property and value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public class PropertyValue<T> : IPropertyValue<T>
    {
        /// <inheritdoc />
        public IProperty<T> Property { get; }

        /// <inheritdoc />
        public T Value { get; }

        /// <inheritdoc />
        public IProperty PropertyUntyped => Property;

        /// <inheritdoc />
        public object ValueUntyped => Value;

        /// <inheritdoc />
        public ValueSource Source { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValue{T}"/> class.
        /// </summary>
        /// <param name="property">Source property.</param>
        /// <param name="value">Value for property.</param>
        /// <param name="source">The source for value.</param>
        public PropertyValue(IProperty<T> property, T value, ValueSource source = null)
        {
            Property = property.AssertArgumentNotNull(nameof(property));
            Value = value;
            Source = source ?? ValueSource.Defined;
        }

        /// <summary>
        /// Implicitly converts to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="propertyValue">PropertyValue.</param>
        public static implicit operator T(PropertyValue<T> propertyValue) => propertyValue.Value;

        /// <inheritdoc />
        public override string ToString() => $"{Property.Name}: {Value.FormatValue()}";
    }

    /// <summary>
    /// Represents value source.
    /// </summary>
    public sealed class ValueSource : ValueObject
    {
        /// <summary>
        /// Value is not set.
        /// </summary>
        public static readonly ValueSource NotDefined = new ValueSource("NotDefined");

        /// <summary>
        /// Value is defined.
        /// </summary>
        public static readonly ValueSource Defined = new ValueSource("Defined");

        /// <summary>
        /// Value was calculated.
        /// </summary>
        public static readonly ValueSource Calculated = new ValueSource("Calculated");

        /// <summary>
        /// Value is default value for property.
        /// </summary>
        public static readonly ValueSource DefaultValue = new ValueSource("DefaultValue");

        /// <summary>
        /// Gets the source name.
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueSource"/> class.
        /// </summary>
        /// <param name="sourceName">The source name.</param>
        public ValueSource(string sourceName)
        {
            SourceName = sourceName.AssertArgumentNotNull(nameof(sourceName));
        }

        /// <inheritdoc />
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return SourceName;
        }
    }

    /// <summary>
    /// PropertyValue extensions.
    /// </summary>
    public static class PropertyValue
    {
        /// <summary>
        /// Creates new instance if <see cref="IPropertyValue"/>.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <param name="value">Value for property.</param>
        /// <param name="valueSource">Value source.</param>
        /// <returns>Created property value.</returns>
        public static IPropertyValue Create(IProperty property, object value, ValueSource valueSource = default)
        {
            property.AssertArgumentNotNull(nameof(property));

            Type propertyType = property.Type;

            if (value != null && value.GetType() != propertyType)
                throw new InvalidOperationException($"Value type {value.GetType()} should be the same as property type {propertyType}.");

            if (value == null && property.Type.IsValueType)
                throw new ArgumentException($"Existing property {property.Name} has type {property.Type} and null value is not allowed");

            Type propertyValueType = typeof(PropertyValue<>).MakeGenericType(propertyType);
            ValueSource source = valueSource ?? ValueSource.Defined;
            IPropertyValue propertyValue = (IPropertyValue)Activator.CreateInstance(propertyValueType, property, value, source);
            return propertyValue;
        }
    }
}
