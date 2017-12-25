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
    internal static class RootBinding
    {
        public static RootBinding<TGraph, TResult> From<TGraph, TResult>(
            Expression<Func<TGraph, TResult>> expression)
        {
            return new Builder<TGraph, TResult>(expression).Build();
        }

        private sealed class Builder<TGraph, TResult>
        {
            private readonly Func<Expression, Binding> _from;
            private readonly LambdaExpression _expression;
            private readonly ParameterBinding<TGraph> _parameter;

            public Builder(Expression<Func<TGraph, TResult>> expression)
            {
                _from = From;
                _expression = expression;
                _parameter = (ParameterBinding<TGraph>)ParameterBinding.From(expression.Parameters[0]);
            }

            public RootBinding<TGraph, TResult> Build()
            {
                var body = (Binding<TResult>)From(_expression.Body);
                return new LambdaBinding<TGraph, TResult>(_parameter, body);
            }

            private Binding From(Expression expression)
            {
                switch (expression)
                {
                    case ParameterExpression _:
                        return _parameter;
                    case MemberExpression m:
                        return MemberBinding.From(m, _from);
                    case ConstantExpression c:
                        return ConstantBinding.From(c);
                    case BinaryExpression b:
                        return BinaryBinding.From(b, _from);
                    case UnaryExpression u:
                        return UnaryBinding.From(u, _from);
                    case null:
                        return null;
                    default:
                        throw Exceptions.NotSupported(expression);
                }
            }
        }
    }
}
