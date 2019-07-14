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
        private static void Append<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int> 
        {
            Append(source:empty, element:0);
            Append(source:repeat, element:1);
            Append(source:sequence, element:5);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Append<TEnumerator> (EnumerableAdapter<TEnumerator, int> source, int element) 
            where TEnumerator : IAdaptableEnumerator<int> 
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
        private static void Cast<TEnumerable, TCast> (
            EnumerableAdapter<TEnumerable, int> empty,
            EnumerableAdapter<TEnumerable, int> repeat,
            EnumerableAdapter<TEnumerable, int> sequence,
            Func<int, TCast> resultSelector
        )
            where TEnumerable : IAdaptableEnumerator<int>
            where TCast : IConvertible
        {
            Cast<TEnumerable, TCast>(source:empty, resultSelector:(value) => throw new InvalidOperationException());
            Cast(source:repeat, resultSelector:resultSelector);
            Cast(source:sequence, resultSelector:resultSelector);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Cast<TEnumerable, TCast> (
            EnumerableAdapter<TEnumerable, int> source, 
            Func<int, TCast> resultSelector
        )
            where TEnumerable : IAdaptableEnumerator<int>
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
        private static void Concat<TEnumerator> (EnumerableAdapter<TEnumerator, int> source) 
            where TEnumerator : IAdaptableEnumerator<int>
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
                    sequence:s_sequenceArray.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyHashSet.AsEnumerable(), 
                    repeat:s_repeatHashSet.AsEnumerable(), 
                    sequence:s_sequenceHashSet.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyIList.AsEnumerable(), 
                    repeat:s_repeatIList.AsEnumerable(), 
                    sequence:s_sequenceIList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyIReadOnlyList.AsEnumerable(), 
                    repeat:s_repeatIReadOnlyList.AsEnumerable(), 
                    sequence:s_sequenceIReadOnlyList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyLinkedList.AsEnumerable(), 
                    repeat:s_repeatLinkedList.AsEnumerable(), 
                    sequence:s_sequenceLinkedList.AsEnumerable()
                )
            );
            TestNoGC(
                code:() => Contains(
                    empty:s_emptyList.AsEnumerable(), 
                    repeat:s_repeatList.AsEnumerable(), 
                    sequence:s_sequenceList.AsEnumerable()
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Contains<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            ValidateIsFalse(code:() => empty.Contains(value:0));
            ValidateIsTrue(code:() => repeat.Contains(value:1));
            ValidateIsTrue(code:() => sequence.Contains(value:0));
            ValidateIsFalse(code:() => repeat.Contains(value:0));
            ValidateIsFalse(code:() => sequence.Contains(value:6));
        }

        //--------------------------------------------------------------------------------------------------------------
        [Test]
        public static void Count () {
            TestNoGC(
                code:() => Count(
                    empty:s_emptyArray.AsEnumerable(),
                    repeat:s_repeatArray.AsEnumerable(),
                    sequence:s_sequenceArray.AsEnumerable(),
                    repeatCount:5,
                    sequenceZeroCount:1,
                    sequenceTotalCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyHashSet.AsEnumerable(),
                    repeat:s_repeatHashSet.AsEnumerable(),
                    sequence:s_sequenceHashSet.AsEnumerable(),
                    repeatCount:1,
                    sequenceZeroCount:1,
                    sequenceTotalCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyIList.AsEnumerable(),
                    repeat:s_repeatIList.AsEnumerable(),
                    sequence:s_sequenceIList.AsEnumerable(),
                    repeatCount:5,
                    sequenceZeroCount:1,
                    sequenceTotalCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyIReadOnlyList.AsEnumerable(),
                    repeat:s_repeatIReadOnlyList.AsEnumerable(),
                    sequence:s_sequenceIReadOnlyList.AsEnumerable(),
                    repeatCount:5,
                    sequenceZeroCount:1,
                    sequenceTotalCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyLinkedList.AsEnumerable(),
                    repeat:s_repeatLinkedList.AsEnumerable(),
                    sequence:s_sequenceLinkedList.AsEnumerable(),
                    repeatCount:5,
                    sequenceZeroCount:1,
                    sequenceTotalCount:5
                )
            );
            TestNoGC(
                code:() => Count(
                    empty:s_emptyList.AsEnumerable(),
                    repeat:s_repeatList.AsEnumerable(),
                    sequence:s_sequenceList.AsEnumerable(),
                    repeatCount:5,
                    sequenceZeroCount:1,
                    sequenceTotalCount:5
                )
            );
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Count<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence,
            int repeatCount,
            int sequenceZeroCount,
            int sequenceTotalCount
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            AssertAreEqual(expected:0, actual:empty.Count(predicate:(value) => throw new InvalidOperationException()));
            AssertAreEqual(expected:repeatCount, actual:repeat.Count(predicate:(value) => value == 1));
            AssertAreEqual(expected:sequenceZeroCount, actual:sequence.Count(predicate:(value) => value == 0));
            AssertAreEqual(expected:repeatCount, actual:repeat.Count(predicate:(value) => value > 0));
            AssertAreEqual(expected:sequenceTotalCount, actual:sequence.Count(predicate:(value) => value < 5));
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
        private static void ElementAt<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            ElementAt(source:empty);
            ElementAt(source:repeat);
            ElementAt(source:sequence);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAt<TEnumerator> (EnumerableAdapter<TEnumerator, int> source)
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void ElementAtOrDefault<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            ElementAtOrDefault(source:empty);
            ElementAtOrDefault(source:repeat);
            ElementAtOrDefault(source:sequence);
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void ElementAtOrDefault<TEnumerator> (EnumerableAdapter<TEnumerator, int> source)
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void First<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence,
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void First<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> source, 
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void FirstOrDefault<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence,
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void FirstOrDefault<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> source, 
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void Last<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence,
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void Last<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> source, 
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            using (var enumerator = source.GetEnumerator()) {
                var result = source.Last(predicate:predicate);
                int? last = default;
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
        private static void LastOrDefault<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence,
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
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
        private static void LastOrDefault<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> source, 
            Func<int, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            using (var enumerator = source.GetEnumerator()) {
                var result = source.LastOrDefault(predicate:predicate);
                int last = default;
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
        private static void Mismatch<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty, 
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:repeat));
            TestNoGC(code:() => Mismatch(expected:0, first:empty, second:sequence));
            TestNoGC(code:() => Mismatch(expected:0, first:repeat, second:empty));
            TestNoGC(code:() => Mismatch(expected:0, first:repeat, second:sequence));
            TestNoGC(code:() => Mismatch(expected:0, first:sequence, second:empty));
            TestNoGC(code:() => Mismatch(expected:0, first:sequence, second:repeat));
        }

        //--------------------------------------------------------------------------------------------------------------
        private static void Mismatch<TEnumerator> (
            int? expected, 
            EnumerableAdapter<TEnumerator, int> first, 
            EnumerableAdapter<TEnumerator, int> second
        )
            where TEnumerator : IAdaptableEnumerator<int>
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            int? result;

            result = first.Mismatch(second:first);
            AssertAreEqual(expected:default, actual:result);

            result = first.Mismatch(second:first, comparer:EqualityComparer<int>.Default);
            AssertAreEqual(expected:default, actual:result);

            result = second.Mismatch(second:second);
            AssertAreEqual(expected:default, actual:result);

            result = second.Mismatch(second:second, comparer:EqualityComparer<int>.Default);
            AssertAreEqual(expected:default, actual:result);

            result = first.Mismatch(second:second);
            AssertAreEqual(expected:expected, actual:result);

            result = first.Mismatch(second:second, comparer:EqualityComparer<int>.Default);
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
        private static void Prepend<TEnumerator> (
            EnumerableAdapter<TEnumerator, int> empty,
            EnumerableAdapter<TEnumerator, int> repeat,
            EnumerableAdapter<TEnumerator, int> sequence
        )
            where TEnumerator : IAdaptableEnumerator<int> 
        {
            Prepend(source:empty, element:0);
            Prepend(source:repeat, element:1);
            Prepend(source:sequence, element:5);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private static void Prepend<TEnumerator> (EnumerableAdapter<TEnumerator, int> source, int element) 
            where TEnumerator : IAdaptableEnumerator<int> 
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
    }
    
}