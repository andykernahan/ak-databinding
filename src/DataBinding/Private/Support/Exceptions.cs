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
using System.Text;

namespace AK.DataBinding.Private.Support
{
    internal static class Exceptions
    {
        public static NotSupportedException NotSupported(Expression expression)
        {
            return NotSupportedCore(
                expression,
                "The expression is not supported.");
        }

        public static NotSupportedException StaticMembersNotSupported(MemberExpression expression)
        {
            return NotSupportedCore(
                expression,
                "Static member expressions are not supported as there is no mechanism for observing on changes to the member.");
        }

        public static NotSupportedException ShortCircuitingNotSupported(BinaryExpression expression)
        {
            return NotSupportedCore(
                expression,
                "The short-circuiting operators are not supported as they cannot be applied to nullable operands.");
        }

        private static NotSupportedException NotSupportedCore(Expression expression, string message)
        {
            var sb = new StringBuilder();
            sb.Append(message).AppendLine();
            sb.Append("Expression: ").Append(expression);
            return new NotSupportedException(sb.ToString());
        }
    }
}
