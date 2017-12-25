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
    internal sealed class LambdaBinding<TGraph, TResult> : RootBinding<TGraph, TResult>
    {
        private readonly ParameterBinding<TGraph> _parameter;
        private readonly Binding<TResult> _body;
        private bool _binding;

        public LambdaBinding(ParameterBinding<TGraph> parameter, Binding<TResult> body)
        {
            _parameter = parameter;
            _body = body;
            _body.ResultChanged += (_, __) => MaybeOnResultChanged();
        }

        public override Binding<TResult> Clone(BindingCloneContext context)
        {
            return new LambdaBinding<TGraph, TResult>((ParameterBinding<TGraph>)context.Clone(_parameter), context.Clone(_body));
        }

        protected override void BindCore(TGraph graph)
        {
            try
            {
                _binding = true;
                _parameter.Bind(graph);
            }
            finally
            {
                _binding = false;
            }
            MaybeOnResultChanged();
        }

        protected override void UnbindCore()
        {
            try
            {
                _binding = true;
                _parameter.Unbind();
            }
            finally
            {
                _binding = false;
            }
            MaybeOnResultChanged(None<TResult>());
        }

        private void MaybeOnResultChanged()
        {
            if (!_binding)
            {
                MaybeOnResultChanged(_body.Result);
            }
        }
    }
}
