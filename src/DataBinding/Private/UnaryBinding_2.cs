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
    internal sealed class UnaryBinding<TOperand, TResult> : Binding<TResult>
    {
        private readonly Binding<TOperand> _operand;
        private readonly Func<TOperand, TResult> _op;

        public UnaryBinding(Binding<TOperand> operand, Func<TOperand, TResult> op)
        {
            _operand = operand;
            _op = op;
            _operand.ResultChanged += (_, __) => MaybeOnResultChanged();
            MaybeOnResultChanged();
        }

        public override Binding<TResult> Clone(BindingCloneContext context)
        {
            return new UnaryBinding<TOperand, TResult>(context.Clone(_operand), _op);
        }

        private void MaybeOnResultChanged()
        {
            MaybeOnResultChanged(Evaluate());
        }

        private BindingResult<TResult> Evaluate()
        {
            var operand = _operand.Result;
            return operand.HasValue ? Some(_op(operand._value)) : None<TResult>();
        }
    }
}
