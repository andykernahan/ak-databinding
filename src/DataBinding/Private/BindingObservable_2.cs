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
using System.Threading;

namespace AK.DataBinding.Private
{
    internal sealed class BindingObservable<TGraph, TResult> : IObservable<BindingResultChange<TResult>>
    {
        private readonly IRootBindingPrototype<TGraph, TResult> _prototype;
        private readonly TGraph _graph;

        public BindingObservable(IRootBindingPrototype<TGraph, TResult> prototype, TGraph graph)
        {
            _prototype = prototype;
            _graph = graph;
        }

        public IDisposable Subscribe(IObserver<BindingResultChange<TResult>> observer)
        {
            Requires.NotNull(observer, nameof(observer));

            return new Subscription(observer, _prototype, _graph);
        }

        private sealed class Subscription : IDisposable
        {
            private IObserver<BindingResultChange<TResult>> _observer;
            private IRootBinding<TGraph, TResult> _binding;

            public Subscription(
                IObserver<BindingResultChange<TResult>> observer,
                IRootBindingPrototype<TGraph, TResult> prototype,
                TGraph graph)
            {
                _observer = observer;
                _binding = prototype.Clone();
                _binding.ResultChanged += OnBindingResultChanged;
                _binding.Bind(graph);
            }

            public void Dispose()
            {
                if (Interlocked.Exchange(ref _observer, null) == null)
                {
                    return;
                }
                _binding.ResultChanged -= OnBindingResultChanged;
                _binding.Unbind();
                _binding = null;
            }

            private void OnBindingResultChanged(object _, BindingResultChange<TResult> e)
            {
                _observer?.OnNext(e);
            }
        }
    }
}
