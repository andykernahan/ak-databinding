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
    /// Defines a prototypical <see cref="IRootBinding{T,TResult}"/> through which new binding instances can be made.
    /// </summary>
    /// <typeparam name="TGraph">The type of the object graph.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <threadsafety static="true" instance="true" />
    public interface IRootBindingPrototype<in TGraph, TResult>
    {
        /// <summary>
        /// Creates a new instance of the root binding.
        /// </summary>
        /// <param name="mode">The binding mode.</param>
        /// <returns>A new <see cref="IRootBinding{T,TResult}"/> instance.</returns>
        IRootBinding<TGraph, TResult> Clone(BindingMode mode = BindingMode.Default);
    }
}
