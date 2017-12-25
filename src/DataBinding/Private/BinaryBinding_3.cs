﻿// Copyright 2017 Andy Kernahan
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

namespace AK.DataBinding.Private
{
    internal abstract class BinaryBinding<TLeft, TRight, TResult> : Binding<TResult>
    {
        protected BinaryBinding(Binding<TLeft> left, Binding<TRight> right, Func<TLeft, TRight, TResult> op)
        {
            Left = left;
            Right = right;
            Op = op;
            Left.ResultChanged += (_, __) => MaybeOnResultChanged();
            Right.ResultChanged += (_, __) => MaybeOnResultChanged();
            MaybeOnResultChanged();
        }

        protected Binding<TLeft> Left { get; }

        protected Binding<TRight> Right { get; }

        protected Func<TLeft, TRight, TResult> Op { get; }

        protected abstract BindingResult<TResult> Evaluate();

        private void MaybeOnResultChanged()
        {
            MaybeOnResultChanged(Evaluate());
        }
    }
}
