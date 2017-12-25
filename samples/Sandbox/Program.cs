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
using AK.DataBinding;

namespace Sandbox
{
    internal static class Program
    {
        private static void Main()
        {
            var person = new Person();

            // We can create a prototype root binding from the expression:

            IRootBindingPrototype<Person, bool> prototype =
                Binding.CreatePrototype((Person x) => x.Name == null | x.Name.First.Length <= 3);

            // From the prototype, we can clone new instances of the root binding:

            IRootBinding<Person, bool> binding = prototype.Clone();

            // The root binding provides a very simple API:

            binding.ResultChanged += (object _, BindingResultChange<bool> e) => Console.WriteLine(e);
            binding.Bind(person);

            person.Name = new Name("1234"); // True
            person.Name = new Name("1");    // False
            person.Name = null;             // True

            binding.Unbind();

            // We can also create an observable from an expression:

            var d = person.Observe(x => x.Age == null | (x.Age > 33 & x.Age < 35))
                          .Subscribe(x => Console.WriteLine(x));

            person.Age = 34;   // True
            person.Age = 36;   // False
            person.Age = null; // True

            d.Dispose();

            // We can also create an accessor func:

            Func<Person, BindingResult<bool>> accessor = prototype.ToFunc();

            Console.WriteLine(accessor(null));   // False
            Console.WriteLine(accessor(person)); // True
        }
    }
}
