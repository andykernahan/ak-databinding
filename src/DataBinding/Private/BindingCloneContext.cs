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

using System.Collections.Generic;

namespace AK.DataBinding.Private
{
    internal sealed class BindingCloneContext
    {
        private readonly List<Cloned> _clones = new List<Cloned>();

        public BindingCloneContext(BindingMode bindingMode)
        {
            BindingMode = bindingMode;
        }

        public BindingMode BindingMode { get; }

        public Binding<TResult> Clone<TResult>(Binding<TResult> binding)
        {
            var clone = (Binding<TResult>)FindClone(binding);
            if (clone == null)
            {
                clone = binding.Clone(this);
                AddClone(binding, clone);
            }
            return clone;
        }

        private Binding FindClone(Binding original)
        {
            foreach (var item in _clones)
            {
                if (ReferenceEquals(item.Original, original))
                {
                    return item.Clone;
                }
            }
            return null;
        }

        private void AddClone(Binding original, Binding clone)
        {
            _clones.Add(new Cloned(original, clone));
        }

        private struct Cloned
        {
            public Cloned(Binding original, Binding clone)
            {
                Original = original;
                Clone = clone;
            }

            public Binding Original { get; }

            public Binding Clone { get; }
        }
    }
}
