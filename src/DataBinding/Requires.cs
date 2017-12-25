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

namespace AK.DataBinding
{
    internal static class Requires
    {
#if DEBUG
        internal static void NotNull<T>(T obj, string paramName) where T : class
#else
        internal static void NotNull(object obj, string paramName)
#endif
        {
            if (obj == null)
            {
                ThrowArgumentNullException(paramName);
            }
        }

        internal static void NotNullAllowStructs<T>(T obj, string paramName)
        {
            if (obj == null)
            {
                ThrowArgumentNullException(paramName);
            }
        }

        private static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}
