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

namespace AK.DataBinding
{
    /// <summary>
    /// Provides methods for creating instances of the <see cref="BindingResult{T}"/> structure.
    /// This class is <see langword="static"/>.
    /// </summary>
    public static class BindingResult
    {
        /// <summary>
        /// Creates a new instance of the <see cref="BindingResult{T}"/> structure with no value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>An instance no value.</returns>
        public static BindingResult<T> None<T>()
        {
            return default(BindingResult<T>);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingResult{T}"/> structure with the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>A new instance with the given value.</returns>
        public static BindingResult<T> Some<T>(T value)
        {
            return new BindingResult<T>(value);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BindingResult{T}"/> structure from the given nullable value.
        /// If the value is not <see langword="null"/>, then <see cref="Some{T}"/> will be returned; otherwise,
        /// <see cref="None{T}"/> will be returned.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>A new instance with either the value, or no value..</returns>
        public static BindingResult<T> FromNullable<T>(T? value) where T : struct
        {
            return value != null ? Some(value.GetValueOrDefault()) : None<T>();
        }

        /// <summary>
        /// Returns the <see langword="true"/> result for <typeparamref name="TBoolean"/>.
        /// </summary>
        /// <typeparam name="TBoolean">
        /// The type of the value of the result, which must be either boolean or nullable boolean.
        /// </typeparam>
        /// <returns>The <see langword="true"/> result for <typeparamref name="TBoolean"/>.</returns>
        internal static BindingResult<TBoolean> True<TBoolean>()
        {
            return Booleans<TBoolean>.TrueResult;
        }

        /// <summary>
        /// Returns the <see langword="false"/> result for <typeparamref name="TBoolean"/>.
        /// </summary>
        /// <typeparam name="TBoolean">
        /// The type of the value of the result, which must be either boolean or nullable boolean.
        /// </typeparam>
        /// <returns>The <see langword="false"/> result for <typeparamref name="TBoolean"/>.</returns>
        internal static BindingResult<TBoolean> False<TBoolean>()
        {
            return Booleans<TBoolean>.FalseResult;
        }

        private static class Booleans<TBoolean>
        {
            public static readonly BindingResult<TBoolean> TrueResult = Some(Convert(true));

            public static readonly BindingResult<TBoolean> FalseResult = Some(Convert(false));

            private static TBoolean Convert(bool value)
            {
                return (TBoolean)(object)value;
            }
        }
    }
}
