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
using System.Linq;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace AK.DataBinding.Private.Support
{
    internal static class BindingCtors
    {
        private static readonly ConcurrentDictionary<object, Delegate> s_cache =
            new ConcurrentDictionary<object, Delegate>();

        public static Delegate For(Type gtd, Type t1)
        {
            return s_cache.GetOrAdd(Tuple.Create(gtd, t1), key =>
            {
                var args = (Tuple<Type, Type>)key;
                return Create(args.Item1.MakeGenericType(args.Item2));
            });
        }

        public static Delegate For(Type gtd, Type t1, Type t2)
        {
            return s_cache.GetOrAdd(Tuple.Create(gtd, t1, t2), key =>
            {
                var args = (Tuple<Type, Type, Type>)key;
                return Create(args.Item1.MakeGenericType(args.Item2, args.Item3));
            });
        }

        public static Delegate For(Type gtd, Type t1, Type t2, Type t3)
        {
            return s_cache.GetOrAdd(Tuple.Create(gtd, t1, t2, t3), key =>
            {
                var args = (Tuple<Type, Type, Type, Type>)key;
                return Create(args.Item1.MakeGenericType(args.Item2, args.Item3, args.Item4));
            });
        }

        private static Delegate Create(Type type)
        {
            var ctor = type.GetConstructors().Single();
            var ctorParameters = ctor.GetParameters();
            var ctorArguments = new Expression[ctorParameters.Length];
            var λParameters = new ParameterExpression[ctorParameters.Length];
            for (var i = 0; i < ctorParameters.Length; ++i)
            {
                λParameters[i] = Parameter(typeof(object));
                ctorArguments[i] = Convert(λParameters[i], ctorParameters[i].ParameterType);
            }
            var λ = Lambda(
                Convert(New(ctor, ctorArguments), typeof(Binding)),
                λParameters);
            return λ.Compile();
        }
    }
}
