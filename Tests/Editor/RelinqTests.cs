//======================================================================================================================
//  Copyright 2019 Andy Bond (LegoCylon)
//  
//  Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
//  the License. You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
//  an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and limitations under the License.
//======================================================================================================================

using System;
using System.Collections.Generic;

using NUnit.Framework;

using Relinq;

using UnityEngine.TestTools.Constraints;

using Is = NUnit.Framework.Is;

namespace Tests.EditMode {

    public static class RelinqTests {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        private static void AssertAreEqual<T> (T expected, T actual) {
            if (EqualityComparer<T>.Default.Equals(x:expected, y:actual)) {
                return;
            }
            
            throw new ArgumentException(message:$"{expected} vs {actual}");
        }

        //--------------------------------------------------------------------------------------------------------------
        // ReSharper disable once InconsistentNaming
        private static void TestNoGC (TestDelegate code) {
            code();
            Assert.That(code:code, constraint:Is.Not.AllocatingGCMemory());
        }
        
        //--------------------------------------------------------------------------------------------------------------
        // ReSharper disable once InconsistentNaming
        private static void TestNoGC<TResult> (Func<TResult> code, TResult expected) {
            var result = code();
            Assert.That(code:delegate { code(); }, constraint:Is.Not.AllocatingGCMemory());
            
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ValidateIsFalse (Func<bool> code) => TestNoGC(code:code, expected:false);
        
        //--------------------------------------------------------------------------------------------------------------
        private static void ValidateIsTrue (Func<bool> code) => TestNoGC(code:code, expected:true);

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Add () {
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            var added = new List<int> { diff };
            
            AssertAreEqual(expected:diff.Count(), actual:added.Count);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Aggregate () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            
            TestNoGC(
                code:() => empty.Aggregate(
                    seed:0, 
                    func:(accumulate, source) => throw new InvalidOperationException(), 
                    resultSelector:(accumulate) => accumulate
                ),
                expected:0
            );
            TestNoGC(
                code:() => empty.Aggregate(
                    seed:10, 
                    func:(accumulate, source) => throw new InvalidOperationException(), 
                    resultSelector:(accumulate) => accumulate
                ),
                expected:10
            );
            TestNoGC(
                code:() => empty.Aggregate(
                    seed:10, 
                    func:(accumulate, source) => throw new InvalidOperationException(), 
                    resultSelector:(accumulate) => accumulate - 10
                ),
                expected:0
            );
            TestNoGC(
                code:() => same.Aggregate(
                    seed:0, 
                    func:(accumulate, source) => accumulate + source, 
                    resultSelector:(accumulate) => accumulate
                ),
                expected:0
            );
            TestNoGC(
                code:() => same.Aggregate(
                    seed:10, 
                    func:(accumulate, source) => accumulate + source, 
                    resultSelector:(accumulate) => accumulate
                ),
                expected:10
            );
            TestNoGC(
                code:() => same.Aggregate(
                    seed:10, 
                    func:(accumulate, source) => accumulate + source, 
                    resultSelector:(accumulate) => accumulate - 10
                ),
                expected:0
            );
            TestNoGC(
                code:() => diff.Aggregate(
                    seed:0, 
                    func:(accumulate, source) => accumulate + source, 
                    resultSelector:(accumulate) => accumulate
                ),
                expected:3
            );
            TestNoGC(
                code:() => diff.Aggregate(
                    seed:10, 
                    func:(accumulate, source) => accumulate + source, 
                    resultSelector:(accumulate) => accumulate
                ),
                expected:13
            );
            TestNoGC(
                code:() => diff.Aggregate(
                    seed:10, 
                    func:(accumulate, source) => accumulate + source, 
                    resultSelector:(accumulate) => accumulate - 10
                ),
                expected:3
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void All () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            
            ValidateIsTrue(code:() => empty.All(predicate:(value) => throw new InvalidOperationException()));
            ValidateIsTrue(code:() => same.All(predicate:(value) => value >= 0));
            ValidateIsTrue(code:() => diff.All(predicate:(value) => value >= 0));
            ValidateIsFalse(code:() => same.All(predicate:(value) => value < 0));
            ValidateIsFalse(code:() => diff.All(predicate:(value) => value < 0));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Any () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            
            ValidateIsTrue(code:() => same.Any());
            ValidateIsTrue(code:() => diff.Any());
            ValidateIsTrue(code:() => same.Any(predicate:(value) => value == 0));
            ValidateIsTrue(code:() => diff.Any(predicate:(value) => value == 0));
            ValidateIsFalse(code:() => empty.Any());
            ValidateIsFalse(code:() => empty.Any(predicate:(value) => throw new InvalidOperationException()));
            ValidateIsFalse(code:() => same.Any(predicate:(value) => value < 0));
            ValidateIsFalse(code:() => diff.Any(predicate:(value) => value < 0));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Append () {
            var empty = new List<int>();
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Append(source:empty, element:0));
            TestNoGC(code:() => Append(source:diff, element:3));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Append<TSource> (List<TSource> source, TSource element) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable.Append(element:element)) {
                var value = visited < source.Count ? source[index:visited] : element;
                AssertAreEqual(expected:value, actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count + 1, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Array () {
            var empty = System.Array.Empty<int>();
            var diff = new[] { 0, 1, 2, };
            
            TestNoGC(code:() => Array(source:empty));
            TestNoGC(code:() => Array(source:diff));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Array<TSource> (TSource[] source) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable) {
                AssertAreEqual(expected:source[visited], actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Length, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Cast () {
            var diff = new List<string>(collection:new[] { "0", "1", "2" });

            //  Cast boxes if either the source or destination types are objects, so the only way to validate that the
            //  cast doesn't generate garbage is to cast to the same type.
            TestNoGC(code:() => Cast(source:diff, caster:(value) => value));
            
            //  Actually test conversion.
            //
            Cast(source:diff, caster:int.Parse);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Cast<TSource, TCast> (List<TSource> source, Func<TSource, TCast> caster) 
            where TCast : IConvertible
        {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable.Cast<TCast>()) {
                var element = caster(arg:source[index:visited % source.Count]);
                AssertAreEqual(expected:element, actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Concat () {
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Concat(source:diff));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Concat<TSource> (List<TSource> source) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable.Concat(second:enumerable)) {
                var element = source[index:visited % source.Count];
                AssertAreEqual(expected:element, actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count * 2, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Contains () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();

            ValidateIsFalse(code:() => empty.Contains(value:0));
            ValidateIsTrue(code:() => same.Contains(value:0));
            ValidateIsTrue(code:() => diff.Contains(value:0));
            ValidateIsFalse(code:() => same.Contains(value:1));
            ValidateIsFalse(code:() => diff.Contains(value:3));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Count () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();

            TestNoGC(code:() => empty.Count(predicate:(value) => throw new InvalidOperationException()), expected:0);
            TestNoGC(code:() => same.Count(predicate:(value) => value == 0), expected:3);
            TestNoGC(code:() => diff.Count(predicate:(value) => value == 0), expected:1);
            TestNoGC(code:() => same.Count(predicate:(value) => value > 0), expected:0);
            TestNoGC(code:() => diff.Count(predicate:(value) => value < 3), expected:3);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void DefaultIfEmpty () {
            var empty = new List<int>();
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => DefaultIfEmpty(source:empty));
            TestNoGC(code:() => DefaultIfEmpty(source:diff));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void DefaultIfEmpty<TSource> (List<TSource> source) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable.DefaultIfEmpty()) {
                var value = source.Count > 0 ? source[index:visited] : default;
                AssertAreEqual(expected:value, actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count > 0 ? source.Count : 1, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void ElementAt () {
            var empty = new List<int>();
            var diff = new List<int>(collection:new[] { 0, 1, 2 });

            Assert.Throws<ArgumentOutOfRangeException>(code:() => ElementAt(source:empty, index:0, expected:0));
            TestNoGC(code:() => ElementAt(source:diff, index:0, expected:0));
            TestNoGC(code:() => ElementAt(source:diff, index:1, expected:1));
            TestNoGC(code:() => ElementAt(source:diff, index:2, expected:2));
            Assert.Throws<ArgumentOutOfRangeException>(code:() => ElementAt(source:diff, index:3, expected:0));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAt<TSource> (List<TSource> source, int index, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.ElementAt(index:index);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void ElementAtOrDefault () {
            var empty = new List<int>();
            var diff = new List<int>(collection:new[] { 0, 1, 2 });

            TestNoGC(code:() => ElementAtOrDefault(source:empty, index:0, expected:default));
            TestNoGC(code:() => ElementAtOrDefault(source:diff, index:0, expected:0));
            TestNoGC(code:() => ElementAtOrDefault(source:diff, index:1, expected:1));
            TestNoGC(code:() => ElementAtOrDefault(source:diff, index:2, expected:2));
            TestNoGC(code:() => ElementAtOrDefault(source:diff, index:3, expected:default));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAtOrDefault<TSource> (List<TSource> source, int index, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.ElementAtOrDefault(index:index);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Empty () {
            TestNoGC(code:Empty<int>);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Empty<TSource> () {
            foreach (var _ in Enumerable.Empty<TSource>()) {
                throw new InvalidOperationException();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void First () {
            var empty = new List<int>();
            var zero = new List<int>(collection:new[] { 0, 1, 2 });
            var one = new List<int>(collection:new[] { 1, 2, 3 });
            var two = new List<int>(collection:new[] { 2, 3, 4 });

            Assert.Throws<InvalidOperationException>(code:() => First(source:empty, expected:0));
            Assert.Throws<InvalidOperationException>(
                code:() => First(
                    source:empty, 
                    expected:0, 
                    //  Wrap this invalid exception with an aggregate because it's not the expected throw
                    //
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            TestNoGC(code:() => First(source:zero, expected:0));
            TestNoGC(code:() => First(source:zero, expected:0, predicate:(value) => value == 0));
            Assert.Throws<InvalidOperationException>(
                code:() => First(source:zero, expected:0, predicate:(value) => false)
            );
            TestNoGC(code:() => First(source:one, expected:1));
            TestNoGC(code:() => First(source:one, expected:1, predicate:(value) => value > 0));
            Assert.Throws<InvalidOperationException>(
                code:() => First(source:one, expected:1, predicate:(value) => false)
            );
            TestNoGC(code:() => First(source:two, expected:2));
            TestNoGC(code:() => First(source:two, expected:2, predicate:(value) => value < 5));
            Assert.Throws<InvalidOperationException>(
                code:() => First(source:two, expected:2, predicate:(value) => false)
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void First<TSource> (List<TSource> source, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.First();
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void First<TSource> (List<TSource> source, TSource expected, Func<TSource, bool> predicate) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.First(predicate:predicate);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void FirstOrDefault () {
            var empty = new List<int>();
            var zero = new List<int>(collection:new[] { 0, 1, 2 });
            var one = new List<int>(collection:new[] { 1, 2, 3 });
            var two = new List<int>(collection:new[] { 2, 3, 4 });

            TestNoGC(code:() => FirstOrDefault(source:empty, expected:default));
            TestNoGC(
                code:() => FirstOrDefault(
                    source:empty, 
                    expected:default, 
                    predicate:(value) => throw new InvalidProgramException()
                )
            );
            TestNoGC(code:() => FirstOrDefault(source:empty, expected:default));
            TestNoGC(code:() => FirstOrDefault(source:zero, expected:0));
            TestNoGC(code:() => FirstOrDefault(source:zero, expected:0, predicate:(value) => value == 0));
            TestNoGC(code:() => FirstOrDefault(source:zero, expected:default, predicate:(value) => false));
            TestNoGC(code:() => FirstOrDefault(source:one, expected:1));
            TestNoGC(code:() => FirstOrDefault(source:one, expected:1, predicate:(value) => value > 0));
            TestNoGC(code:() => FirstOrDefault(source:one, expected:default, predicate:(value) => false));
            TestNoGC(code:() => FirstOrDefault(source:two, expected:2));
            TestNoGC(code:() => FirstOrDefault(source:two, expected:2, predicate:(value) => value < 5));
            TestNoGC(code:() => FirstOrDefault(source:two, expected:default, predicate:(value) => false));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void FirstOrDefault<TSource> (List<TSource> source, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.FirstOrDefault();
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void FirstOrDefault<TSource> (
            List<TSource> source, 
            TSource expected,
            Func<TSource, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.FirstOrDefault(predicate:predicate);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Last () {
            var empty = new List<int>();
            var zero = new List<int>(collection:new[] { 2, 1, 0 });
            var one = new List<int>(collection:new[] { 3, 2, 1 });
            var two = new List<int>(collection:new[] { 4, 3, 2 });

            Assert.Throws<InvalidOperationException>(code:() => Last(source:empty, expected:0));
            Assert.Throws<InvalidOperationException>(
                code:() => Last(
                    source:empty, 
                    expected:0, 
                    //  Wrap this invalid exception with an aggregate because it's not the expected throw
                    //
                    predicate:(value) => throw new AggregateException(new InvalidProgramException())
                )
            );
            TestNoGC(code:() => Last(source:zero, expected:0));
            TestNoGC(code:() => Last(source:zero, expected:0, predicate:(value) => value == 0));
            Assert.Throws<InvalidOperationException>(
                code:() => Last(source:zero, expected:default, predicate:(value) => false)
            );
            TestNoGC(code:() => Last(source:one, expected:1));
            TestNoGC(code:() => Last(source:one, expected:1, predicate:(value) => value > 0));
            Assert.Throws<InvalidOperationException>(
                code:() => Last(source:one, expected:default, predicate:(value) => false)
            );
            TestNoGC(code:() => Last(source:two, expected:2));
            TestNoGC(code:() => Last(source:two, expected:2, predicate:(value) => value < 5));
            Assert.Throws<InvalidOperationException>(
                code:() => Last(source:two, expected:default, predicate:(value) => false)
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Last<TSource> (List<TSource> source, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.Last();
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Last<TSource> (
            List<TSource> source, 
            TSource expected, 
            Func<TSource, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.Last(predicate:predicate);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void LastOrDefault () {
            var empty = new List<int>();
            var zero = new List<int>(collection:new[] { 2, 1, 0 });
            var one = new List<int>(collection:new[] { 3, 2, 1 });
            var two = new List<int>(collection:new[] { 4, 3, 2 });

            TestNoGC(code:() => LastOrDefault(source:empty, expected:default));
            TestNoGC(
                code:() => LastOrDefault(
                    source:empty, 
                    expected:default, 
                    predicate:(value) => throw new InvalidProgramException()
                )
            );
            TestNoGC(code:() => LastOrDefault(source:zero, expected:0));
            TestNoGC(code:() => LastOrDefault(source:zero, expected:0, predicate:(value) => value == 0));
            TestNoGC(code:() => LastOrDefault(source:zero, expected:default, predicate:(value) => false));
            TestNoGC(code:() => LastOrDefault(source:one, expected:1));
            TestNoGC(code:() => LastOrDefault(source:one, expected:1, predicate:(value) => value > 0));
            TestNoGC(code:() => LastOrDefault(source:one, expected:default, predicate:(value) => false));
            TestNoGC(code:() => LastOrDefault(source:two, expected:2));
            TestNoGC(code:() => LastOrDefault(source:two, expected:2, predicate:(value) => value < 5));
            TestNoGC(code:() => LastOrDefault(source:two, expected:default, predicate:(value) => false));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void LastOrDefault<TSource> (List<TSource> source, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.LastOrDefault();
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void LastOrDefault<TSource> (
            List<TSource> source, 
            TSource expected,
            Func<TSource, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.LastOrDefault(predicate:predicate);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void List () {
            var empty = new List<int>();
            var diff = new List<int> { 0, 1, 2, };
            
            TestNoGC(code:() => List(source:empty));
            TestNoGC(code:() => List(source:diff));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void List<TSource> (List<TSource> source) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable) {
                AssertAreEqual(expected:source[index:visited], actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Max () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            var nullable = new List<int?>(collection:new int?[] { 0, default, 4 }).AsEnumerable();
            
            Assert.Throws<InvalidOperationException>(code:() => empty.Max());
            TestNoGC(code:() => same.Max(), expected:0);
            TestNoGC(code:() => diff.Max(), expected:2);
            TestNoGC(code:() => nullable.Max(), expected:4);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Min () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            var nullable = new List<int?>(collection:new int?[] { 0, default, 4 }).AsEnumerable();
            
            Assert.Throws<InvalidOperationException>(code:() => empty.Min());
            TestNoGC(code:() => same.Min(), expected:0);
            TestNoGC(code:() => diff.Min(), expected:0);
            TestNoGC(code:() => nullable.Min(), expected:0);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Mismatch () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            var also = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:same));
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:diff));
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:also));
            TestNoGC(code:() => Mismatch(expected:0, first:same, second:empty));
            TestNoGC(code:() => Mismatch(expected:1, first:same, second:diff));
            TestNoGC(code:() => Mismatch(expected:1, first:same, second:also));
            TestNoGC(code:() => Mismatch(expected:0, first:diff, second:empty));
            TestNoGC(code:() => Mismatch(expected:1, first:diff, second:same));
            TestNoGC(code:() => Mismatch(expected:default, first:diff, second:also));
            TestNoGC(code:() => Mismatch(expected:0, first:also, second:empty));
            TestNoGC(code:() => Mismatch(expected:1, first:also, second:same));
            TestNoGC(code:() => Mismatch(expected:default, first:also, second:diff));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Mismatch<TSource> (int? expected, List<TSource> first, List<TSource> second) {
            var firstAdapter = first.AsEnumerable();
            var secondAdapter = second.AsEnumerable();

            // ReSharper disable once JoinDeclarationAndInitializer
            int? result;

            result = firstAdapter.Mismatch(second:firstAdapter);
            AssertAreEqual(expected:default, actual:result);

            result = firstAdapter.Mismatch(second:firstAdapter, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:default, actual:result);

            result = secondAdapter.Mismatch(second:secondAdapter);
            AssertAreEqual(expected:default, actual:result);

            result = secondAdapter.Mismatch(second:secondAdapter, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:default, actual:result);

            result = firstAdapter.Mismatch(second:secondAdapter);
            AssertAreEqual(expected:expected, actual:result);

            result = firstAdapter.Mismatch(second:secondAdapter, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:expected, actual:result);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void None () {
            var empty = new List<int>().AsEnumerable();
            var same = new List<int>(collection:new[] { 0, 0, 0 }).AsEnumerable();
            var diff = new List<int>(collection:new[] { 0, 1, 2 }).AsEnumerable();
            
            ValidateIsFalse(code:() => same.None());
            ValidateIsFalse(code:() => diff.None());
            ValidateIsFalse(code:() => same.None(predicate:(value) => value == 0));
            ValidateIsFalse(code:() => diff.None(predicate:(value) => value == 0));
            ValidateIsTrue(code:() => empty.None());
            ValidateIsTrue(code:() => empty.None(predicate:(value) => throw new InvalidOperationException()));
            ValidateIsTrue(code:() => same.None(predicate:(value) => value < 0));
            ValidateIsTrue(code:() => diff.None(predicate:(value) => value < 0));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void OfType () {
            var empty = new List<object>();
            var diff = new List<object>(collection:new object[] { 0, "1", "2", typeof(Type) });
            
            TestNoGC(code:() => OfType<object>(source:empty, expected:0));
            TestNoGC(code:() => OfType<Type>(source:empty, expected:0));
            TestNoGC(code:() => OfType<string>(source:empty, expected:0));
            TestNoGC(code:() => OfType<object>(source:diff, expected:4));
            TestNoGC(code:() => OfType<Type>(source:diff, expected:1));
            TestNoGC(code:() => OfType<string>(source:diff, expected:2));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void OfType<TResult> (List<object> source, int expected) 
            where TResult : class
        {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.OfType<TResult>()) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Prepend () {
            var empty = new List<int>();
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Prepend(source:empty, element:0));
            TestNoGC(code:() => Prepend(source:diff, element:-1));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Prepend<TSource> (List<TSource> source, TSource element) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var test in enumerable.Prepend(element:element)) {
                var value = visited > 0 ? source[index:visited - 1] : element;
                AssertAreEqual(expected:value, actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count + 1, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        #if false
        [Test]
        public static void Reverse () {
            var empty = new List<int>();
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            Validate(code:() => Reverse(source:empty));
            Validate(code:() => Reverse(source:diff));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Reverse<TSource> (List<TSource> source) {
            var enumerable = source.ToAdapter();
            
            var visited = 0;
            foreach (var test in enumerable.Reverse()) {
                var value = source[index:source.Count - 1 - visited];
                AssertAreEqual(expected:value, actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }
        #endif

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Range () {
            var negative = new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, };
            var positive = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, };
            var powers = new[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, };
            
            TestNoGC(code:() => Range(start:-5, count:10, elements:negative));
            TestNoGC(
                code:() => Range(start:-5, count:10, elements:negative, generator:(start, index) => start + index)
            );
            TestNoGC(code:() => Range(start:5, count:10, elements:positive));
            TestNoGC(code:() => Range(start:5, count:10, elements:positive, generator:(start, index) => start + index));
            TestNoGC(
                code:() => Range(start:0, count:10, elements:powers, generator:(start, index) => 1 << (start + index))
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Range (int start, int count, int[] elements) {
            var visited = 0;
            foreach (var result in Enumerable.Range(start:start, count:count)) {
                AssertAreEqual(expected:elements[visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Range<TSource> (
            TSource start, 
            int count, 
            Func<TSource, int, TSource> generator, 
            TSource[] elements
        ) {
            var visited = 0;
            foreach (var result in Enumerable.Range(start:start, count:count, generator:generator)) {
                AssertAreEqual(expected:elements[visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Repeat () {
            TestNoGC(code:() => Repeat(element:0, count:5));
            TestNoGC(code:() => Repeat(element:0.0, count:10));
            TestNoGC(code:() => Repeat(element:"Test", count:10));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Repeat<TSource> (TSource element, int count) {
            var visited = 0;
            foreach (var result in Enumerable.Repeat(element:element, count:count)) {
                AssertAreEqual(expected:element, actual:result);
                ++visited;
            }
            AssertAreEqual(expected:count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Replace () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Replace(source:empty, what:0, with:0, expected:0));
            TestNoGC(code:() => Replace(source:same, what:0, with:0, expected:0));
            TestNoGC(code:() => Replace(source:same, what:0, with:1, expected:3));
            TestNoGC(code:() => Replace(source:same, what:1, with:0, expected:0));
            TestNoGC(code:() => Replace(source:diff, what:0, with:1, expected:1));
            TestNoGC(code:() => Replace(source:diff, what:1, with:0, expected:1));
            TestNoGC(code:() => Replace(source:diff, what:2, with:0, expected:1));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Replace<TSource> (List<TSource> source, TSource what, TSource with, int expected) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            var replaced = 0;
            var equalityComparer = EqualityComparer<TSource>.Default;
            foreach (var result in enumerable.Replace(what:what, with:with, equalityComparer:equalityComparer)) {
                var was = source[index:visited];
                if (equalityComparer.Equals(x:what, y:was) && 
                    !equalityComparer.Equals(x:with, y:was) && 
                    equalityComparer.Equals(x:with, y:result)
                ) {
                    ++replaced;
                }
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:replaced);
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Select () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(
                code:() => Select<int, float>(
                    source:empty, 
                    selectTo:(value) => throw new InvalidOperationException(), 
                    selectFrom:(value) => throw new InvalidOperationException()
                )
            );
            TestNoGC(
                code:() => Select(source:same, selectTo:(value) => (float)value, selectFrom:(value) => (int)value)
            );
            TestNoGC(
                code:() => Select(
                    source:same, 
                    selectTo:(value, index) => (float)value, 
                    selectFrom:(value, index) => (int)value
                )
            );
            TestNoGC(
                code:() => Select(source:diff, selectTo:(value) => (float)value, selectFrom:(value) => (int)value)
            );
            TestNoGC(
                code:() => Select(
                    source:diff, 
                    selectTo:(value, index) => (float)value, 
                    selectFrom:(value, index) => (int)value
                )
            );
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Select<TSource, TResult> (
            List<TSource> source, 
            Func<TSource, TResult> selectTo, 
            Func<TResult, TSource> selectFrom
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var result in enumerable.Select(selector:selectTo).Select(selector:selectFrom)) {
                AssertAreEqual(expected:source[index:visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Select<TSource, TResult> (
            List<TSource> source, 
            Func<TSource, int, TResult> selectTo, 
            Func<TResult, int, TSource> selectFrom
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var result in enumerable.Select(selector:selectTo).Select(selector:selectFrom)) {
                AssertAreEqual(expected:source[index:visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SelectMany () {
            var empty = new List<List<int>>();
            var diff = new List<List<int>>(
                collection:new[] { new List<int>(), new List<int>(collection:new[] { 0, 1, 2 })}
            );
            
            TestNoGC(code:() => SelectMany(source:empty, selector:(source) => source.AsEnumerable(), expected:0));
            TestNoGC(
                code:() => SelectMany(source:empty, selector:(source, index) => source.AsEnumerable(), expected:0)
            );
            TestNoGC(
                code:() => SelectMany(
                    source:empty, 
                    collectionSelector:(source) => source.AsEnumerable(), 
                    resultSelector:(source, indirect) => (float)indirect, 
                    expected:0
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:empty, 
                    collectionSelector:(source, index) => source.AsEnumerable(), 
                    resultSelector:(source, indirect) => (float)indirect, 
                    expected:0
                )
            );
            TestNoGC(code:() => SelectMany(source:diff, selector:(source) => source.AsEnumerable(), expected:3));
            TestNoGC(
                code:() => SelectMany(source:diff, selector:(source, index) => source.AsEnumerable(), expected:3)
            );
            TestNoGC(
                code:() => SelectMany(
                    source:diff, 
                    collectionSelector:(source) => source.AsEnumerable(), 
                    resultSelector:(source, indirect) => (float)indirect, 
                    expected:3
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:diff, 
                    collectionSelector:(source, index) => source.AsEnumerable(), 
                    resultSelector:(source, indirect) => (float)indirect, 
                    expected:3
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<TSource, TResult> (
            List<List<TSource>> source,
            Func<List<TSource>, EnumerableAdapter<ListEnumerator<TResult>, TResult>> selector,
            int expected
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.SelectMany(selector:selector)) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<TSource, TResult> (
            List<List<TSource>> source,
            Func<List<TSource>, int, EnumerableAdapter<ListEnumerator<TResult>, TResult>> selector,
            int expected
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.SelectMany(selector:selector)) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<TSource, TIndirect, TResult> (
            List<List<TSource>> source,
            Func<List<TSource>, EnumerableAdapter<ListEnumerator<TIndirect>, TIndirect>> collectionSelector,
            Func<List<TSource>, TIndirect, TResult> resultSelector,
            int expected
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.SelectMany(
                    collectionSelector:collectionSelector, 
                    resultSelector:resultSelector
                )
            ) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<TSource, TIndirect, TResult> (
            List<List<TSource>> source,
            Func<List<TSource>, int, EnumerableAdapter<ListEnumerator<TIndirect>, TIndirect>> collectionSelector,
            Func<List<TSource>, TIndirect, TResult> resultSelector,
            int expected
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.SelectMany(
                    collectionSelector:collectionSelector, 
                    resultSelector:resultSelector
                )
            ) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SequenceEqual () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            var also = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => SequenceEqual(expected:false, first:empty, second:same));
            TestNoGC(code:() => SequenceEqual(expected:false, first:empty, second:diff));
            TestNoGC(code:() => SequenceEqual(expected:false, first:empty, second:also));
            TestNoGC(code:() => SequenceEqual(expected:false, first:same, second:empty));
            TestNoGC(code:() => SequenceEqual(expected:false, first:same, second:diff));
            TestNoGC(code:() => SequenceEqual(expected:false, first:same, second:also));
            TestNoGC(code:() => SequenceEqual(expected:false, first:diff, second:empty));
            TestNoGC(code:() => SequenceEqual(expected:false, first:diff, second:same));
            TestNoGC(code:() => SequenceEqual(expected:true, first:diff, second:also));
            TestNoGC(code:() => SequenceEqual(expected:false, first:also, second:empty));
            TestNoGC(code:() => SequenceEqual(expected:false, first:also, second:same));
            TestNoGC(code:() => SequenceEqual(expected:true, first:also, second:diff));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SequenceEqual<TSource> (bool expected, List<TSource> first, List<TSource> second) {
            var firstAdapter = first.AsEnumerable();
            var secondAdapter = second.AsEnumerable();
            
            // ReSharper disable once JoinDeclarationAndInitializer
            bool result;

            result = firstAdapter.SequenceEqual(second:firstAdapter);
            AssertAreEqual(expected:true, actual:result);

            result = firstAdapter.SequenceEqual(second:firstAdapter, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:true, actual:result);

            result = secondAdapter.SequenceEqual(second:secondAdapter);
            AssertAreEqual(expected:true, actual:result);

            result = secondAdapter.SequenceEqual(second:secondAdapter, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:true, actual:result);

            result = firstAdapter.SequenceEqual(second:secondAdapter);
            AssertAreEqual(expected:expected, actual:result);

            result = firstAdapter.SequenceEqual(second:secondAdapter, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:expected, actual:result);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Single () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });

            Assert.Throws<InvalidOperationException>(code:() => Single(source:empty, expected:0));
            Assert.Throws<InvalidOperationException>(
                code:() => Single(
                    source:empty, 
                    expected:0, 
                    //  Wrap this invalid exception with an aggregate because it's not the expected throw
                    //
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            Assert.Throws<InvalidOperationException>(code:() => Single(source:same, expected:0));
            Assert.Throws<InvalidOperationException>(
                code:() => Single(source:same, expected:0, predicate:(value) => value == 1)
            );
            Assert.Throws<InvalidOperationException>(code:() => Single(source:diff, expected:0));
            TestNoGC(code:() => Single(expected:2, source:diff, predicate:(value) => value == 2));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Single<TSource> (List<TSource> source, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.Single();
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Single<TSource> (
            List<TSource> source, 
            TSource expected, 
            Func<TSource, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.Single(predicate:predicate);
            AssertAreEqual(expected:expected, actual:result);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SingleOrDefault () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });

            TestNoGC(code:() => SingleOrDefault(source:empty, expected:default));
            TestNoGC(
                code:() => SingleOrDefault(
                    source:empty, 
                    expected:default, 
                    predicate:(value) => throw new InvalidOperationException()
                )
            );
            Assert.Throws<InvalidOperationException>(code:() => SingleOrDefault(source:same, expected:0));
            Assert.Throws<InvalidOperationException>(
                code:() => SingleOrDefault(source:same, expected:0, predicate:(value) => value == 0)
            );
            TestNoGC(code:() => SingleOrDefault(source:same, expected:default, predicate:(value) => value == 1));
            Assert.Throws<InvalidOperationException>(code:() => SingleOrDefault(source:diff, expected:0));
            Assert.Throws<InvalidOperationException>(
                code:() => SingleOrDefault(source:diff, expected:0, predicate:(value) => value < 2)
            );
            TestNoGC(code:() => SingleOrDefault(source:diff, expected:2, predicate:(value) => value == 2));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void SingleOrDefault<TSource> (List<TSource> source, TSource expected) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.SingleOrDefault();
            AssertAreEqual(expected:expected, actual:result);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void SingleOrDefault<TSource> (
            List<TSource> source, 
            TSource expected, 
            Func<TSource, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var result = enumerable.SingleOrDefault(predicate:predicate);
            AssertAreEqual(expected:expected, actual:result);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Skip () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Skip(source:empty, count:0, skipped:0, remaining:0));
            TestNoGC(code:() => Skip(source:empty, count:1, skipped:0, remaining:0));
            TestNoGC(code:() => Skip(source:empty, count:10, skipped:0, remaining:0));
            TestNoGC(code:() => Skip(source:same, count:0, skipped:0, remaining:3));
            TestNoGC(code:() => Skip(source:same, count:1, skipped:1, remaining:2));
            TestNoGC(code:() => Skip(source:same, count:10, skipped:3, remaining:0));
            TestNoGC(code:() => Skip(source:diff, count:0, skipped:0, remaining:3));
            TestNoGC(code:() => Skip(source:diff, count:1, skipped:1, remaining:2));
            TestNoGC(code:() => Skip(source:diff, count:10, skipped:3, remaining:0));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Skip<TSource> (
            List<TSource> source, 
            int count, 
            int skipped, 
            int remaining
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var result in enumerable.Skip(count:count)) {
                AssertAreEqual(expected:source[index:count + visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:skipped, actual:source.Count - visited);
            AssertAreEqual(expected:remaining, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SkipWhile () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            SkipWhile(source:empty, count:0, skipped:0, remaining:0);
            SkipWhile(source:empty, count:1, skipped:0, remaining:0);
            SkipWhile(source:empty, count:10, skipped:0, remaining:0);
            SkipWhile(source:same, count:0, skipped:0, remaining:3);
            SkipWhile(source:same, count:1, skipped:1, remaining:2);
            SkipWhile(source:same, count:10, skipped:3, remaining:0);
            SkipWhile(source:diff, count:0, skipped:0, remaining:3);
            SkipWhile(source:diff, count:1, skipped:1, remaining:2);
            SkipWhile(source:diff, count:10, skipped:3, remaining:0);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void SkipWhile<TSource> (
            List<TSource> source, 
            int count, 
            int skipped, 
            int remaining
        ) {
            //  Using a lambda rather than a local function allows us to capture outside of the Validate.
            // ReSharper disable once ConvertToLocalFunction
            // ReSharper disable once ImplicitlyCapturedClosure
            Func<TSource, int, bool> predicate = (value, index) => index < count;
            TestNoGC(
                code:() => SkipWhile(
                    source:source,
                    count:count,
                    skipped:skipped,
                    remaining:remaining,
                    predicate:predicate
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SkipWhile<TSource> (
            List<TSource> source,
            int count,
            int skipped,
            int remaining,
            Func<TSource, int, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var result in enumerable.SkipWhile(predicate:predicate)) {
                AssertAreEqual(expected:source[index:count + visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:skipped, actual:source.Count - visited);
            AssertAreEqual(expected:remaining, actual:visited);
        }
         
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Take () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(code:() => Take(source:empty, count:0, taken:0, remaining:0));
            TestNoGC(code:() => Take(source:empty, count:1, taken:0, remaining:0));
            TestNoGC(code:() => Take(source:empty, count:10, taken:0, remaining:0));
            TestNoGC(code:() => Take(source:same, count:0, taken:0, remaining:3));
            TestNoGC(code:() => Take(source:same, count:1, taken:1, remaining:2));
            TestNoGC(code:() => Take(source:same, count:10, taken:3, remaining:0));
            TestNoGC(code:() => Take(source:diff, count:0, taken:0, remaining:3));
            TestNoGC(code:() => Take(source:diff, count:1, taken:1, remaining:2));
            TestNoGC(code:() => Take(source:diff, count:10, taken:3, remaining:0));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Take<TSource> (
            List<TSource> source, 
            int count, 
            int taken, 
            int remaining
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var result in enumerable.Take(count:count)) {
                AssertAreEqual(expected:source[index:visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:taken, actual:visited);
            AssertAreEqual(expected:remaining, actual:source.Count - visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void TakeWhile () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TakeWhile(source:empty, count:0, taken:0, remaining:0);
            TakeWhile(source:empty, count:1, taken:0, remaining:0);
            TakeWhile(source:empty, count:10, taken:0, remaining:0);
            TakeWhile(source:same, count:0, taken:0, remaining:3);
            TakeWhile(source:same, count:1, taken:1, remaining:2);
            TakeWhile(source:same, count:10, taken:3, remaining:0);
            TakeWhile(source:diff, count:0, taken:0, remaining:3);
            TakeWhile(source:diff, count:1, taken:1, remaining:2);
            TakeWhile(source:diff, count:10, taken:3, remaining:0);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void TakeWhile<TSource> (
            List<TSource> source, 
            int count, 
            int taken, 
            int remaining
        ) {
            //  Using a lambda rather than a local function allows us to capture outside of the Validate.
            // ReSharper disable once ConvertToLocalFunction
            // ReSharper disable once ImplicitlyCapturedClosure
            Func<TSource, int, bool> predicate = (value, index) => index < count;
            TestNoGC(
                code:() => TakeWhile(
                    source:source,
                    taken:taken,
                    remaining:remaining,
                    predicate:predicate
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void TakeWhile<TSource> (
            List<TSource> source,
            int taken,
            int remaining,
            Func<TSource, int, bool> predicate
        ) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var result in enumerable.TakeWhile(predicate:predicate)) {
                AssertAreEqual(expected:source[index:visited], actual:result);
                ++visited;
            }
            AssertAreEqual(expected:taken, actual:visited);
            AssertAreEqual(expected:remaining, actual:source.Count - visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Where () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });
            
            TestNoGC(
                code:() => Where(source:empty, predicate:(value) => throw new InvalidOperationException(), expected:0)
            );
            TestNoGC(
                code:() => Where(
                    source:empty, 
                    predicate:(value, index) => throw new InvalidOperationException(), 
                    expected:0
                )
            );
            TestNoGC(
                code:() => Where(source:same, predicate:(value) => value == 0, expected:3)
            );
            TestNoGC(
                code:() => Where(source:same, predicate:(value, index) => value == 0, expected:3)
            );
            TestNoGC(
                code:() => Where(source:diff, predicate:(value) => value == 0, expected:1)
            );
            TestNoGC(
                code:() => Where(source:diff, predicate:(value, index) => value == 0, expected:1)
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Where<TSource> (List<TSource> source, Func<TSource, bool> predicate, int expected) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.Where(predicate:predicate)) {
                ++visited;
            }
            AssertAreEqual(expected:visited, actual:expected);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Where<TSource> (List<TSource> source, Func<TSource, int, bool> predicate, int expected) {
            var enumerable = source.AsEnumerable();
            var visited = 0;
            foreach (var _ in enumerable.Where(predicate:predicate)) {
                ++visited;
            }
            AssertAreEqual(expected:visited, actual:expected);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Zip () {
            var empty = new List<int>();
            var same = new List<int>(collection:new[] { 0, 0, 0 });
            var diff = new List<int>(collection:new[] { 0, 1, 2 });

            Zip(first:empty, second:empty);
            Zip(first:empty, second:same);
            Zip(first:empty, second:diff);
            Zip(first:same, second:diff);
            Zip(first:same, second:empty);
            Zip(first:diff, second:same);
            Zip(first:diff, second:empty);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Zip<TFirstSource, TSecondSource> (
            List<TFirstSource> first,
            List<TSecondSource> second
        ) {
            //  Using a lambda rather than a local function allows us to capture outside of the Validate.
            // ReSharper disable once ConvertToLocalFunction
            Func<TFirstSource, TSecondSource, (TFirstSource, TSecondSource)> resultSelector = (f, s) => (f, s);
            // ReSharper disable once ConvertToLocalFunction
            Func<TFirstSource, TSecondSource, (TFirstSource, TSecondSource), bool> resultTester = (f, s, p) => 
                EqualityComparer<TFirstSource>.Default.Equals(x:f, y:p.Item1) && 
                EqualityComparer<TSecondSource>.Default.Equals(x:s, y:p.Item2)
            ;
            TestNoGC(
                code:() => Zip(first:first, second:second, resultSelector:resultSelector, resultTester:resultTester)
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Zip<TFirstSource, TSecondSource, TResult> (
            List<TFirstSource> first, 
            List<TSecondSource> second, 
            Func<TFirstSource, TSecondSource, TResult> resultSelector,
            Func<TFirstSource, TSecondSource, TResult, bool> resultTester
        ) {
            var firstAdapter = first.AsEnumerable();
            var secondAdapter = second.AsEnumerable();
            var visited = 0;
            foreach (var result in firstAdapter.Zip(second:secondAdapter, resultSelector:resultSelector)) {
                AssertAreEqual(
                    expected:true,
                    actual:resultTester(arg1:first[index:visited], arg2:second[index:visited], arg3:result)
                );
                ++visited;
            }
            AssertAreEqual(expected:Math.Min(val1:first.Count, val2:second.Count), actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private interface IExperiment<out TSource> : IDisposable {
            TSource Current { get; }
            bool MoveNext ();
            void Reset ();
        }

        //--------------------------------------------------------------------------------------------------------------
        private struct Experiment<TSource> : IExperiment<TSource> {
            TSource IExperiment<TSource>.Current => default;
            public void Dispose () { }
            bool IExperiment<TSource>.MoveNext () => false;
            void IExperiment<TSource>.Reset () { }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void ExperimentTest () {
            TestNoGC(code:() => ExperimentTest<Experiment<int>, int>(experiment:new Experiment<int>()));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void ExperimentTest<TExperiment, TSource> (TExperiment experiment)
            where TExperiment : IExperiment<TSource> {
            using (experiment) {
                while (experiment.MoveNext()) { }
            }
        }
    }
    
}