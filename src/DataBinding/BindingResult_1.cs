// Copyright 2017 Andy Kernahan
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;

namespace AK.DataBinding
{
    /// <summary>
    /// Contains the result of a binding, which maybe be none.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public struct BindingResult<T> : IEquatable<BindingResult<T>>
    {
        internal readonly T _value;

        /// <summary>
        /// Initialises a new instance of the <see cref="BindingResult{T}"/> structure.
        /// </summary>
        /// <param name="value">The value.</param>
        public BindingResult(T value)
        {
            _value = value;
            HasValue = true;
        }

        /// <summary>
        /// Gets the value (can be <see langword="null"/>).
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// There is no value.
        /// </exception>
        public T Value
        {
            get
            {
                VerifyHasValue();
                return _value;
            }
        }

        /// <summary>
        /// Gets a value indicating if there is a value.
        /// </summary>
        public bool HasValue { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is BindingResult<T> other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(BindingResult<T> other)
        {
            return HasValue == other.HasValue && (!HasValue || ValueComparer.Equals(_value, other._value));
        }

        /// <summary>
        /// Determines whether two <see cref="BindingResult{T}"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> when equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(BindingResult<T> left, BindingResult<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="BindingResult{T}"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> when not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(BindingResult<T> left, BindingResult<T> right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 17;
                result = 37 * result + GetType().GetHashCode();
                result = 37 * result + HasValue.GetHashCode();
                result = 37 * result + (HasValue && _value != null ? ValueComparer.GetHashCode(_value) : 0);
                return result;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return HasValue ? _value?.ToString() ?? "null" : "(None)";
        }

        /// <summary>
        /// Gets the value, or the default of <typeparamref name="T"/> if there is none.
        /// </summary>
        /// <returns>The value, or the default of <typeparamref name="T"/> if there is none.</returns>
        public T GetValueOrDefault()
        {
            return _value;
        }

        /// <summary>
        /// Gets the value, or the given default value if there is none.
        /// </summary>
        /// <param name="defaultValue">The default to return if there is no value.</param>
        /// <returns>The value, or the given default value if there is none.</returns>
        public T GetValueOrDefault(T defaultValue)
        {
            return HasValue ? _value : defaultValue;
        }

        private static EqualityComparer<T> ValueComparer => EqualityComparer<T>.Default;

        private void VerifyHasValue()
        {
            if (!HasValue)
            {
                throw new InvalidOperationException("There is no value.");
            }
        }
    }
}
