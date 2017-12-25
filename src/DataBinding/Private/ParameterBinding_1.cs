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

using static AK.DataBinding.BindingResult;

namespace AK.DataBinding.Private
{
    internal sealed class ParameterBinding<TResult> : RootBinding<TResult, TResult>
    {
        public override Binding<TResult> Clone(BindingCloneContext context)
        {
            return new ParameterBinding<TResult>();
        }

        protected override void BindCore(TResult graph)
        {
            OnResultChanged(Some(graph));
        }

        protected override void UnbindCore()
        {
            OnResultChanged(None<TResult>());
        }
    }
}
