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
using System.Diagnostics;
using System.Linq.Expressions;
using AK.DataBinding.Private.Support;

namespace AK.DataBinding.Private
{
    internal static class BinaryBinding
    {
        public static Binding From(BinaryExpression expression, Func<Expression, Binding> from)
        {
            var ctor = GetBindingCtor(expression);
            return ctor(
                from(expression.Left),
                from(expression.Right),
                Operators.For(expression));
        }

        private static Func<object, object, object, Binding> GetBindingCtor(BinaryExpression expression)
        {
            GetBindingTypeDefinition(expression, out var bindingType, out var bindingTypeArity);
            var ctor = bindingTypeArity == 3 ?
                BindingCtors.For(bindingType, expression.Left.Type, expression.Right.Type, expression.Type) :
                BindingCtors.For(bindingType, expression.Left.Type, expression.Right.Type);
            return (Func<object, object, object, Binding>)ctor;
        }

        private static void GetBindingTypeDefinition(BinaryExpression expression, out Type type, out int arity)
        {
            arity = 3;
            switch (expression.NodeType)
            {
                case ExpressionType.And:
                    type = IsLogical(expression) ? typeof(LogicalAndBinding<,,>) : typeof(DefaultBinaryBinding<,,>);
                    break;
                case ExpressionType.AndAlso:
                    throw Exceptions.ShortCircuitingNotSupported(expression);
                case ExpressionType.ArrayIndex:
                    type = typeof(DefaultBinaryBinding<,,>);
                    break;
                case ExpressionType.Equal:
                    type = typeof(EqualBinding<,>);
                    arity = 2;
                    break;
                case ExpressionType.ExclusiveOr:
                    type = typeof(DefaultBinaryBinding<,,>);
                    break;
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    type = typeof(RelationalBinding<,>);
                    arity = 2;
                    break;
                case ExpressionType.Or:
                    type = IsLogical(expression) ? typeof(LogicalOrBinding<,,>) : typeof(DefaultBinaryBinding<,,>);
                    break;
                case ExpressionType.OrElse:
                    throw Exceptions.ShortCircuitingNotSupported(expression);
                case ExpressionType.NotEqual:
                    type = typeof(NotEqualBinding<,>);
                    arity = 2;
                    break;
                default:
                    throw Exceptions.NotSupported(expression);
            }
            Debug.Assert(arity == type.GetGenericArguments().Length);
        }

        private static bool IsLogical(BinaryExpression expression)
        {
            return TypeUtils.IsBoolean(expression.Left.Type) &&
                   TypeUtils.IsBoolean(expression.Right.Type);
        }
    }
}
