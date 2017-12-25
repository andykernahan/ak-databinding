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

namespace AK.DataBinding.Private.Support
{
    internal static class TypeUtils
    {
        public static bool IsBoolean(Type type)
        {
            return GetNonNullable(type) == typeof(bool);
        }

        public static Type GetNonNullable(Type type)
        {
            return IsNullable(type) ? type.GetGenericArguments()[0] : type;
        }

        private static bool IsNullable(Type type)
        {
            return type.IsValueType &&
                   type.IsConstructedGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
