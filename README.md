# ak-databinding

Provides support for observing on expressions.

The expression may contain:
* Deep member access expressions, with null propagation semantics.
* Equality and relational binary expressions.
* Logical unary and binary expressions.
  * Binary expressions with operators `{And, Or, ExclusiveOr}` are evaluated using nullable boolean logic.
  * Binary expressions with operators `{AndAlso, OrElse}` are **not** supported, primarily because they cannot be applied to nullable operands; secondarily, given the current implementation, the operands would **not** be evaluated in a short-circuited manner.

[![Build status](https://ci.appveyor.com/api/projects/status/65lv8x5yemmf8mgh/branch/master?svg=true)](https://ci.appveyor.com/project/andykernahan/ak-databinding/branch/master)

# Sample

The full source is available [here](https://github.com/andykernahan/ak-databinding/blob/master/samples/Sandbox/Program.cs).

```csharp
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
```
