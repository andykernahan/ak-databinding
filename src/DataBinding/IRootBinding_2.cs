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
    /// Defines an object which can be bound to another and which notifies when the bound result changes.
    /// </summary>
    /// <typeparam name="TGraph">The type of the object graph.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IRootBinding<in TGraph, TResult> : IBinding<TResult>
    {
        /// <summary>
        /// Binds to the given object graph.
        /// </summary>
        /// <param name="graph">The object graph to bind to (can be <see langword="null"/>).</param>
        void Bind(TGraph graph);

        /// <summary>
        /// Unbinds from the object graph.
        /// </summary>
        void Unbind();
    }
}
