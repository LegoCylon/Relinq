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
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private static readonly int[] s_emptyArray = new int[] { };
        private static readonly int[] s_repeatArray = new int[] { 1, 1, 1, 1, 1, };
        private static readonly int[] s_sequenceArray = new int[] { 0, 1, 2, 3, 4, };
        private static readonly HashSet<int> s_emptyHashSet = new HashSet<int>();
        private static readonly HashSet<int> s_repeatHashSet = new HashSet<int> { 1, 1, 1, 1, 1, };
        private static readonly HashSet<int> s_sequenceHashSet = new HashSet<int> { 0, 1, 2, 3, 4, };
        private static readonly IList<int> s_emptyIList = new List<int>();
        private static readonly IList<int> s_repeatIList = new List<int> { 1, 1, 1, 1, 1, };
        private static readonly IList<int> s_sequenceIList = new List<int> { 0, 1, 2, 3, 4, };
        private static readonly IReadOnlyList<int> s_emptyIReadOnlyList = new List<int>();
        private static readonly IReadOnlyList<int> s_repeatIReadOnlyList = new List<int> { 1, 1, 1, 1, 1, };
        private static readonly IReadOnlyList<int> s_sequenceIReadOnlyList = new List<int> { 0, 1, 2, 3, 4, };
        private static readonly LinkedList<int> s_emptyLinkedList = new LinkedList<int>();
        private static readonly LinkedList<int> s_repeatLinkedList = new LinkedList<int> { 1, 1, 1, 1, 1, };
        private static readonly LinkedList<int> s_sequenceLinkedList = new LinkedList<int> { 0, 1, 2, 3, 4, };
        private static readonly List<int> s_emptyList = new List<int>();
        private static readonly List<int> s_repeatList = new List<int> { 1, 1, 1, 1, 1, };
        private static readonly List<int> s_sequenceList = new List<int> { 0, 1, 2, 3, 4, };

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
        private static HashSet<T> NewHashSet<T> (T value) => new HashSet<T> { value, };

        //--------------------------------------------------------------------------------------------------------------
        private static IList<T> NewIList<T> (T value) => new List<T> { value, };

        //--------------------------------------------------------------------------------------------------------------
        private static IReadOnlyList<T> NewIReadOnlyList<T> (T value) => new List<T> { value, };

        //--------------------------------------------------------------------------------------------------------------
        private static LinkedList<T> NewLinkedList<T> (T value) => new LinkedList<T> { value, };
        
        //--------------------------------------------------------------------------------------------------------------
        private static List<T> NewList<T> (T value) => new List<T> { value, };
        
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
        public static void AddHashSet () {
            AddHashSet(source:s_emptyArray.AsEnumerable());
            AddHashSet(source:s_repeatArray.AsEnumerable());
            AddHashSet(source:s_sequenceArray.AsEnumerable());
            AddHashSet(source:s_emptyHashSet.AsEnumerable());
            AddHashSet(source:s_repeatHashSet.AsEnumerable());
            AddHashSet(source:s_sequenceHashSet.AsEnumerable());
            AddHashSet(source:s_emptyIList.AsEnumerable());
            AddHashSet(source:s_repeatIList.AsEnumerable());
            AddHashSet(source:s_sequenceIList.AsEnumerable());
            AddHashSet(source:s_emptyIReadOnlyList.AsEnumerable());
            AddHashSet(source:s_repeatIReadOnlyList.AsEnumerable());
            AddHashSet(source:s_sequenceIReadOnlyList.AsEnumerable());
            AddHashSet(source:s_emptyLinkedList.AsEnumerable());
            AddHashSet(source:s_repeatLinkedList.AsEnumerable());
            AddHashSet(source:s_sequenceLinkedList.AsEnumerable());
            AddHashSet(source:s_emptyList.AsEnumerable());
            AddHashSet(source:s_repeatList.AsEnumerable());
            AddHashSet(source:s_sequenceList.AsEnumerable());
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void AddHashSet<TEnumerator, TSource> (EnumerableAdapter<TEnumerator, TSource> source)
            where TEnumerator : IAdaptableEnumerator<TSource> {
            var added = new HashSet<TSource>();
            added.AddRange(enumerable:source);

            foreach (var element in source) {
                AssertAreEqual(expected:true, actual:added.Contains(item:element));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void AddList () {
            AddList(source:s_emptyArray.AsEnumerable());
            AddList(source:s_repeatArray.AsEnumerable());
            AddList(source:s_sequenceArray.AsEnumerable());
            AddList(source:s_emptyHashSet.AsEnumerable());
            AddList(source:s_repeatHashSet.AsEnumerable());
            AddList(source:s_sequenceHashSet.AsEnumerable());
            AddList(source:s_emptyIList.AsEnumerable());
            AddList(source:s_repeatIList.AsEnumerable());
            AddList(source:s_sequenceIList.AsEnumerable());
            AddList(source:s_emptyIReadOnlyList.AsEnumerable());
            AddList(source:s_repeatIReadOnlyList.AsEnumerable());
            AddList(source:s_sequenceIReadOnlyList.AsEnumerable());
            AddList(source:s_emptyLinkedList.AsEnumerable());
            AddList(source:s_repeatLinkedList.AsEnumerable());
            AddList(source:s_sequenceLinkedList.AsEnumerable());
            AddList(source:s_emptyList.AsEnumerable());
            AddList(source:s_repeatList.AsEnumerable());
            AddList(source:s_sequenceList.AsEnumerable());
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void AddList<TEnumerator, TSource> (EnumerableAdapter<TEnumerator, TSource> source)
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var added = new List<TSource> { source };
            
            AssertAreEqual(expected:source.Count(), actual:added.Count);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Aggregate () {
            TestNoGC(code:() => Aggregate(source:s_emptyArray.AsEnumerable(), sum:0));
            TestNoGC(code:() => Aggregate(source:s_repeatArray.AsEnumerable(), sum:5));
            TestNoGC(code:() => Aggregate(source:s_sequenceArray.AsEnumerable(), sum:10));
            TestNoGC(code:() => Aggregate(source:s_emptyHashSet.AsEnumerable(), sum:0));
            TestNoGC(code:() => Aggregate(source:s_repeatHashSet.AsEnumerable(), sum:1));
            TestNoGC(code:() => Aggregate(source:s_sequenceHashSet.AsEnumerable(), sum:10));
            TestNoGC(code:() => Aggregate(source:s_emptyIList.AsEnumerable(), sum:0));
            TestNoGC(code:() => Aggregate(source:s_repeatIList.AsEnumerable(), sum:5));
            TestNoGC(code:() => Aggregate(source:s_sequenceIList.AsEnumerable(), sum:10));
            TestNoGC(code:() => Aggregate(source:s_emptyIReadOnlyList.AsEnumerable(), sum:0));
            TestNoGC(code:() => Aggregate(source:s_repeatIReadOnlyList.AsEnumerable(), sum:5));
            TestNoGC(code:() => Aggregate(source:s_sequenceIReadOnlyList.AsEnumerable(), sum:10));
            TestNoGC(code:() => Aggregate(source:s_emptyLinkedList.AsEnumerable(), sum:0));
            TestNoGC(code:() => Aggregate(source:s_repeatLinkedList.AsEnumerable(), sum:5));
            TestNoGC(code:() => Aggregate(source:s_sequenceLinkedList.AsEnumerable(), sum:10));
            TestNoGC(code:() => Aggregate(source:s_emptyList.AsEnumerable(), sum:0));
            TestNoGC(code:() => Aggregate(source:s_repeatList.AsEnumerable(), sum:5));
            TestNoGC(code:() => Aggregate(source:s_sequenceList.AsEnumerable(), sum:10));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Aggregate<TEnumerator> (EnumerableAdapter<TEnumerator, int> source, int sum) 
            where TEnumerator : IAdaptableEnumerator<int> 
        {
            AssertAreEqual(
                expected:sum,
                actual:source.Aggregate(
                    seed:0, 
                    func:(accumulate, value) => accumulate + value, 
                    resultSelector:(accumulate) => accumulate
                )
            );
            AssertAreEqual(
                expected:10 + sum,
                actual:source.Aggregate(
                    seed:10, 
                    func:(accumulate, value) => accumulate + value, 
                    resultSelector:(accumulate) => accumulate
                )
            );
            AssertAreEqual(
                expected:sum,
                actual:source.Aggregate(
                    seed:10, 
                    func:(accumulate, value) => accumulate + value, 
                    resultSelector:(accumulate) => accumulate - 10
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void All () {
            TestNoGC(
                code:() => All(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    sequence:s_sequenceArray.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => All(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => All(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => All(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => All(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => All(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable()
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void All<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            ValidateIsTrue(code:() => empty.All(predicate:(value) => throw new InvalidOperationException()));
            ValidateIsTrue(code:() => repeat.All(predicate:(value) => value >= 0));
            ValidateIsTrue(code:() => sequence.All(predicate:(value) => value >= 0));
            ValidateIsFalse(code:() => repeat.All(predicate:(value) => value < 0));
            ValidateIsFalse(code:() => sequence.All(predicate:(value) => value < 0));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Any () {
            TestNoGC(
                code:() => Any(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    sequence:s_sequenceArray.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Any(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Any(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Any(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Any(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Any(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable()
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Any<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            ValidateIsTrue(code:() => repeat.Any());
            ValidateIsTrue(code:() => sequence.Any());
            ValidateIsTrue(code:() => repeat.Any(predicate:(value) => value == 1));
            ValidateIsTrue(code:() => sequence.Any(predicate:(value) => value == 0));
            ValidateIsFalse(code:() => empty.Any());
            ValidateIsFalse(code:() => empty.Any(predicate:(value) => throw new InvalidOperationException()));
            ValidateIsFalse(code:() => repeat.Any(predicate:(value) => value < 1));
            ValidateIsFalse(code:() => sequence.Any(predicate:(value) => value < 0));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Append () {
            TestNoGC(
                code:() => Append(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    sequence:s_sequenceArray.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Append(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Append(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Append(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Append(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Append(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable()
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Append<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource> 
        {
            Append(source:empty, element:default);
            Append(source:repeat, element:default);
            Append(source:sequence, element:default);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Append<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            TSource element
        ) 
            where TEnumerator : IAdaptableEnumerator<TSource> 
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in source.Append(element:element)) {
                    var value = enumerator.MoveNext() ? enumerator.Current : element;
                    AssertAreEqual(expected:value, actual:test);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count() + 1, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Array () {
            TestNoGC(code:() => Array(source:s_emptyArray));
            TestNoGC(code:() => Array(source:s_repeatArray));
            TestNoGC(code:() => Array(source:s_sequenceArray));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Array<TSource> (TSource[] source) {
            var visited = 0;
            foreach (var test in source.AsEnumerable()) {
                AssertAreEqual(expected:source[visited], actual:test);
                ++visited;
            }
            AssertAreEqual(expected:source.Length, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Cast () {
            TestNoGC(
                code:() => Cast(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    sequence:s_sequenceArray.AsEnumerable(), 
                    resultSelector:(value) => (float)value
                )
            );
            TestNoGC(
                code:() => Cast(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable(), 
                    resultSelector:(value) => (float)value
                )
            );
            TestNoGC(
                code:() => Cast(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable(), 
                    resultSelector:(value) => (float)value
                )
            );
            TestNoGC(
                code:() => Cast(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable(), 
                    resultSelector:(value) => (float)value
                )
            );
            TestNoGC(
                code:() => Cast(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable(), 
                    resultSelector:(value) => (float)value
                )
            );
            TestNoGC(
                code:() => Cast(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable(), 
                    resultSelector:(value) => (float)value
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Cast<TEnumerable, TSource, TCast> (
            EnumerableAdapter<TEnumerable, TSource> empty,
            EnumerableAdapter<TEnumerable, TSource> repeat,
            EnumerableAdapter<TEnumerable, TSource> sequence,
            Func<TSource, TCast> resultSelector
        )
            where TEnumerable : IAdaptableEnumerator<TSource>
            where TCast : IConvertible
        {
            Cast<TEnumerable, TSource, TCast>(
                source:empty, 
                resultSelector:(value) => throw new InvalidOperationException()
            );
            Cast(source:repeat, resultSelector:resultSelector);
            Cast(source:sequence, resultSelector:resultSelector);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Cast<TEnumerable, TSource, TCast> (
            EnumerableAdapter<TEnumerable, TSource> source, 
            Func<TSource, TCast> resultSelector
        )
            where TEnumerable : IAdaptableEnumerator<TSource>
            where TCast : IConvertible
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in source.Cast(resultSelector:resultSelector)) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    var element = resultSelector(arg:enumerator.Current);
                    AssertAreEqual(expected:element, actual:test);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count(), actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Concat () {
            TestNoGC(code:() => Concat(source:s_emptyArray.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_repeatArray.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_sequenceArray.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_emptyHashSet.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_repeatHashSet.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_sequenceHashSet.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_emptyIList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_repeatIList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_sequenceIList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_emptyIReadOnlyList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_repeatIReadOnlyList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_sequenceIReadOnlyList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_emptyLinkedList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_repeatLinkedList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_sequenceLinkedList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_emptyList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_repeatList.AsEnumerable()));
            TestNoGC(code:() => Concat(source:s_sequenceList.AsEnumerable()));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Concat<TEnumerator, TSource> (EnumerableAdapter<TEnumerator, TSource> source) 
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in source.Concat(second:source)) {
                    if ((visited % source.Count()) == 0) {
                        enumerator.Reset();
                    }
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:test);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count() * 2, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Contains () {
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    repeatHas:1,
                    repeatMissing:0,
                    sequence:s_sequenceArray.AsEnumerable(),
                    sequenceHas:0,
                    sequenceMissing:6
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    repeatHas:1,
                    repeatMissing:0,
                    sequence:s_sequenceHashSet.AsEnumerable(),
                    sequenceHas:0,
                    sequenceMissing:6
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    repeatHas:1,
                    repeatMissing:0,
                    sequence:s_sequenceIList.AsEnumerable(),
                    sequenceHas:0,
                    sequenceMissing:6
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    repeatHas:1,
                    repeatMissing:0,
                    sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                    sequenceHas:0,
                    sequenceMissing:6
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    repeatHas:1,
                    repeatMissing:0,
                    sequence:s_sequenceLinkedList.AsEnumerable(),
                    sequenceHas:0,
                    sequenceMissing:6
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    repeatHas:1,
                    repeatMissing:0,
                    sequence:s_sequenceList.AsEnumerable(),
                    sequenceHas:0,
                    sequenceMissing:6
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Contains<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            TSource repeatHas,
            TSource repeatMissing,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            TSource sequenceHas,
            TSource sequenceMissing
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            AssertAreEqual(expected:false, actual:empty.Contains(value:default));
            AssertAreEqual(expected:true, actual:repeat.Contains(value:repeatHas));
            AssertAreEqual(expected:false, actual:repeat.Contains(value:repeatMissing));
            AssertAreEqual(expected:true, actual:sequence.Contains(value:sequenceHas));
            AssertAreEqual(expected:false, actual:sequence.Contains(value:sequenceMissing));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Count () {
            Func<int, bool> repeatHas = (value) => value == 1;
            Func<int, bool> repeatMissing = (value) => value != 1;
            Func<int, bool> sequenceHas = (value) => value < 5;
            Func<int, bool> sequenceMissing = (value) => value > 5;
            TestNoGC(
                code:() => Count(
                    empty:s_emptyArray.AsEnumerable(),
                    repeat:s_repeatArray.AsEnumerable(),
                    repeatHas:repeatHas,
                    repeatMissing:repeatMissing,
                    repeatCount:5,
                    sequence:s_sequenceArray.AsEnumerable(),
                    sequenceHas:sequenceHas,
                    sequenceMissing:sequenceMissing,
                    sequenceCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyHashSet.AsEnumerable(),
                    repeat:s_repeatHashSet.AsEnumerable(),
                    repeatHas:repeatHas,
                    repeatMissing:repeatMissing,
                    repeatCount:1,
                    sequence:s_sequenceHashSet.AsEnumerable(),
                    sequenceHas:sequenceHas,
                    sequenceMissing:sequenceMissing,
                    sequenceCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyIList.AsEnumerable(),
                    repeat:s_repeatIList.AsEnumerable(),
                    repeatHas:repeatHas,
                    repeatMissing:repeatMissing,
                    repeatCount:5,
                    sequence:s_sequenceIList.AsEnumerable(),
                    sequenceHas:sequenceHas,
                    sequenceMissing:sequenceMissing,
                    sequenceCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyIReadOnlyList.AsEnumerable(),
                    repeat:s_repeatIReadOnlyList.AsEnumerable(),
                    repeatHas:repeatHas,
                    repeatMissing:repeatMissing,
                    repeatCount:5,
                    sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                    sequenceHas:sequenceHas,
                    sequenceMissing:sequenceMissing,
                    sequenceCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyLinkedList.AsEnumerable(),
                    repeat:s_repeatLinkedList.AsEnumerable(),
                    repeatHas:repeatHas,
                    repeatMissing:repeatMissing,
                    repeatCount:5,
                    sequence:s_sequenceLinkedList.AsEnumerable(),
                    sequenceHas:sequenceHas,
                    sequenceMissing:sequenceMissing,
                    sequenceCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyList.AsEnumerable(),
                    repeat:s_repeatList.AsEnumerable(),
                    repeatHas:repeatHas,
                    repeatMissing:repeatMissing,
                    repeatCount:5,
                    sequence:s_sequenceList.AsEnumerable(),
                    sequenceHas:sequenceHas,
                    sequenceMissing:sequenceMissing,
                    sequenceCount:5
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Count<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            Func<TSource, bool> repeatHas,
            Func<TSource, bool> repeatMissing,
            int repeatCount,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> sequenceHas,
            Func<TSource, bool> sequenceMissing,
            int sequenceCount
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            AssertAreEqual(expected:0, actual:empty.Count(predicate:(value) => throw new InvalidOperationException()));
            AssertAreEqual(expected:repeatCount, actual:repeat.Count(predicate:repeatHas));
            AssertAreEqual(expected:0, actual:repeat.Count(predicate:repeatMissing));
            AssertAreEqual(expected:sequenceCount, actual:sequence.Count(predicate:sequenceHas));
            AssertAreEqual(expected:0, actual:sequence.Count(predicate:sequenceMissing));
        } 

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void DefaultIfEmpty () {
            TestNoGC(code:() => DefaultIfEmpty(source:s_emptyArray.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_sequenceArray.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_repeatArray.AsEnumerable()));
            
            TestNoGC(code:() => DefaultIfEmpty(source:s_emptyHashSet.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_sequenceHashSet.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_repeatHashSet.AsEnumerable()));
            
            TestNoGC(code:() => DefaultIfEmpty(source:s_emptyIList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_sequenceIList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_repeatIList.AsEnumerable()));
            
            TestNoGC(code:() => DefaultIfEmpty(source:s_emptyIReadOnlyList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_sequenceIReadOnlyList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_repeatIReadOnlyList.AsEnumerable()));
            
            TestNoGC(code:() => DefaultIfEmpty(source:s_emptyLinkedList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_sequenceLinkedList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_repeatLinkedList.AsEnumerable()));
            
            TestNoGC(code:() => DefaultIfEmpty(source:s_emptyList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_sequenceList.AsEnumerable()));
            TestNoGC(code:() => DefaultIfEmpty(source:s_repeatList.AsEnumerable()));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void DefaultIfEmpty<TEnumerator, TSource> (EnumerableAdapter<TEnumerator, TSource> source)
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in source.DefaultIfEmpty()) {
                    var value = enumerator.MoveNext() ? enumerator.Current : default;
                    AssertAreEqual(expected:value, actual:test);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count() > 0 ? source.Count() : 1, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Dictionary () {
            var empty = new Dictionary<int, string>();
            var diff = new Dictionary<int, string> {
                {  0, "0" }, 
                {  1, "1" }, 
                {  2, "2" }, 
            };
            
            TestNoGC(code:() => Dictionary(source:empty));
            TestNoGC(code:() => Dictionary(source:diff));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Dictionary<TKey, TValue> (Dictionary<TKey, TValue> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:false, actual:enumerable.HasIndexer);
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in enumerable) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current.Key, actual:test.Key);
                    AssertAreEqual(expected:enumerator.Current.Value, actual:test.Value);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void ElementAt () {
            ElementAt(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            ElementAt(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            ElementAt(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            ElementAt(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            ElementAt(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            ElementAt(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAt<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            ElementAt(source:empty);
            ElementAt(source:repeat);
            ElementAt(source:sequence);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAt<TEnumerator, TSource> (EnumerableAdapter<TEnumerator, TSource> source)
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => {
                var visited = 0;
                using (var enumerator = source.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        var result = source.ElementAt(index:visited);
                        AssertAreEqual(expected:enumerator.Current, actual:result);
                        ++visited;
                    }
                }
            });
            Assert.Throws<ArgumentOutOfRangeException>(code:() => source.ElementAt(index:-1));
            Assert.Throws<ArgumentOutOfRangeException>(code:() => source.ElementAt(index:source.Count()));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void ElementAtOrDefault () {
            ElementAtOrDefault(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            ElementAtOrDefault(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            ElementAtOrDefault(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            ElementAtOrDefault(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            ElementAtOrDefault(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            ElementAtOrDefault(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAtOrDefault<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            ElementAtOrDefault(source:empty);
            ElementAtOrDefault(source:repeat);
            ElementAtOrDefault(source:sequence);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAtOrDefault<TEnumerator, TSource> (EnumerableAdapter<TEnumerator, TSource> source)
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => {
                var visited = 0;
                using (var enumerator = source.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        var result = source.ElementAtOrDefault(index:visited);
                        AssertAreEqual(expected:enumerator.Current, actual:result);
                        ++visited;
                    }
                }
            });
            Assert.Throws<ArgumentOutOfRangeException>(code:() => source.ElementAtOrDefault(index:-1));
            AssertAreEqual(expected:default, actual:source.ElementAtOrDefault(index:source.Count()));
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
            First(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            First(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            First(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            First(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            First(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            First(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void First<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            Assert.Throws<InvalidOperationException>(code:() => First(source:empty, predicate:predicate));
            Assert.Throws<InvalidOperationException>(
                code:() => First(
                    source:empty, 
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            TestNoGC(code:() => First(source:repeat, predicate:predicate));
            TestNoGC(code:() => First(source:repeat, predicate:(value) => true));
            Assert.Throws<InvalidOperationException>(
                code:() => First(source:repeat, predicate:(value) => false)
            );
            TestNoGC(code:() => First(source:sequence, predicate:predicate));
            TestNoGC(code:() => First(source:sequence, predicate:(value) => true));
            Assert.Throws<InvalidOperationException>(code:() => First(source:sequence, predicate:(value) => false));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void First<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator()) {
                var result = source.First(predicate:predicate);
                while (enumerator.MoveNext()) {
                    var expected = enumerator.Current;
                    if (!predicate(arg:expected)) {
                        continue;
                    }

                    AssertAreEqual(expected:expected, actual:result);
                    return;
                }
                AssertAreEqual(expected:true, actual:enumerator.MoveNext());
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void FirstOrDefault () {
            FirstOrDefault(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            FirstOrDefault(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            FirstOrDefault(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            FirstOrDefault(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            FirstOrDefault(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            FirstOrDefault(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void FirstOrDefault<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => FirstOrDefault(source:empty, predicate:predicate));
            TestNoGC(
                code:() => FirstOrDefault(
                    source:empty, 
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            TestNoGC(code:() => FirstOrDefault(source:repeat, predicate:predicate));
            TestNoGC(code:() => FirstOrDefault(source:repeat, predicate:(value) => true));
            TestNoGC(code:() => FirstOrDefault(source:repeat, predicate:(value) => false));
            TestNoGC(code:() => FirstOrDefault(source:sequence, predicate:predicate));
            TestNoGC(code:() => FirstOrDefault(source:sequence, predicate:(value) => true));
            TestNoGC(code:() => FirstOrDefault(source:sequence, predicate:(value) => false));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void FirstOrDefault<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator()) {
                var result = source.FirstOrDefault(predicate:predicate);
                while (enumerator.MoveNext()) {
                    var expected = enumerator.Current;
                    if (!predicate(arg:expected)) {
                        continue;
                    }
                    
                    AssertAreEqual(expected:expected, actual:result);
                    return;
                }
                AssertAreEqual(expected:default, actual:result);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void HashSet () {
            TestNoGC(code:() => HashSet(source:s_emptyHashSet));
            TestNoGC(code:() => HashSet(source:s_repeatHashSet));
            TestNoGC(code:() => HashSet(source:s_sequenceHashSet));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void HashSet<TSource> (HashSet<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:false, actual:enumerable.HasIndexer);
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in enumerable) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:test);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void IList () {
            TestNoGC(code:() => IList(source:s_emptyIList));
            TestNoGC(code:() => IList(source:s_repeatIList));
            TestNoGC(code:() => IList(source:s_sequenceIList));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void IList<TSource> (IList<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:true, actual:enumerable.HasIndexer);
            var visited = 0;
            foreach (var test in enumerable) {
                AssertAreEqual(expected:source[index:visited], actual:test);
                AssertAreEqual(expected:source[index:visited], actual:enumerable[index:visited]);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void IReadOnlyList () {
            TestNoGC(code:() => IReadOnlyList(source:s_emptyIReadOnlyList));
            TestNoGC(code:() => IReadOnlyList(source:s_repeatIReadOnlyList));
            TestNoGC(code:() => IReadOnlyList(source:s_sequenceIReadOnlyList));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void IReadOnlyList<TSource> (IReadOnlyList<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:true, actual:enumerable.HasIndexer);
            var visited = 0;
            foreach (var test in enumerable) {
                AssertAreEqual(expected:source[index:visited], actual:test);
                AssertAreEqual(expected:source[index:visited], actual:enumerable[index:visited]);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Last () {
            Last(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            Last(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            Last(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            Last(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            Last(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            Last(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Last<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> predicate
        )
            where TSource : struct
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            Assert.Throws<InvalidOperationException>(code:() => Last(source:empty, predicate:predicate));
            Assert.Throws<InvalidOperationException>(
                code:() => Last(
                    source:empty, 
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            TestNoGC(code:() => Last(source:repeat, predicate:predicate));
            TestNoGC(code:() => Last(source:repeat, predicate:(value) => true));
            Assert.Throws<InvalidOperationException>(
                code:() => Last(source:repeat, predicate:(value) => false)
            );
            TestNoGC(code:() => Last(source:sequence, predicate:predicate));
            TestNoGC(code:() => Last(source:sequence, predicate:(value) => true));
            Assert.Throws<InvalidOperationException>(code:() => Last(source:sequence, predicate:(value) => false));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Last<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, bool> predicate
        )
            where TSource : struct
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator()) {
                var result = source.Last(predicate:predicate);
                TSource? last = default;
                while (enumerator.MoveNext()) {
                    var expected = enumerator.Current;
                    if (!predicate(arg:expected)) {
                        continue;
                    }

                    last = expected;
                }
                AssertAreEqual(expected:true, actual:last.HasValue);
                AssertAreEqual(expected:last.Value, actual:result);
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void LastOrDefault () {
            LastOrDefault(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            LastOrDefault(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            LastOrDefault(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            LastOrDefault(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            LastOrDefault(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
            LastOrDefault(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable(), 
                predicate:(value) => value >= 0
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void LastOrDefault<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => LastOrDefault(source:empty, predicate:predicate));
            TestNoGC(
                code:() => LastOrDefault(
                    source:empty, 
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            TestNoGC(code:() => LastOrDefault(source:repeat, predicate:predicate));
            TestNoGC(code:() => LastOrDefault(source:repeat, predicate:(value) => true));
            TestNoGC(code:() => LastOrDefault(source:repeat, predicate:(value) => false));
            TestNoGC(code:() => LastOrDefault(source:sequence, predicate:predicate));
            TestNoGC(code:() => LastOrDefault(source:sequence, predicate:(value) => true));
            TestNoGC(code:() => LastOrDefault(source:sequence, predicate:(value) => false));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void LastOrDefault<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            using (var enumerator = source.GetEnumerator()) {
                var result = source.LastOrDefault(predicate:predicate);
                TSource last = default;
                while (enumerator.MoveNext()) {
                    var expected = enumerator.Current;
                    if (!predicate(arg:expected)) {
                        continue;
                    }

                    last = expected;
                }
                AssertAreEqual(expected:last, actual:result);
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void LinkedList () {
            TestNoGC(code:() => LinkedList(source:s_emptyLinkedList));
            TestNoGC(code:() => LinkedList(source:s_repeatLinkedList));
            TestNoGC(code:() => LinkedList(source:s_sequenceLinkedList));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void LinkedList<TSource> (LinkedList<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:false, actual:enumerable.HasIndexer);
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in enumerable) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:test);
                    ++visited;
                }
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void List () {
            TestNoGC(code:() => List(source:s_emptyList));
            TestNoGC(code:() => List(source:s_repeatList));
            TestNoGC(code:() => List(source:s_sequenceList));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void List<TSource> (List<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:true, actual:enumerable.HasIndexer);
            var visited = 0;
            foreach (var test in enumerable) {
                AssertAreEqual(expected:source[index:visited], actual:test);
                AssertAreEqual(expected:source[index:visited], actual:enumerable[index:visited]);
                ++visited;
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Max () {
            Max(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            Max(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            Max(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            Max(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            Max(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            Max(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Max<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            Assert.Throws<InvalidOperationException>(code:() => empty.Max());
            TestNoGC(code:() => repeat.Max(), expected:1);
            TestNoGC(code:() => sequence.Max(), expected:4);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Min () {
            Min(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            Min(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            Min(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            Min(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            Min(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            Min(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Min<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            Assert.Throws<InvalidOperationException>(code:() => empty.Min());
            TestNoGC(code:() => repeat.Min(), expected:1);
            TestNoGC(code:() => sequence.Min(), expected:0);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Mismatch () {
            Mismatch(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            Mismatch(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            Mismatch(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            Mismatch(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            Mismatch(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            Mismatch(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Mismatch<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty, 
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:repeat));
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:sequence));
            TestNoGC(code:() => Mismatch(expected:0, first:repeat, second:empty));
            TestNoGC(code:() => Mismatch(expected:0, first:repeat, second:sequence));
            TestNoGC(code:() => Mismatch(expected:0, first:sequence, second:empty));
            TestNoGC(code:() => Mismatch(expected:0, first:sequence, second:repeat));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Mismatch<TEnumerator, TSource> (
            int? expected, 
            EnumerableAdapter<TEnumerator, TSource> first, 
            EnumerableAdapter<TEnumerator, TSource> second
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            int? result;

            result = first.Mismatch(second:first);
            AssertAreEqual(expected:default, actual:result);

            result = first.Mismatch(second:first, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:default, actual:result);

            result = second.Mismatch(second:second);
            AssertAreEqual(expected:default, actual:result);

            result = second.Mismatch(second:second, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:default, actual:result);

            result = first.Mismatch(second:second);
            AssertAreEqual(expected:expected, actual:result);

            result = first.Mismatch(second:second, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:expected, actual:result);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void None () {
            TestNoGC(
                code:() => None(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    sequence:s_sequenceArray.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => None(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => None(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => None(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => None(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => None(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable()
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void None<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            ValidateIsFalse(code:() => repeat.None());
            ValidateIsFalse(code:() => sequence.None());
            ValidateIsFalse(code:() => repeat.None(predicate:(value) => value == 1));
            ValidateIsFalse(code:() => sequence.None(predicate:(value) => value == 0));
            ValidateIsTrue(code:() => empty.None());
            ValidateIsTrue(code:() => empty.None(predicate:(value) => throw new InvalidOperationException()));
            ValidateIsTrue(code:() => repeat.None(predicate:(value) => value < 1));
            ValidateIsTrue(code:() => sequence.None(predicate:(value) => value < 0));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void OfType () {
            OfType(
                empty:new object[] {}.AsEnumerable(), 
                full:new object[] { 0, "1", "2", typeof(Type) }.AsEnumerable()
            );
            OfType(
                empty:new HashSet<object>().AsEnumerable(), 
                full:new HashSet<object>(collection:new object[] { 0, "1", "2", typeof(Type) }).AsEnumerable()
            );
            OfType(
                empty:((IList<object>)new List<object>()).AsEnumerable(), 
                full:((IList<object>)new List<object> { 0, "1", "2", typeof(Type) }).AsEnumerable()
            );
            OfType(
                empty:((IReadOnlyList<object>)new List<object>()).AsEnumerable(), 
                full:((IReadOnlyList<object>)new List<object> { 0, "1", "2", typeof(Type) }).AsEnumerable()
            );
            OfType(
                empty:new LinkedList<object>().AsEnumerable(), 
                full:new LinkedList<object>(collection:new object[] { 0, "1", "2", typeof(Type) }).AsEnumerable()
            );
            OfType(
                empty:new List<object>().AsEnumerable(), 
                full:new List<object>(collection:new object[] { 0, "1", "2", typeof(Type) }).AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void OfType<TEnumerator> (
            EnumerableAdapter<TEnumerator, object> empty,
            EnumerableAdapter<TEnumerator, object> full
        )
            where TEnumerator : IAdaptableEnumerator<object>
        {
            TestNoGC(code:() => OfType<TEnumerator, object>(source:empty, expected:0));
            TestNoGC(code:() => OfType<TEnumerator, Type>(source:empty, expected:0));
            TestNoGC(code:() => OfType<TEnumerator, string>(source:empty, expected:0));
            TestNoGC(code:() => OfType<TEnumerator, object>(source:full, expected:4));
            TestNoGC(code:() => OfType<TEnumerator, Type>(source:full, expected:1));
            TestNoGC(code:() => OfType<TEnumerator, string>(source:full, expected:2));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void OfType<TEnumerator, TResult> (EnumerableAdapter<TEnumerator, object> source, int expected) 
            where TEnumerator : IAdaptableEnumerator<object>
            where TResult : class
        {
            var visited = 0;
            foreach (var _ in source.OfType<TResult>()) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Prepend () {
            TestNoGC(
                code:() => Prepend(
                    empty:s_emptyArray.AsEnumerable(), 
                    repeat:s_repeatArray.AsEnumerable(), 
                    sequence:s_sequenceArray.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Prepend(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Prepend(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Prepend(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Prepend(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Prepend(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable()
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Prepend<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource> 
        {
            Prepend(source:empty, element:default);
            Prepend(source:repeat, element:default);
            Prepend(source:sequence, element:default);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Prepend<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            TSource element
        ) 
            where TEnumerator : IAdaptableEnumerator<TSource> 
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in source.Prepend(element:element)) {
                    var value = visited > 0 && enumerator.MoveNext() ? enumerator.Current : element;
                    AssertAreEqual(expected:value, actual:test);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:1 + source.Count(), actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Queue () {
            var empty = new Queue<int>();
            var sequence = new Queue<int> { 0, 1, 2, };
            
            TestNoGC(code:() => Queue(source:empty));
            TestNoGC(code:() => Queue(source:sequence));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Queue<TSource> (Queue<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:false, actual:enumerable.HasIndexer);
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in enumerable) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:test);
                    ++visited;
                }
            }
            AssertAreEqual(expected:source.Count, actual:visited);
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
            Func<int, float> selectTo = (value) => (float)value;
            Func<float, int> selectFrom = (value) => (int)value;
            Func<int, int, float> selectToIndexed = (value, index) => (float)(value + index);
            Func<float, int, int> selectFromIndexed = (value, index) => (int)(value - index);
            
            Select(
                empty:s_emptyArray.AsEnumerable(),
                repeat:s_repeatArray.AsEnumerable(),
                sequence:s_sequenceArray.AsEnumerable(),
                selectTo:selectTo,
                selectFrom:selectFrom,
                selectToIndexed:selectToIndexed,
                selectFromIndexed:selectFromIndexed
            );
            Select(
                empty:s_emptyHashSet.AsEnumerable(),
                repeat:s_repeatHashSet.AsEnumerable(),
                sequence:s_sequenceHashSet.AsEnumerable(),
                selectTo:selectTo,
                selectFrom:selectFrom,
                selectToIndexed:selectToIndexed,
                selectFromIndexed:selectFromIndexed
            );
            Select(
                empty:s_emptyIList.AsEnumerable(),
                repeat:s_repeatIList.AsEnumerable(),
                sequence:s_sequenceIList.AsEnumerable(),
                selectTo:selectTo,
                selectFrom:selectFrom,
                selectToIndexed:selectToIndexed,
                selectFromIndexed:selectFromIndexed
            );
            Select(
                empty:s_emptyIReadOnlyList.AsEnumerable(),
                repeat:s_repeatIReadOnlyList.AsEnumerable(),
                sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                selectTo:selectTo,
                selectFrom:selectFrom,
                selectToIndexed:selectToIndexed,
                selectFromIndexed:selectFromIndexed
            );
            Select(
                empty:s_emptyLinkedList.AsEnumerable(),
                repeat:s_repeatLinkedList.AsEnumerable(),
                sequence:s_sequenceLinkedList.AsEnumerable(),
                selectTo:selectTo,
                selectFrom:selectFrom,
                selectToIndexed:selectToIndexed,
                selectFromIndexed:selectFromIndexed
            );
            Select(
                empty:s_emptyList.AsEnumerable(),
                repeat:s_repeatList.AsEnumerable(),
                sequence:s_sequenceList.AsEnumerable(),
                selectTo:selectTo,
                selectFrom:selectFrom,
                selectToIndexed:selectToIndexed,
                selectFromIndexed:selectFromIndexed
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Select<TEnumerator, TSource, TResult> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, TResult> selectTo,
            Func<TResult, TSource> selectFrom,
            Func<TSource, int, TResult> selectToIndexed,
            Func<TResult, int, TSource> selectFromIndexed
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(
                code:() => Select<TEnumerator, TSource, TResult>(
                    source:empty, 
                    selectTo:(value) => throw new InvalidOperationException(), 
                    selectFrom:(value) => throw new InvalidOperationException()
                )
            );
            TestNoGC(
                code:() => Select<TEnumerator, TSource, TResult>(
                    source:empty, 
                    selectTo:(value, index) => throw new InvalidOperationException(), 
                    selectFrom:(value, index) => throw new InvalidOperationException()
                )
            );
            TestNoGC(
                code:() => Select<TEnumerator, TSource, TResult>(
                    source:repeat, 
                    selectTo:selectTo, 
                    selectFrom:selectFrom
                )
            );
            TestNoGC(
                code:() => Select<TEnumerator, TSource, TResult>(
                    source:repeat, 
                    selectTo:selectToIndexed, 
                    selectFrom:selectFromIndexed
                )
            );
            TestNoGC(
                code:() => Select<TEnumerator, TSource, TResult>(
                    source:sequence, 
                    selectTo:selectTo, 
                    selectFrom:selectFrom
                )
            );
            TestNoGC(
                code:() => Select<TEnumerator, TSource, TResult>(
                    source:sequence, 
                    selectTo:selectToIndexed, 
                    selectFrom:selectFromIndexed
                )
            );
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Select<TEnumerator, TSource, TResult> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, TResult> selectTo, 
            Func<TResult, TSource> selectFrom
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var result in source.Select(selector:selectTo).Select(selector:selectFrom)) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:result);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count(), actual:visited);
        }
        
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Select<TEnumerator, TSource, TResult> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, int, TResult> selectTo, 
            Func<TResult, int, TSource> selectFrom
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var result in source.Select(selector:selectTo).Select(selector:selectFrom)) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:result);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:source.Count(), actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SelectMany () {
            SelectMany(
                empty:new[] { s_emptyArray, }.AsEnumerable(),
                sequence:new[] { s_sequenceArray, }.AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:new[] { s_emptyHashSet, }.AsEnumerable(),
                sequence:new[] { s_sequenceHashSet, }.AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:new[] { s_emptyIList, }.AsEnumerable(),
                sequence:new[] { s_sequenceIList, }.AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:new[] { s_emptyIReadOnlyList, }.AsEnumerable(),
                sequence:new[] { s_sequenceIReadOnlyList, }.AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:new[] { s_emptyLinkedList, }.AsEnumerable(),
                sequence:new[] { s_sequenceLinkedList, }.AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:new[] { s_emptyList, }.AsEnumerable(),
                sequence:new[] { s_sequenceList, }.AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            
            SelectMany(
                empty:NewHashSet(value:s_emptyArray).AsEnumerable(),
                sequence:NewHashSet(value:s_sequenceArray).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewHashSet(value:s_emptyHashSet).AsEnumerable(),
                sequence:NewHashSet(value:s_sequenceHashSet).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewHashSet(value:s_emptyIList).AsEnumerable(),
                sequence:NewHashSet(value:s_sequenceIList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewHashSet(value:s_emptyIReadOnlyList).AsEnumerable(),
                sequence:NewHashSet(value:s_sequenceIReadOnlyList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewHashSet(value:s_emptyLinkedList).AsEnumerable(),
                sequence:NewHashSet(value:s_sequenceLinkedList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewHashSet(value:s_emptyList).AsEnumerable(),
                sequence:NewHashSet(value:s_sequenceList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            
            SelectMany(
                empty:NewIList(value:s_emptyArray).AsEnumerable(),
                sequence:NewIList(value:s_sequenceArray).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIList(value:s_emptyHashSet).AsEnumerable(),
                sequence:NewIList(value:s_sequenceHashSet).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIList(value:s_emptyIList).AsEnumerable(),
                sequence:NewIList(value:s_sequenceIList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIList(value:s_emptyIReadOnlyList).AsEnumerable(),
                sequence:NewIList(value:s_sequenceIReadOnlyList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIList(value:s_emptyLinkedList).AsEnumerable(),
                sequence:NewIList(value:s_sequenceLinkedList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIList(value:s_emptyList).AsEnumerable(),
                sequence:NewIList(value:s_sequenceList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            
            SelectMany(
                empty:NewIReadOnlyList(value:s_emptyArray).AsEnumerable(),
                sequence:NewIReadOnlyList(value:s_sequenceArray).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIReadOnlyList(value:s_emptyHashSet).AsEnumerable(),
                sequence:NewIReadOnlyList(value:s_sequenceHashSet).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIReadOnlyList(value:s_emptyIList).AsEnumerable(),
                sequence:NewIReadOnlyList(value:s_sequenceIList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIReadOnlyList(value:s_emptyIReadOnlyList).AsEnumerable(),
                sequence:NewIReadOnlyList(value:s_sequenceIReadOnlyList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIReadOnlyList(value:s_emptyLinkedList).AsEnumerable(),
                sequence:NewIReadOnlyList(value:s_sequenceLinkedList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewIReadOnlyList(value:s_emptyList).AsEnumerable(),
                sequence:NewIReadOnlyList(value:s_sequenceList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            
            SelectMany(
                empty:NewLinkedList(value:s_emptyArray).AsEnumerable(),
                sequence:NewLinkedList(value:s_sequenceArray).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewLinkedList(value:s_emptyHashSet).AsEnumerable(),
                sequence:NewLinkedList(value:s_sequenceHashSet).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewLinkedList(value:s_emptyIList).AsEnumerable(),
                sequence:NewLinkedList(value:s_sequenceIList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewLinkedList(value:s_emptyIReadOnlyList).AsEnumerable(),
                sequence:NewLinkedList(value:s_sequenceIReadOnlyList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewLinkedList(value:s_emptyLinkedList).AsEnumerable(),
                sequence:NewLinkedList(value:s_sequenceLinkedList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewLinkedList(value:s_emptyList).AsEnumerable(),
                sequence:NewLinkedList(value:s_sequenceList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            
            SelectMany(
                empty:NewList(value:s_emptyArray).AsEnumerable(),
                sequence:NewList(value:s_sequenceArray).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewList(value:s_emptyHashSet).AsEnumerable(),
                sequence:NewList(value:s_sequenceHashSet).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewList(value:s_emptyIList).AsEnumerable(),
                sequence:NewList(value:s_sequenceIList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewList(value:s_emptyIReadOnlyList).AsEnumerable(),
                sequence:NewList(value:s_sequenceIReadOnlyList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewList(value:s_emptyLinkedList).AsEnumerable(),
                sequence:NewList(value:s_sequenceLinkedList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
            SelectMany(
                empty:NewList(value:s_emptyList).AsEnumerable(),
                sequence:NewList(value:s_sequenceList).AsEnumerable(),
                selector:(source) => source.AsEnumerable(),
                selectorIndexed:(source, index) => source.AsEnumerable(),
                collectionSelector:(source) => source.AsEnumerable(),
                collectionSelectorIndexed:(source, index) => source.AsEnumerable(),
                resultSelector:(source, indirect) => (float)indirect
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<
            TOuterEnumerator, 
            TInnerEnumerable, 
            TIndirectEnumerator,
            TIndirect,
            TResult
        > (
            EnumerableAdapter<TOuterEnumerator, TInnerEnumerable> empty,
            EnumerableAdapter<TOuterEnumerator, TInnerEnumerable> sequence,
            Func<TInnerEnumerable, EnumerableAdapter<TIndirectEnumerator, TIndirect>> selector,
            Func<TInnerEnumerable, int, EnumerableAdapter<TIndirectEnumerator, TIndirect>> selectorIndexed,
            Func<TInnerEnumerable, EnumerableAdapter<TIndirectEnumerator, TIndirect>> collectionSelector,
            Func<TInnerEnumerable, int, EnumerableAdapter<TIndirectEnumerator, TIndirect>> collectionSelectorIndexed,
            Func<TInnerEnumerable, TIndirect, TResult> resultSelector
        )
            where TOuterEnumerator : IAdaptableEnumerator<TInnerEnumerable>
            where TIndirectEnumerator : IAdaptableEnumerator<TIndirect>
        {
            TestNoGC(
                code:() => SelectMany(
                    source:empty, 
                    selector:selector, 
                    expected:0
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:empty, 
                    selector:selectorIndexed, 
                    expected:0
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:empty, 
                    collectionSelector:collectionSelector, 
                    resultSelector:resultSelector, 
                    expected:0
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:empty, 
                    collectionSelector:collectionSelectorIndexed, 
                    resultSelector:resultSelector, 
                    expected:0
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:sequence, 
                    selector:selector, 
                    expected:5
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:sequence, 
                    selector:selectorIndexed, 
                    expected:5
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:sequence, 
                    collectionSelector:collectionSelector, 
                    resultSelector:resultSelector, 
                    expected:5
                )
            );
            TestNoGC(
                code:() => SelectMany(
                    source:sequence, 
                    collectionSelector:collectionSelectorIndexed, 
                    resultSelector:resultSelector, 
                    expected:5
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<
            TOuterEnumerator, 
            TInnerEnumerable, 
            TResultEnumerator, 
            TResult
        > (
            EnumerableAdapter<TOuterEnumerator, TInnerEnumerable> source,
            Func<TInnerEnumerable, EnumerableAdapter<TResultEnumerator, TResult>> selector,
            int expected
        )
            where TOuterEnumerator : IAdaptableEnumerator<TInnerEnumerable>
            where TResultEnumerator : IAdaptableEnumerator<TResult>
        {
            var visited = 0;
            foreach (var _ in source.SelectMany(selector:selector)) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<
            TOuterEnumerator, 
            TInnerEnumerable, 
            TResultEnumerator, 
            TResult
        > (
            EnumerableAdapter<TOuterEnumerator, TInnerEnumerable> source,
            Func<TInnerEnumerable, int, EnumerableAdapter<TResultEnumerator, TResult>> selector,
            int expected
        )
            where TOuterEnumerator : IAdaptableEnumerator<TInnerEnumerable>
            where TResultEnumerator : IAdaptableEnumerator<TResult>
        {
            var visited = 0;
            foreach (var _ in source.SelectMany(selector:selector)) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<
            TOuterEnumerator, 
            TInnerEnumerable, 
            TIndirectEnumerator,
            TIndirect,
            TResult
        > (
            EnumerableAdapter<TOuterEnumerator, TInnerEnumerable> source,
            Func<TInnerEnumerable, EnumerableAdapter<TIndirectEnumerator, TIndirect>> collectionSelector,
            Func<TInnerEnumerable, TIndirect, TResult> resultSelector,
            int expected
        )
            where TOuterEnumerator : IAdaptableEnumerator<TInnerEnumerable>
            where TIndirectEnumerator : IAdaptableEnumerator<TIndirect>
        {
            var visited = 0;
            foreach (var _ in source.SelectMany(
                    collectionSelector:collectionSelector, 
                    resultSelector:resultSelector
                )
            ) {
                ++visited;
            }
            AssertAreEqual(expected:expected, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SelectMany<
            TOuterEnumerator, 
            TInnerEnumerable, 
            TIndirectEnumerator,
            TIndirect,
            TResult
        > (
            EnumerableAdapter<TOuterEnumerator, TInnerEnumerable> source,
            Func<TInnerEnumerable, int, EnumerableAdapter<TIndirectEnumerator, TIndirect>> collectionSelector,
            Func<TInnerEnumerable, TIndirect, TResult> resultSelector,
            int expected
        )
            where TOuterEnumerator : IAdaptableEnumerator<TInnerEnumerable>
            where TIndirectEnumerator : IAdaptableEnumerator<TIndirect>
        {
            var visited = 0;
            foreach (var _ in source.SelectMany(
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
            SequenceEqual(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            SequenceEqual(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            SequenceEqual(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            SequenceEqual(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            SequenceEqual(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            SequenceEqual(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SequenceEqual<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => SequenceEqual(expected:false, first:empty, second:repeat));
            TestNoGC(code:() => SequenceEqual(expected:false, first:empty, second:sequence));
            TestNoGC(code:() => SequenceEqual(expected:false, first:repeat, second:empty));
            TestNoGC(code:() => SequenceEqual(expected:false, first:repeat, second:sequence));
            TestNoGC(code:() => SequenceEqual(expected:false, first:sequence, second:empty));
            TestNoGC(code:() => SequenceEqual(expected:false, first:sequence, second:repeat));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SequenceEqual<TEnumerator, TSource> (
            bool expected, 
            EnumerableAdapter<TEnumerator, TSource> first, 
            EnumerableAdapter<TEnumerator, TSource> second
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            bool result;

            result = first.SequenceEqual(second:first);
            AssertAreEqual(expected:true, actual:result);

            result = first.SequenceEqual(second:first, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:true, actual:result);

            result = second.SequenceEqual(second:second);
            AssertAreEqual(expected:true, actual:result);

            result = second.SequenceEqual(second:second, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:true, actual:result);

            result = first.SequenceEqual(second:second);
            AssertAreEqual(expected:expected, actual:result);

            result = first.SequenceEqual(second:second, comparer:EqualityComparer<TSource>.Default);
            AssertAreEqual(expected:expected, actual:result);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Single () {
            Func<int, bool> sequenceHas = (value) => value == 0;
            Single(
                empty:s_emptyArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            Single(
                empty:s_emptyHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            Single(
                empty:s_emptyIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            Single(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            Single(
                empty:s_emptyLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            Single(
                empty:s_emptyList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Single<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> sequenceHas
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            Assert.Throws<InvalidOperationException>(code:() => empty.Single());
            Assert.Throws<InvalidOperationException>(
                code:() => empty.Single(
                    //  Wrap this invalid exception with an aggregate because it's not the expected throw
                    //
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            Assert.Throws<InvalidOperationException>(code:() => sequence.Single());
            TestNoGC(code:() => sequence.Single(predicate:sequenceHas));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SingleOrDefault () {
            Func<int, bool> sequenceHas = (value) => value == 0;
            SingleOrDefault(
                empty:s_emptyArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            SingleOrDefault(
                empty:s_emptyHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            SingleOrDefault(
                empty:s_emptyIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            SingleOrDefault(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            SingleOrDefault(
                empty:s_emptyLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
            SingleOrDefault(
                empty:s_emptyList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable(),
                sequenceHas:sequenceHas
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SingleOrDefault<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            Func<TSource, bool> sequenceHas
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => empty.SingleOrDefault());
            TestNoGC(
                code:() => empty.SingleOrDefault(
                    //  Wrap this invalid exception with an aggregate because it's not the expected throw
                    //
                    predicate:(value) => throw new AggregateException(new InvalidOperationException())
                )
            );
            Assert.Throws<InvalidOperationException>(code:() => sequence.SingleOrDefault());
            TestNoGC(code:() => sequence.SingleOrDefault(predicate:sequenceHas));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Skip () {
            Skip(
                empty:s_emptyArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            Skip(
                empty:s_emptyHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            Skip(
                empty:s_emptyIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            Skip(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            Skip(
                empty:s_emptyLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            Skip(
                empty:s_emptyList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Skip<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => Skip(source:empty, count:0, skipped:0, remaining:0));
            TestNoGC(code:() => Skip(source:empty, count:1, skipped:0, remaining:0));
            TestNoGC(code:() => Skip(source:empty, count:10, skipped:0, remaining:0));
            TestNoGC(code:() => Skip(source:sequence, count:0, skipped:0, remaining:5));
            TestNoGC(code:() => Skip(source:sequence, count:1, skipped:1, remaining:4));
            TestNoGC(code:() => Skip(source:sequence, count:10, skipped:5, remaining:0));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Skip<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            int count, 
            int skipped, 
            int remaining
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                for (var i = 0; i < count && enumerator.MoveNext(); ++i) { }
                foreach (var result in source.Skip(count:count)) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:result);
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:skipped, actual:source.Count() - visited);
            AssertAreEqual(expected:remaining, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void SkipWhile () {
            SkipWhile(
                empty:s_emptyArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            SkipWhile(
                empty:s_emptyHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            SkipWhile(
                empty:s_emptyIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            SkipWhile(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            SkipWhile(
                empty:s_emptyLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            SkipWhile(
                empty:s_emptyList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void SkipWhile<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            SkipWhile(source:empty, count:0, skipped:0, remaining:0);
            SkipWhile(source:empty, count:1, skipped:0, remaining:0);
            SkipWhile(source:empty, count:10, skipped:0, remaining:0);
            SkipWhile(source:sequence, count:0, skipped:0, remaining:5);
            SkipWhile(source:sequence, count:1, skipped:1, remaining:4);
            SkipWhile(source:sequence, count:10, skipped:5, remaining:0);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void SkipWhile<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source,
            int count,
            int skipped,
            int remaining
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            //  Using a lambda rather than a local function allows us to capture outside of the TestNoGC.
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
        private static void SkipWhile<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source,
            int count,
            int skipped,
            int remaining,
            Func<TSource, int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                for (var i = 0; enumerator.MoveNext() && predicate(arg1:enumerator.Current, arg2:i); ++i) { }
                foreach (var result in source.SkipWhile(predicate:predicate)) {
                    AssertAreEqual(expected:enumerator.Current, actual:result);
                    enumerator.MoveNext();
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:enumerator.MoveNext());
            }
            AssertAreEqual(expected:skipped, actual:source.Count() - visited);
            AssertAreEqual(expected:remaining, actual:visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Stack () {
            var empty = new Stack<int>();
            var sequence = new Stack<int> { 0, 1, 2, };
            
            TestNoGC(code:() => Stack(source:empty));
            TestNoGC(code:() => Stack(source:sequence));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Stack<TSource> (Stack<TSource> source) {
            var enumerable = source.AsEnumerable();
            AssertAreEqual(expected:true, actual:enumerable.HasCount);
            AssertAreEqual(expected:source.Count, actual:enumerable.Count());
            AssertAreEqual(expected:false, actual:enumerable.HasIndexer);
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var test in enumerable) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:test);
                    ++visited;
                }
            }
            AssertAreEqual(expected:source.Count, actual:visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Take () {
            Take(
                empty:s_emptyArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            Take(
                empty:s_emptyHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            Take(
                empty:s_emptyIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            Take(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            Take(
                empty:s_emptyLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            Take(
                empty:s_emptyList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Take<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TestNoGC(code:() => Take(source:empty, count:0, taken:0, remaining:0));
            TestNoGC(code:() => Take(source:empty, count:1, taken:0, remaining:0));
            TestNoGC(code:() => Take(source:empty, count:10, taken:0, remaining:0));
            TestNoGC(code:() => Take(source:sequence, count:0, taken:0, remaining:5));
            TestNoGC(code:() => Take(source:sequence, count:1, taken:1, remaining:4));
            TestNoGC(code:() => Take(source:sequence, count:10, taken:5, remaining:0));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Take<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            int count, 
            int taken, 
            int remaining
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var result in source.Take(count:count)) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:result);
                    ++visited;
                }
            }
            AssertAreEqual(expected:taken, actual:visited);
            AssertAreEqual(expected:remaining, actual:source.Count() - visited);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void TakeWhile () {
            TakeWhile(
                empty:s_emptyArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            TakeWhile(
                empty:s_emptyHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            TakeWhile(
                empty:s_emptyIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            TakeWhile(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            TakeWhile(
                empty:s_emptyLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            TakeWhile(
                empty:s_emptyList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void TakeWhile<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            TakeWhile(source:empty, count:0, taken:0, remaining:0);
            TakeWhile(source:empty, count:1, taken:0, remaining:0);
            TakeWhile(source:empty, count:10, taken:0, remaining:0);
            TakeWhile(source:sequence, count:0, taken:0, remaining:5);
            TakeWhile(source:sequence, count:1, taken:1, remaining:4);
            TakeWhile(source:sequence, count:10, taken:5, remaining:0);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void TakeWhile<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source,
            int count,
            int taken,
            int remaining
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            //  Using a lambda rather than a local function allows us to capture outside of the TestNoGC.
            // ReSharper disable once ConvertToLocalFunction
            // ReSharper disable once ImplicitlyCapturedClosure
            Func<TSource, int, bool> predicate = (value, index) => index < count;
            TestNoGC(
                code:() => TakeWhile(
                    source:source, 
                    count:count, 
                    taken:taken, 
                    remaining:remaining, 
                    predicate:predicate
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void TakeWhile<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source,
            int count,
            int taken,
            int remaining,
            Func<TSource, int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            using (var enumerator = source.GetEnumerator()) {
                foreach (var result in source.TakeWhile(predicate:predicate)) {
                    AssertAreEqual(expected:true, actual:enumerator.MoveNext());
                    AssertAreEqual(expected:enumerator.Current, actual:result);
                    ++visited;
                }
            }
            AssertAreEqual(expected:taken, actual:visited);
            AssertAreEqual(expected:remaining, actual:source.Count() - visited);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Where () {
            Func<int, bool> repeatHas = (value) => value == 1;
            Func<int, int, bool> repeatHasIndexed = (value, index) => value == 1;
            Func<int, bool> sequenceHas = (value) => value == 1;
            Func<int, int, bool> sequenceHasIndexed = (value, index) => value == 1;

            Where(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                repeatCount:5, 
                repeatHas:repeatHas, 
                repeatHasIndexed:repeatHasIndexed, 
                sequence:s_sequenceArray.AsEnumerable(),
                sequenceCount:1,
                sequenceHas:sequenceHas,
                sequenceHasIndexed:sequenceHasIndexed
            );
            Where(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                repeatCount:1, 
                repeatHas:repeatHas, 
                repeatHasIndexed:repeatHasIndexed, 
                sequence:s_sequenceHashSet.AsEnumerable(),
                sequenceCount:1,
                sequenceHas:sequenceHas,
                sequenceHasIndexed:sequenceHasIndexed
            );
            Where(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                repeatCount:5, 
                repeatHas:repeatHas, 
                repeatHasIndexed:repeatHasIndexed, 
                sequence:s_sequenceIList.AsEnumerable(),
                sequenceCount:1,
                sequenceHas:sequenceHas,
                sequenceHasIndexed:sequenceHasIndexed
            );
            Where(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                repeatCount:5, 
                repeatHas:repeatHas, 
                repeatHasIndexed:repeatHasIndexed, 
                sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                sequenceCount:1,
                sequenceHas:sequenceHas,
                sequenceHasIndexed:sequenceHasIndexed
            );
            Where(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                repeatCount:5, 
                repeatHas:repeatHas, 
                repeatHasIndexed:repeatHasIndexed, 
                sequence:s_sequenceLinkedList.AsEnumerable(),
                sequenceCount:1,
                sequenceHas:sequenceHas,
                sequenceHasIndexed:sequenceHasIndexed
            );
            Where(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                repeatCount:5, 
                repeatHas:repeatHas, 
                repeatHasIndexed:repeatHasIndexed, 
                sequence:s_sequenceList.AsEnumerable(),
                sequenceCount:1,
                sequenceHas:sequenceHas,
                sequenceHasIndexed:sequenceHasIndexed
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Where<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            int repeatCount,
            Func<TSource, bool> repeatHas,
            Func<TSource, int, bool> repeatHasIndexed,
            EnumerableAdapter<TEnumerator, TSource> sequence,
            int sequenceCount,
            Func<TSource, bool> sequenceHas,
            Func<TSource, int, bool> sequenceHasIndexed
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
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
                code:() => Where(source:repeat, predicate:repeatHas, expected:repeatCount)
            );
            TestNoGC(
                code:() => Where(source:repeat, predicate:repeatHasIndexed, expected:repeatCount)
            );
            TestNoGC(
                code:() => Where(source:sequence, predicate:sequenceHas, expected:sequenceCount)
            );
            TestNoGC(
                code:() => Where(source:sequence, predicate:sequenceHasIndexed, expected:sequenceCount)
            );
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Where<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, bool> predicate, 
            int expected
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            foreach (var _ in source.Where(predicate:predicate)) {
                ++visited;
            }
            AssertAreEqual(expected:visited, actual:expected);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Where<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> source, 
            Func<TSource, int, bool> predicate, 
            int expected
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            var visited = 0;
            foreach (var _ in source.Where(predicate:predicate)) {
                ++visited;
            }
            AssertAreEqual(expected:visited, actual:expected);
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Zip () {
            Zip(
                empty:s_emptyArray.AsEnumerable(), 
                repeat:s_repeatArray.AsEnumerable(), 
                sequence:s_sequenceArray.AsEnumerable()
            );
            Zip(
                empty:s_emptyHashSet.AsEnumerable(), 
                repeat:s_repeatHashSet.AsEnumerable(), 
                sequence:s_sequenceHashSet.AsEnumerable()
            );
            Zip(
                empty:s_emptyIList.AsEnumerable(), 
                repeat:s_repeatIList.AsEnumerable(), 
                sequence:s_sequenceIList.AsEnumerable()
            );
            Zip(
                empty:s_emptyIReadOnlyList.AsEnumerable(), 
                repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                sequence:s_sequenceIReadOnlyList.AsEnumerable()
            );
            Zip(
                empty:s_emptyLinkedList.AsEnumerable(), 
                repeat:s_repeatLinkedList.AsEnumerable(), 
                sequence:s_sequenceLinkedList.AsEnumerable()
            );
            Zip(
                empty:s_emptyList.AsEnumerable(), 
                repeat:s_repeatList.AsEnumerable(), 
                sequence:s_sequenceList.AsEnumerable()
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Zip<TEnumerator, TSource> (
            EnumerableAdapter<TEnumerator, TSource> empty,
            EnumerableAdapter<TEnumerator, TSource> repeat,
            EnumerableAdapter<TEnumerator, TSource> sequence
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
        {
            Zip(first:empty, second:empty);
            Zip(first:empty, second:repeat);
            Zip(first:empty, second:sequence);
            Zip(first:repeat, second:sequence);
            Zip(first:repeat, second:empty);
            Zip(first:sequence, second:repeat);
            Zip(first:sequence, second:empty);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Zip<TFirstEnumerator, TFirstSource, TSecondEnumerator, TSecondSource> (
            EnumerableAdapter<TFirstEnumerator, TFirstSource> first,
            EnumerableAdapter<TSecondEnumerator, TSecondSource> second
        )
            where TFirstEnumerator : IAdaptableEnumerator<TFirstSource>
            where TSecondEnumerator : IAdaptableEnumerator<TSecondSource>
        {
            //  Using a lambda rather than a local function allows us to capture outside of the TestNoGC.
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
        private static void Zip<TFirstEnumerator, TFirstSource, TSecondEnumerator, TSecondSource, TResult> (
            EnumerableAdapter<TFirstEnumerator, TFirstSource> first, 
            EnumerableAdapter<TSecondEnumerator, TSecondSource> second, 
            Func<TFirstSource, TSecondSource, TResult> resultSelector,
            Func<TFirstSource, TSecondSource, TResult, bool> resultTester
        )
            where TFirstEnumerator : IAdaptableEnumerator<TFirstSource>
            where TSecondEnumerator : IAdaptableEnumerator<TSecondSource>
        {
            var visited = 0;
            using (var firstEnumerator = first.GetEnumerator())
            using (var secondEnumerator = second.GetEnumerator()) {
                foreach (var result in first.Zip(second:second, resultSelector:resultSelector)) {
                    AssertAreEqual(expected:true, actual:firstEnumerator.MoveNext());
                    AssertAreEqual(expected:true, actual:secondEnumerator.MoveNext());
                    AssertAreEqual(
                        expected:true,
                        actual:resultTester(arg1:firstEnumerator.Current, arg2:secondEnumerator.Current, arg3:result)
                    );
                    ++visited;
                }
                AssertAreEqual(expected:false, actual:firstEnumerator.MoveNext() && secondEnumerator.MoveNext());
            }
            AssertAreEqual(expected:Math.Min(val1:first.Count(), val2:second.Count()), actual:visited);
        }
    }
    
}