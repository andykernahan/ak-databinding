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
    public class BitwiseAndTests
    {
        [Theory]
        [ClassData(typeof(Int32PairProviders.NonNullable))]
        public void Produces_correct_result_for_non_nullable_operands(Tuple<int, int> graph)
        {
            var expected = BindingResult.Some(graph.Item1 & graph.Item2);

            var actual = Binding.Create((Tuple<int, int> x) => x.Item1 & x.Item2);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(Int32PairProviders.Item1Nullable))]
        public void Produces_correct_result_for_nullable_left_operand(Tuple<int?, int> graph)
        {
            var expected = BindingResult.Some(graph.Item1 & graph.Item2);

            var actual = Binding.Create((Tuple<int?, int> x) => x.Item1 & x.Item2);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(Int32PairProviders.Item2Nullable))]
        public void Produces_correct_result_for_nullable_right_operand(Tuple<int, int?> graph)
        {
            var expected = BindingResult.Some(graph.Item1 & graph.Item2);

            var actual = Binding.Create((Tuple<int, int?> x) => x.Item1 & x.Item2);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(Int32PairProviders.BothNullable))]
        public void Produces_correct_result_for_nullable_operands(Tuple<int?, int?> graph)
        {
            var expected = BindingResult.Some(graph.Item1 & graph.Item2);

            var actual = Binding.Create((Tuple<int?, int?> x) => x.Item1 & x.Item2);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(Int32PairProviders.Item1Nullable))]
        public void Produces_correct_result_for_left_operand_whose_result_maybe_none(Tuple<int?, int> pair)
        {
            var graph = Tuple.Create(
                pair.Item1 != null ? Tuple.Create(pair.Item1.Value) : null,
                pair.Item2);

            var expected = BindingResult.FromNullable(graph.Item1?.Item1 & graph.Item2);

            var actual = Binding.Create((Tuple<Tuple<int>, int> x) => x.Item1.Item1 & x.Item2);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(Int32PairProviders.Item2Nullable))]
        public void Produces_correct_result_for_right_operand_whose_result_maybe_none(Tuple<int, int?> pair)
        {
            var graph = Tuple.Create(
                pair.Item1,
                pair.Item2 != null ? Tuple.Create(pair.Item2.Value) : null);

            var expected = BindingResult.FromNullable(graph.Item1 & graph.Item2?.Item1);

            var actual = Binding.Create((Tuple<int, Tuple<int>> x) => x.Item1 & x.Item2.Item1);

            AssertEx.Bind(graph, expected, actual);
        }

        [Theory]
        [ClassData(typeof(Int32PairProviders.BothNullable))]
        public void Produces_correct_result_for_operands_whose_results_maybe_none(Tuple<int?, int?> pair)
        {
            var graph = Tuple.Create(
                pair.Item1 != null ? Tuple.Create(pair.Item1.Value) : null,
                pair.Item2 != null ? Tuple.Create(pair.Item2.Value) : null);

            var expected = BindingResult.FromNullable(graph.Item1?.Item1 & graph.Item2?.Item1);

            var actual = Binding.Create((Tuple<Tuple<int>, Tuple<int>> x) => x.Item1.Item1 & x.Item2.Item1);

            AssertEx.Bind(graph, expected, actual);
        }
    }
}
