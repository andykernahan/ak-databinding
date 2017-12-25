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
using System.Linq.Expressions;
using Xunit;

namespace AK.DataBinding.Private
{
    public class NotSupportedTests
    {
        [Fact]
        public void Cannot_create_binding_from_expression_containing_or_else()
        {
            Expression<Func<Tuple<bool, bool>, bool>> expression = x => x.Item1 || x.Item2;

            Assert.Throws<NotSupportedException>(() => Binding.Create(expression));
        }

        [Fact]
        public void Cannot_create_binding_from_expression_containing_and_else()
        {
            Expression<Func<Tuple<bool, bool>, bool>> expression = x => x.Item1 && x.Item2;

            Assert.Throws<NotSupportedException>(() => Binding.Create(expression));
        }
    }
}
