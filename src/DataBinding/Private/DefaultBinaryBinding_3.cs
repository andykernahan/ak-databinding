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
using static AK.DataBinding.BindingResult;

namespace AK.DataBinding.Private
{
    internal sealed class DefaultBinaryBinding<TLeft, TRight, TResult> : BinaryBinding<TLeft, TRight, TResult>
    {
        public DefaultBinaryBinding(Binding<TLeft> left, Binding<TRight> right, Func<TLeft, TRight, TResult> op)
            : base(left, right, op)
        {
        }

        public override Binding<TResult> Clone(BindingCloneContext context)
        {
            return new DefaultBinaryBinding<TLeft, TRight, TResult>(context.Clone(Left), context.Clone(Right), Op);
        }

        protected override BindingResult<TResult> Evaluate()
        {
            var left = Left.Result;
            var right = Right.Result;
            return left.HasValue && right.HasValue ? Some(Op(left._value, right._value)) : None<TResult>();
        }
    }
}
