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

using AK.DataBinding.UnitTesting;
using Xunit;

namespace AK.DataBinding
{
    public class BindingResultChangeTests
    {
        [Fact]
        public void Ctor_correctly_initializes_members()
        {
            var expectedOldResult = BindingResult.Some(42);
            var expectedNewResult = BindingResult.Some(84);

            var actual = new BindingResultChange<int>(expectedOldResult, expectedNewResult);

            AssertEx.Equal(expectedOldResult, actual.OldResult);
            AssertEx.Equal(expectedNewResult, actual.NewResult);
        }

        [Fact]
        public void Default_ctor_correctly_initializes_members()
        {
            var actual = default(BindingResultChange<string>);

            AssertEx.None(actual.OldResult);
            AssertEx.None(actual.NewResult);
        }

        [Fact]
        public void Implements_equality_contract()
        {
            EqualityContract.Assert(
                equivalent: new[]
                {
                    new BindingResultChange<int>(BindingResult.Some(0), BindingResult.Some(1)),
                    new BindingResultChange<int>(BindingResult.Some(0), BindingResult.Some(1))
                },
                distinct: new[]
                {
                    new BindingResultChange<int>(BindingResult.Some(0), BindingResult.Some(1)),
                    new BindingResultChange<int>(BindingResult.Some(1), BindingResult.Some(1)),
                    new BindingResultChange<int>(BindingResult.Some(0), BindingResult.Some(0)),
                    new BindingResultChange<int>(BindingResult.None<int>(), BindingResult.Some(1)),
                    new BindingResultChange<int>(BindingResult.Some(0), BindingResult.None<int>())
                });
        }
    }
}
