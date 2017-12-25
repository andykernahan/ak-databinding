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

namespace AK.DataBinding.Private
{
    public class InstanceMemberTests
    {
        [Fact]
        public void Can_bind_to_member_of_root()
        {
            var graph = new Person {Age = 42};

            var actual = Binding.Create((Person x) => x.Age);

            AssertEx.Bind(graph, 42, actual);
        }

        [Fact]
        public void Can_bind_to_member_of_leaf()
        {
            var graph = new Person {Name = new Name("Foo")};

            var actual = Binding.Create((Person x) => x.Name.First);

            AssertEx.Bind(graph, "Foo", actual);
        }

        [Fact]
        public void Can_bind_to_non_observable_object()
        {
            var graph = new Person {Name = new Name("FooBar")};

            var actual = Binding.Create((Person x) => x.Name.First.Length);

            AssertEx.Bind(graph, 6, actual);
        }

        [Fact]
        public void Can_bind_to_value_member_of_nullable_object()
        {
            var graph = new Person {Age = 42};

            var actual = Binding.Create((Person x) => x.Age.Value);

            AssertEx.Bind(graph, 42, actual);
        }

        [Fact]
        public void Produces_none_when_object_of_member_in_the_expression_is_null()
        {
            var graph = new Person();
            var none = BindingResult.None<string>();

            var actual = Binding.Create((Person x) => x.Spouse.Name.First);

            AssertEx.Bind(null, none, actual);

            AssertEx.Bind(graph, none, actual);

            graph.Spouse = new Person();
            AssertEx.Bind(graph, none, actual);

            graph.Spouse.Name = new Name("Baz");
            AssertEx.Bind(graph, "Baz", actual);
        }

        [Fact]
        public void Result_becomes_none_when_object_of_member_in_the_expression_becomes_null()
        {
            var name = new Name("a");
            var graph = new Person {Name = name};

            var actual = Binding.Create((Person x) => x.Name.First.Length);

            actual.Bind(graph);
            AssertEx.Some(1, actual);

            graph.Name.First = null;
            AssertEx.None(actual);

            graph.Name.First = "a";
            AssertEx.Some(1, actual);

            graph.Name = null;
            AssertEx.None(actual);

            graph.Name = name;
            AssertEx.Some(1, actual);

            graph.Name = new Name();
            AssertEx.None(actual);

            graph.Name = name;
            AssertEx.Some(1, actual);
        }

        [Fact]
        public void Produces_none_when_object_of_member_of_nullable_object_in_the_expression_is_null()
        {
            var graph = new Person();
            var expected = BindingResult.None<int>();

            var actual = Binding.Create((Person x) => x.Age.Value);

            AssertEx.Bind(graph, expected, actual);
        }

        [Fact]
        public void Result_becomes_none_when_object_of_member_of_nullable_object_in_the_expression_becomes_null()
        {
            var graph = new Person {Age = 42};

            var actual = Binding.Create((Person x) => x.Age.Value);

            actual.Bind(graph);
            AssertEx.Some(42, actual);

            graph.Age = null;
            AssertEx.None(actual);

            graph.Age = 42;
            AssertEx.Some(42, actual);
        }

        private sealed class Person : PropertyChangedBase
        {
            private Name _name;
            private int? _age;
            private Person _spouse;

            public Name Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public int? Age
            {
                get { return _age; }
                set
                {
                    _age = value;
                    OnPropertyChanged();
                }
            }

            public Person Spouse
            {
                get { return _spouse; }
                set
                {
                    _spouse = value;
                    OnPropertyChanged();
                }
            }
        }

        private sealed class Name : PropertyChangedBase
        {
            private string _first;
            private string _last;

            public Name(string first = null, string last = null)
            {
                First = first;
                Last = last;
            }

            public string First
            {
                get { return _first; }
                set
                {
                    _first = value;
                    OnPropertyChanged();
                }
            }

            public string Last
            {
                get { return _last; }
                set
                {
                    _last = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
