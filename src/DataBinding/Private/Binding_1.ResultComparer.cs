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
using System.Collections.Generic;
using AK.DataBinding.Private.Support;

namespace AK.DataBinding.Private
{
    internal partial class Binding<TResult>
    {
        private static class ResultComparer
        {
            private static readonly Func<TResult, TResult, bool> ValueEquals = GetValueEquals();

            public static bool Equals(BindingResult<TResult> x, BindingResult<TResult> y)
            {
                return x.HasValue == y.HasValue && (!x.HasValue || ValueEquals(x._value, y._value));
            }

            private static Func<TResult, TResult, bool> GetValueEquals()
            {
                // To prevent the propagation of unnecessary changes through a binding tree, a binding first determines
                // whether a proposed new result is distinct from its current result before notifying its subscribers. As
                // such, the binding must be extremely careful _how_ it compares the values of two results; simply
                // relying on the value's equality implementation may have the effect of changes _not_ being propagated.
                //
                // Consider the following example:
                //
                // class Person
                // {
                //     public Person(int id) { Id = id; }
                //     public int Id { get; }
                //     public string Name { get; set; }
                //     public override bool Equals(object obj) => Id == (obj as Person)?.Id;
                //     public override int GetHashCode() => _id;
                // }
                //
                // class ViewModel : PropertyChangedBase
                // {
                //     public Person Selected
                //     {
                //         get { return _person; }
                //         set { _person = value; OnPropertyChanged(); }
                //     }
                // }
                //
                // var binding = Binding.From((ViewModel x) => x.Selected.Name);
                // var view = new ViewModel{ Selected = new Person(42) { Name = "Foo" } };
                //
                // binding.Bind(view);
                // Console.WriteLine(binding.Result); // <------- *
                //
                // view.Selected = new Person(42) { Name = "Bar" };
                // Console.WriteLine(binding.Result); // <------- *
                //
                // In the example above, "Foo" would be printed twice if a binding naively relied on the value's
                // equality implementation (as according to that of Person's, the values would be equal as both have
                // the same Id).
                //
                // So, to ensure that necessary changes are propagated and unnecessary changes are not, we use the
                // following rules to determine how two values should be compared:
                //
                //   - For known types: delegate to the default equality comparer.
                //   - For other reference types: two values are equal when they refer to the same instance.
                //   - For other value types: two values are never equal (except when they are both null).

                var type = TypeUtils.GetNonNullable(typeof(TResult));
                if (IsKnownToImplementEquality(type))
                {
                    return EqualityComparer<TResult>.Default.Equals;
                }
                if (type.IsValueType)
                {
                    return (x, y) => x == null && y == null;
                }
                return (x, y) => ReferenceEquals(x, y);
            }

            private static bool IsKnownToImplementEquality(Type type)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.DBNull:
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.DateTime:
                    case TypeCode.String:
                        return true;
                    case TypeCode.Empty:
                    case TypeCode.Object:
                        // This obviously isn't complete; additional types should be added on an adhoc basis.
                        return type.IsEnum ||
                               type == typeof(TimeSpan) ||
                               type == typeof(DateTimeOffset) ||
                               type == typeof(Guid);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
