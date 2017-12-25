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
    /// Provides methods for creating instances of the <see cref="BindingResultChange{T}"/> structure.
    /// This class is <see langword="static"/>.
    /// </summary>
    internal static class BindingResultChange
    {
        /// <summary>
        /// Creates a new instance of the <see cref="BindingResultChange{T}"/> structure.
        /// </summary>
        /// <param name="oldResult">The old binding result.</param>
        /// <param name="newResult">The new binding result.</param>
        /// <returns>A new instance with the given old and new result.</returns>
        public static BindingResultChange<TResult> Create<TResult>(
            BindingResult<TResult> oldResult,
            BindingResult<TResult> newResult)
        {
            return new BindingResultChange<TResult>(oldResult, newResult);
        }
    }
}
