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

namespace AK.DataBinding.Private
{
    internal abstract class RootBinding<TGraph, TResult> : Binding<TResult>, IRootBinding<TGraph, TResult>
    {
        public void Bind(TGraph graph)
        {
            BindCore(graph);
        }

        public void Unbind()
        {
            UnbindCore();
        }

        protected abstract void BindCore(TGraph graph);

        protected abstract void UnbindCore();
    }
}
