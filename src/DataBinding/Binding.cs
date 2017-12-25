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
    /// Provides methods for creating <see cref="IBinding{TResult}"/> objects that are equivalent to
    /// <see cref="Expression"/> objects. This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class Binding
    {
        /// <summary>
        /// Creates a <see cref="IRootBinding{TGraph,TResult}"/> from an expression.
        /// </summary>
        /// <typeparam name="TGraph">The type of the object graph.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The source expression.</param>
        /// <returns>An equivalent <see cref="IRootBinding{T,TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// A node in <paramref name="expression"/> is not supported.
        /// </exception>
        public static IRootBinding<TGraph, TResult> Create<TGraph, TResult>(
            Expression<Func<TGraph, TResult>> expression)
        {
            Requires.NotNull(expression, "expression");

            return RootBinding.From(expression);
        }

        /// <summary>
        /// Creates a <see cref="IRootBindingPrototype{TGraph,TResult}"/> from an expression.
        /// </summary>
        /// <typeparam name="TGraph">The type of the object graph.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The source expression.</param>
        /// <returns>An equivalent <see cref="IRootBindingPrototype{T,TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// A node in <paramref name="expression"/> is not supported.
        /// </exception>
        public static IRootBindingPrototype<TGraph, TResult> CreatePrototype<TGraph, TResult>(
            Expression<Func<TGraph, TResult>> expression)
        {
            Requires.NotNull(expression, "expression");

            var binding = RootBinding.From(expression);
            return new RootBindingPrototype<TGraph, TResult>(binding);            
        }

        internal Binding()
        {
        }
    }
}
