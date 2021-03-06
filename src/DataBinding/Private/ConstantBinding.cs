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
using System.Linq.Expressions;
using AK.DataBinding.Private.Support;

namespace AK.DataBinding.Private
{
    internal static class ConstantBinding
    {
        public static Binding From(ConstantExpression expression)
        {
            var ctor = (Func<object, Binding>)BindingCtors.For(
                typeof(ConstantBinding<>),
                expression.Type);
            return ctor(expression.Value);
        }
    }
}
