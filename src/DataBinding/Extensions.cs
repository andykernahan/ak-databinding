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
using System.Linq.Expressions;
using AK.DataBinding.Private;

namespace AK.DataBinding
{
    /// <summary>
    /// Provides extensions for this namespace. This class is <see langword="static"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Creates an observable sequence from an expression.
        /// </summary>
        /// <typeparam name="TGraph">The type of the object graph.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The binding expression.</param>
        /// <param name="graph">The object graph to bind to.</param>
        /// <returns>An observable sequence that produces a result when the binding result changes.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> or <paramref name="expression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// A node in <paramref name="expression"/> is not supported.
        /// </exception>
        public static IObservable<BindingResultChange<TResult>> Observe<TGraph, TResult>(
            this TGraph graph,
            Expression<Func<TGraph, TResult>> expression)
        {
            Requires.NotNullAllowStructs(graph, nameof(graph));
            Requires.NotNull(expression, nameof(expression));

            return graph.Observe(Binding.CreatePrototype(expression));
        }

        /// <summary>
        /// Creates an observable sequence from a binding prototype.
        /// </summary>
        /// <typeparam name="TGraph">The type of the object graph.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="prototype">The binding prototype.</param>
        /// <param name="graph">The object graph to bind to.</param>
        /// <returns>An observable sequence that produces a result when the binding result changes.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> or <paramref name="prototype"/> is <see langword="null"/>.
        /// </exception>
        public static IObservable<BindingResultChange<TResult>> Observe<TGraph, TResult>(
            this TGraph graph,
            IRootBindingPrototype<TGraph, TResult> prototype)
        {
            Requires.NotNullAllowStructs(graph, nameof(graph));
            Requires.NotNull(prototype, nameof(prototype));

            return new BindingObservable<TGraph, TResult>(prototype, graph);
        }

        /// <summary>
        /// <para>Creates a function which returns the binding result when applied to the argument.</para>
        /// <para>The returned function is _not_ thread-safe.</para>
        /// </summary>
        /// <typeparam name="TGraph">The type of the object graph.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="prototype">The binding prototype.</param>
        /// <returns>An acessor function which returns the binding result (which is not thread-safe).</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="prototype"/> is <see langword="null"/>.
        /// </exception>
        public static Func<TGraph, BindingResult<TResult>> ToFunc<TGraph, TResult>(
            this IRootBindingPrototype<TGraph, TResult> prototype)
        {
            Requires.NotNull(prototype, nameof(prototype));

            var binding = prototype.Clone(BindingMode.OneTime);
            return graph =>
            {
                try
                {
                    binding.Bind(graph);
                    return binding.Result;
                }
                finally
                {
                    binding.Unbind();
                }
            };
        }
    }
}
