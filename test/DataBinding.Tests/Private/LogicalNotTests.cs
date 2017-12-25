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
using AK.DataBinding.UnitTesting;
using Xunit;

namespace AK.DataBinding.Private
{
    public class LogicalNotTests
    {
        [Theory]
        [ClassData(typeof(BooleanSingleProviders.NonNullable))]
        public void Produces_correct_result_for_non_nullable_operand(Tuple<bool> graph)
        {
            var expected = BindingResult.Some(!graph.Item1);

            var actual = Binding.Create((Tuple<bool> x) => !x.Item1);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(BooleanSingleProviders.Nullable))]
        public void Produces_correct_result_for_nullable_operand(Tuple<bool?> graph)
        {
            var expected = BindingResult.Some(!graph.Item1);

            var actual = Binding.Create((Tuple<bool?> x) => !x.Item1);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(BooleanSingleProviders.Nullable))]
        public void Produces_correct_result_for_operand_whose_result_maybe_none(Tuple<bool?> single)
        {
            var graph = Tuple.Create(
                single.Item1 != null ? Tuple.Create(single.Item1.Value) : null);

            var expected = BindingResult.FromNullable(!graph.Item1?.Item1);

            var actual = Binding.Create((Tuple<Tuple<bool>> x) => !x.Item1.Item1);

            AssertEx.Bind(graph, expected, actual);
        }
    }
}
