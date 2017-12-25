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

namespace AK.DataBinding
{
    /// <summary>
    /// Contains a change in the result of a binding.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public struct BindingResultChange<T> : IEquatable<BindingResultChange<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingResultChange{T}"/> structure.
        /// </summary>
        /// <param name="oldResult">The old binding result.</param>
        /// <param name="newResult">The new binding result.</param>
        public BindingResultChange(BindingResult<T> oldResult, BindingResult<T> newResult)
        {
            OldResult = oldResult;
            NewResult = newResult;
        }

        /// <summary>
        /// Gets the old binding result.
        /// </summary>
        public BindingResult<T> OldResult { get; }

        /// <summary>
        /// Gets the new binding result.
        /// </summary>
        public BindingResult<T> NewResult { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is BindingResultChange<T> other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(BindingResultChange<T> other)
        {
            return OldResult == other.OldResult && NewResult == other.NewResult;
        }

        /// <summary>
        /// Determines whether two <see cref="BindingResultChange{T}"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> when equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(BindingResultChange<T> left, BindingResultChange<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="BindingResultChange{T}"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> when not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(BindingResultChange<T> left, BindingResultChange<T> right)
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
                result = 37 * result + OldResult.GetHashCode();
                result = 37 * result + NewResult.GetHashCode();
                return result;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"({nameof(OldResult)}={OldResult}, {nameof(NewResult)}={NewResult})";
        }
    }
}
