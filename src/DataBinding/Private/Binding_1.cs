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

namespace AK.DataBinding.Private
{
    internal abstract partial class Binding<TResult> : Binding, IBinding<TResult>
    {
        public BindingResult<TResult> Result { get; private set; }

        public virtual event Action<object, BindingResultChange<TResult>> ResultChanged;

        public abstract Binding<TResult> Clone(BindingCloneContext context);

        protected void MaybeOnResultChanged(BindingResult<TResult> newResult)
        {
            if (!ResultComparer.Equals(newResult, Result))
            {
                OnResultChanged(newResult);
            }
        }

        protected void OnResultChanged(BindingResult<TResult> newResult)
        {
            var oldResult = Result;
            Result = newResult;
            ResultChanged?.Invoke(this, BindingResultChange.Create(oldResult, newResult));
        }
    }
}
