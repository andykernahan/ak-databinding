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
using AK.DataBinding.Private.Support;

namespace AK.DataBinding.Private
{
    internal static class UnaryBinding
    {
        public static Binding From(UnaryExpression expression, Func<Expression, Binding> from)
        {
            var ctor = (Func<object, object, Binding>)BindingCtors.For(
                GetBindingTypeDefinition(expression),
                expression.Operand.Type,
                expression.Type);
            return ctor(
                from(expression.Operand),
                Operators.For(expression));
        }

        private static Type GetBindingTypeDefinition(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.Not:
                    return typeof(UnaryBinding<,>);
                default:
                    throw Exceptions.NotSupported(expression);
            }
        }
    }
}
