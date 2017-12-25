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
using System.Linq;

namespace AK.DataBinding.UnitTesting
{
    internal static class BooleanPairProviders
    {
        public sealed class NonNullable : ClassDataProviderBase
        {
            protected override IEnumerable<object[]> GetData()
            {
                return GetBothNullable().Where(x => x.Item1 != null && x.Item2 != null)
                                        .Select(x => A(Tuple.Create(x.Item1.Value, x.Item2.Value)));
            }
        }

        public sealed class Item1Nullable : ClassDataProviderBase
        {
            protected override IEnumerable<object[]> GetData()
            {
                return GetBothNullable().Where(x => x.Item2 != null)
                                        .Select(x => A(Tuple.Create(x.Item1, x.Item2.Value)));
            }
        }

        public sealed class Item2Nullable : ClassDataProviderBase
        {
            protected override IEnumerable<object[]> GetData()
            {
                return GetBothNullable().Where(x => x.Item1 != null)
                                        .Select(x => A(Tuple.Create(x.Item1.Value, x.Item2)));
            }
        }

        public sealed class BothNullable : ClassDataProviderBase
        {
            protected override IEnumerable<object[]> GetData()
            {
                return GetBothNullable().Select(x => A(x));
            }
        }

        private static IEnumerable<Tuple<bool?, bool?>> GetBothNullable()
        {
            var items = new bool?[] {null, false, true};
            return from a in items from b in items select Tuple.Create(a, b);
        }
    }
}
