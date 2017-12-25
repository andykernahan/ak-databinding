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
using System.Collections.Concurrent;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace AK.DataBinding.Private.Support
{
    internal static class Operators
    {
        private static readonly ConcurrentDictionary<object, Delegate> s_cache =
            new ConcurrentDictionary<object, Delegate>();

        public static Delegate For(UnaryExpression e)
        {
            return s_cache.GetOrAdd(Tuple.Create(e.NodeType, e.Operand.Type, e.Type), key =>
            {
                var args = (Tuple<ExpressionType, Type, Type>)key;
                var x = Variable(args.Item2);
                var λ = Lambda(
                    MakeUnary(args.Item1, x, args.Item3),
                    x);
                return λ.Compile();
            });
        }

        public static Delegate For(BinaryExpression e)
        {
            return s_cache.GetOrAdd(Tuple.Create(e.NodeType, e.Left.Type, e.Right.Type), key =>
            {
                var args = (Tuple<ExpressionType, Type, Type>)key;
                var x = Variable(args.Item2);
                var y = Variable(args.Item3);
                var λ = Lambda(
                    MakeBinary(args.Item1, x, y),
                    x, y);
                return λ.Compile();
            });
        }
    }
}
