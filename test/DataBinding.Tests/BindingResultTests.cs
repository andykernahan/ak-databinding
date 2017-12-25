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

namespace AK.DataBinding
{
    public class BindingResultTests
    {
        [Fact]
        public void Ctor_correctly_initializes_members()
        {
            var value = "Foo";

            var actual = new BindingResult<string>(value);

            Assert.True(actual.HasValue);
            Assert.Same(value, actual.Value);
        }

        [Fact]
        public void Default_ctor_correctly_initializes_members()
        {
            var actual = default(BindingResult<int>);

            Assert.False(actual.HasValue);
        }

        [Fact]
        public void Value_throws_when_there_is_no_value()
        {
            var sut = default(BindingResult<int>);

            Assert.Throws<InvalidOperationException>(() => sut.Value);
        }

        [Fact]
        public void Get_value_or_default_returns_value_when_there_is_a_value()
        {
            var i = 42;
            var s = "Foo";

            Assert.Equal(i, new BindingResult<int>(i).GetValueOrDefault());
            Assert.Equal(i, new BindingResult<int?>(i).GetValueOrDefault());
            Assert.Equal(s, new BindingResult<string>(s).GetValueOrDefault());
        }

        [Fact]
        public void Get_value_or_default_returns_default_when_there_is_no_value()
        {
            Assert.Equal(0, default(BindingResult<int>).GetValueOrDefault());
            Assert.Null(default(BindingResult<int?>).GetValueOrDefault());
            Assert.Null(default(BindingResult<string>).GetValueOrDefault());
        }

        [Fact]
        public void Get_value_or_default_returns_given_default_when_there_is_no_value()
        {
            var i = 42;
            var s = "Foo";

            Assert.Equal(i, default(BindingResult<int>).GetValueOrDefault(i));
            Assert.Equal(i, default(BindingResult<int?>).GetValueOrDefault(i));
            Assert.Equal(s, default(BindingResult<string>).GetValueOrDefault(s));
        }

        [Fact]
        public void To_string_returns_to_string_of_value_when_there_is_a_value()
        {
            Assert.Equal("42", new BindingResult<int>(42).ToString());
            Assert.Equal("42", new BindingResult<int?>(42).ToString());
            Assert.Equal("Foo", new BindingResult<string>("Foo").ToString());
        }

        [Fact]
        public void To_string_returns_null_when_there_is_a_value_but_its_null()
        {
            var expected = "null";

            Assert.Equal(expected, new BindingResult<int?>(null).ToString());
            Assert.Equal(expected, new BindingResult<string>(null).ToString());
        }

        [Fact]
        public void To_string_returns_none_when_there_is_no_value()
        {
            Assert.Equal("(None)", default(BindingResult<int>).ToString());
        }

        [Fact]
        public void Implements_equality_contract()
        {
            EqualityContract.Assert(
                equivalent: new[]
                {
                    new BindingResult<string>("a"),
                    new BindingResult<string>("a")
                },
                distinct: new[]
                {
                    new BindingResult<string>("a"),
                    new BindingResult<string>("b"),
                    new BindingResult<string>(null),
                    default(BindingResult<string>)
                });

            EqualityContract.Assert(
                equivalent: new[]
                {
                    default(BindingResult<int>),
                    default(BindingResult<int>)
                },
                distinct: new[]
                {
                    default(BindingResult<int>),
                    new BindingResult<int>(0)
                });
        }

        [Fact]
        public void Some_creates_instance_with_given_value()
        {
            var value = "foo";

            var actual = BindingResult.Some(value);

            AssertSome(value, actual);
        }

        [Fact]
        public void Some_value_can_be_null_for_ref_types()
        {
            object value = null;

            var actual = BindingResult.Some(value);

            AssertSome(value, actual);
        }

        [Fact]
        public void Some_value_can_be_null_for_nullable_value_types()
        {
            int? value = null;

            var actual = BindingResult.Some(value);

            AssertSome(value, actual);
        }

        [Fact]
        public void None_creates_instance_with_no_value()
        {
            var actual = BindingResult.None<string>();

            AssertNone(actual);
        }

        [Fact]
        public void From_nullable_creates_instance_with_some_when_value_is_not_null()
        {
            int? value = 42;

            var actual = BindingResult.FromNullable(value);

            AssertSome(value.Value, actual);
        }

        [Fact]
        public void From_nullable_creates_instance_with_none_when_value_is_null()
        {
            int? value = null;

            var actual = BindingResult.FromNullable(value);

            AssertNone(actual);
        }

        [Fact]
        public void True_for_boolean_returns_true()
        {
            var expected = true;

            var actual = BindingResult.True<bool>();

            AssertSome(expected, actual);
        }

        [Fact]
        public void False_for_boolean_returns_false()
        {
            var expected = false;

            var actual = BindingResult.False<bool>();

            AssertSome(expected, actual);
        }

        [Fact]
        public void True_for_nullable_boolean_returns_true()
        {
            bool? expected = true;

            var actual = BindingResult.True<bool?>();

            AssertSome(expected, actual);
        }

        [Fact]
        public void False_for_nullable_boolean_returns_false()
        {
            bool? expected = false;

            var actual = BindingResult.False<bool?>();

            AssertSome(expected, actual);
        }

        [Fact]
        public void True_and_false_for_non_boolean_type_throws()
        {
            // We are not concerned with the exact exception type, only that it throws.
            Assert.ThrowsAny<Exception>(() => BindingResult.True<int>());
            Assert.ThrowsAny<Exception>(() => BindingResult.False<long>());
        }

        private static void AssertSome<T>(T expected, BindingResult<T> actual)
        {
            Assert.True(actual.HasValue);
            Assert.Equal(expected, actual.Value);
        }

        private static void AssertNone<T>(BindingResult<T> actual)
        {
            Assert.False(actual.HasValue);
        }
    }
}
