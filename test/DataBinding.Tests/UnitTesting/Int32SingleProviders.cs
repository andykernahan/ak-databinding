﻿// Copyright 2017 Andy Kernahan
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

namespace AK.DataBinding.UnitTesting
{
    internal static class Int32SingleProviders
    {
        public sealed class NonNullable : ClassDataProviderBase
        {
            protected override IEnumerable<object[]> GetData()
            {
                yield return A(Tuple.Create(0));
                yield return A(Tuple.Create(1));
            }
        }

        public sealed class Nullable : ClassDataProviderBase
        {
            protected override IEnumerable<object[]> GetData()
            {
                yield return A(Tuple.Create((int?)null));
                yield return A(Tuple.Create((int?)0));
                yield return A(Tuple.Create((int?)1));
            }
        }
    }
}
