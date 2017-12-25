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
using AK.DataBinding.Private.Support;
using static AK.DataBinding.BindingResult;

namespace AK.DataBinding.Private
{
    internal sealed class LogicalOrBinding<TLeft, TRight, TResult> : BinaryBinding<TLeft, TRight, TResult>
    {
        static LogicalOrBinding()
        {
            Debug.Assert(TypeUtils.IsBoolean(typeof(TLeft)));
            Debug.Assert(TypeUtils.IsBoolean(typeof(TRight)));
            Debug.Assert(TypeUtils.IsBoolean(typeof(TResult)));
        }

        public LogicalOrBinding(Binding<TLeft> left, Binding<TRight> right, Func<TLeft, TRight, TResult> op)
            : base(left, right, op)
        {
        }

        public override Binding<TResult> Clone(BindingCloneContext context)
        {
            return new LogicalOrBinding<TLeft, TRight, TResult>(context.Clone(Left), context.Clone(Right), Op);
        }

        protected override BindingResult<TResult> Evaluate()
        {
            var left = Left.Result;
            var right = Right.Result;
            if (left.HasValue == right.HasValue)
            {
                return left.HasValue ? Some(Op(left._value, right._value)) : None<TResult>();
            }
            return left == True<TLeft>() || right == True<TRight>() ? True<TResult>() : None<TResult>();
        }
    }
}
