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

using Xunit;

namespace AK.DataBinding.UnitTesting
{
    internal static class AssertEx
    {
        public static void Bind<TGraph, TResult>(TGraph graph, TResult expected, IRootBinding<TGraph, TResult> actual)
        {
            Bind(graph, BindingResult.Some(expected), actual);
        }

        public static void Bind<TGraph, TResult>(TGraph graph, BindingResult<TResult> expected, IRootBinding<TGraph, TResult> actual)
        {
            None(actual);

            actual.Bind(graph);
            Some(expected, actual);

            actual.Unbind();
            None(actual);
        }

        public static void Some<T>(T expected, IBinding<T> actual)
        {
            Some(BindingResult.Some(expected), actual);
        }

        public static void Some<T>(BindingResult<T> expected, IBinding<T> actual)
        {
            Equal(expected, actual.Result);
        }

        public static void None<T>(IBinding<T> actual)
        {
            None(actual.Result);
        }

        public static void None<T>(BindingResult<T> actual)
        {
            Equal(BindingResult.None<T>(), actual);
        }

        public static void Equal<T>(BindingResult<T> expected, BindingResult<T> actual)
        {
            Assert.Equal(expected, actual);
        }
    }
}
