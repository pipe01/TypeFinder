# TypeFinder

Library for locating types that match a set of rules. Useful for DI containers

Sample usage:

```c#
IEnumerable<Type> matches = FindTypes.InCurrentAssembly
    .Excluding(typeof(ThisType))
    .InNamespace("My.Namespace")
    .ThatInherit<IService>()
    .WithAttribute<PublicAttribute>();
```
