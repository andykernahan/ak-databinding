// Copyright 2009 Andy Kernahan
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
//
// Based on https://github.com/andykernahan/ak-f1-timing.

using System;
using System.Collections.Generic;
using System.Linq;
using XAssert = Xunit.Assert;

namespace AK.DataBinding.UnitTesting
{
    public static class EqualityContract
    {
        public static void Assert<T>(IEnumerable<T> equivalent, IEnumerable<T> distinct) where T : IEquatable<T>
        {
            new Contract<T>(equivalent.ToArray(), distinct.ToArray()).Verify();
        }

        private sealed class Contract<T> where T : IEquatable<T>
        {
            private readonly T[] _equivalent;
            private readonly T[] _distinct;

            private static readonly bool IsValueType = typeof(T).IsValueType;

            public Contract(T[] equivalent, T[] distinct)
            {
                _distinct = distinct;
                _equivalent = equivalent;
            }

            public void Verify()
            {
                VerifyGeneral();
                VerifyEquivalent();
                VerifyDistinct();
            }

            private void VerifyGeneral()
            {
                foreach (var x in _equivalent.Concat(_distinct))
                {
                    VerifyGeneral(x);
                }
            }

            private static void VerifyGeneral(T x)
            {
                if (!IsValueType)
                {
                    XAssert.False(x.Equals((T)(object)null));
                }
                XAssert.False(x.Equals(null));
                XAssert.True(x.Equals(x));
                XAssert.True(x.Equals((object)x));
                XAssert.False(x.Equals((object)Foo.Default));
                XAssert.True(x.GetHashCode() == x.GetHashCode());
            }

            private void VerifyEquivalent()
            {
                foreach (var x in _equivalent)
                {
                    foreach (var y in _equivalent)
                    {
                        VerifyEquivalent(x, y);
                    }
                }
            }

            private static void VerifyEquivalent(T x, T y)
            {
                XAssert.True(x.Equals(y));
                XAssert.True(x.Equals((object)y));
                XAssert.True(y.Equals(x));
                XAssert.True(y.Equals((object)x));
                XAssert.True(x.GetHashCode() == y.GetHashCode());
            }

            private void VerifyDistinct()
            {
                for (var xIndex = 0; xIndex < _distinct.Length; ++xIndex)
                {
                    for (var yIndex = 0; yIndex < _distinct.Length; ++yIndex)
                    {
                        if (yIndex == xIndex)
                        {
                            continue;
                        }
                        VerifyDistinct(_distinct[xIndex], _distinct[yIndex]);
                    }
                }
            }

            private static void VerifyDistinct(T x, T y)
            {
                XAssert.False(x.Equals(y));
                XAssert.False(x.Equals((object)y));
                XAssert.False(y.Equals(x));
                XAssert.False(y.Equals((object)x));
                XAssert.False(x.GetHashCode() == y.GetHashCode());
            }
        }

        private sealed class Foo
        {
            public static Foo Default { get; } = new Foo();
        }
    }
}
