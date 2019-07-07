# Supported Linq-like Algorithms
* [Aggregate](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregate?view=netframework-4.7.2)
* [All](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.all?view=netframework-4.7.2)
* [Any](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any?view=netframework-4.7.2)
* [Append](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.append?view=netframework-4.7.2)
* [Cast](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.cast?view=netframework-4.7.2)
* [Concat](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.concat?view=netframework-4.7.2)
* [Contains](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.contains?view=netframework-4.7.2)
* [Count](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count?view=netframework-4.7.2)
* [DefaultIfEmpty](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.defaultifempty?view=netframework-4.7.2)
* [ElementAt](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.elementat?view=netframework-4.7.2)
* [ElementAtOrDefault](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.elementatordefault?view=netframework-4.7.2)
* [Empty](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.empty?view=netframework-4.7.2)
* [First](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.first?view=netframework-4.7.2)
* [FirstOrDefault](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.firstordefault?view=netframework-4.7.2)
* [Last](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.last?view=netframework-4.7.2)
* [LastOrDefault](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.lastordefault?view=netframework-4.7.2)
* [Max](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.max?view=netframework-4.7.2)
* [Min](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.min?view=netframework-4.7.2)
* [OfType](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.oftype?view=netframework-4.7.2)
* [Prepend](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.prepend?view=netframework-4.7.2)
* [Range](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.range?view=netframework-4.7.2)
* [Repeat](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.repeat?view=netframework-4.7.2)
* [Select](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?view=netframework-4.7.2)
* [SelectMany](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany?view=netframework-4.7.2)
* [SequenceEqual](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sequenceequal?view=netframework-4.7.2)
* [Single](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.single?view=netframework-4.7.2)
* [SingleOrDefault](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.singleordefault?view=netframework-4.7.2)
* [Skip](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skip?view=netframework-4.7.2)
* [SkipWhile](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skipwhile?view=netframework-4.7.2)
* [Take](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.take?view=netframework-4.7.2)
* [TakeWhile](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.takewhile?view=netframework-4.7.2)
* [ToList](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tolist?view=netframework-4.7.2)
* [Where](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where?view=netframework-4.7.2)
* [Zip](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.zip?view=netframework-4.7.2)

# Supported Enumerables
* [Array](https://docs.microsoft.com/en-us/dotnet/api/system.array?view=netframework-4.7.2)
* [List](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=netframework-4.7.2)

# Additional Algorithms
* `int? Mismatch<TSecondEnumerator> (EnumerableAdapter<TSecondEnumerator, TSource> second)`
  * Returns the nullable index of the first mismatch between the current enumerable and a second enumerable.
  * If no elements mismatch, then the nullable has no value.
  * Uses the default equality comparer for `TSource`.
* `int? Mismatch<TSecondEnumerator> (EnumerableAdapter<TSecondEnumerator, TSource> second, IEqualityComparer<TSource> comparer)`
  * Returns the nullable index of the first mismatch between the current enumerable and a second enumerable.
  * If no elements mismatch, then the nullable has no value.
  * If `comparer` is null, it uses the default equality comparer for `TSource`.
* `bool None ()`
  * Returns `true` when the enumerable is empty.
* `bool None (Func<TSource, bool> predicate)`
  * Returns `true` if all calls to the predicate (one per element in the enumerable) returns false. 
* `EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource> Replace (TSource what, TSource with)`
  * Returns an enumerable which will substitute `with` for all instances of `what` in the original enumerable.

# Usage
* Unity
  * Add `"com.github.legocylon.relinq": "https://github.com/LegoCylon/Relinq.git",` to the manifest.json file in your project's Packages folder.
  * After Unity imports the package, it will add a lock section with the package name (`com.github.legocylon.relinq`) pinning you to the latest master commit SHA for the package on github. If you want to update to a newer version, delete the lock section for this package or manually update the SHA to the version you want to use.
* C#
  * Add `using Relinq;` to each file you want to be able to use one of the methods above.
  * At each callsite that you prefer to use Relinq rather than Linq, call `.AsEnumerable()` to convert enumerables into `EnumerableAdapter<TEnumerator, TSource>` instances. It's intentional that the name AsEnumerable conflicts with another Linq method of the same name - to help reinforce that mixing calls to both libraries is going to result in heap allocations when Linq is used by mistake.

# Implementation Details
The algorithms use stack memory embedded in value type structs to maintain state rather than heap memory. While this avoids generating garbage, it will require additional stack memory for most algorithms which can get expensive as they are nested.

The value type enumerables use duck typing to facilitate foreach support rather than implementing IEnumerable or IEnumerator. This also helps to avoid accidentally creating garbage when trying to pass the enumerables around.

Although the algorithms themselves don't generate heap allocations, you may still generate them at the callsite if state is captured from the callsite (i.e. via a lambda closure).

Most of the standard Linq algorithms which aren't supported have generally been omitted due to the inability avoid generating garbage. This is particularly true for any set- or dictionary-based algorithms like `Distinct`, `Except`, `Group`, `GroupBy`, `Intersect`, `Join`, `OrderBy`, `OrderByDescending`, `ThenBy`, `ThenByDescending`, and `Union`.

`Average` and `Sum` weren't implemented because they have a ton of overloads, were outside my original use case, and can be implemented in terms of `Aggregate`.