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

namespace Relinq {

    public readonly struct EnumerableAdapter<TEnumerator, TSource> : 
        IEnumerableAdapter<TEnumerator, TSource>
        where TEnumerator : IAdaptedEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly EnumeratorAdapter<TEnumerator, TSource> m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter (in EnumeratorAdapter<TEnumerator, TSource> enumerator) {
            m_enumerator = enumerator;
        }

        //--------------------------------------------------------------------------------------------------------------
        public TSource Aggregate (Func<TSource, TSource, TSource> func) =>
            Skip(count:1).Aggregate(seed:First(), func:func, resultSelector:(value) => value)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public TAccumulate Aggregate<TAccumulate> (TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) =>
            Aggregate(seed:seed, func:func, resultSelector:(value) => value)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public TResult Aggregate<TAccumulate, TResult> (
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector
        ) {
            func = func ?? throw new ArgumentNullException(paramName:nameof(func));
            resultSelector = resultSelector ?? throw new ArgumentNullException(paramName:nameof(resultSelector));
            foreach (var element in this) {
                seed = func(arg1:seed, arg2:element);
            }
            return resultSelector(arg:seed);
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool All (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            foreach (var element in this) {
                if (!predicate(arg:element)) {
                    return false;
                }
            }
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool Any () => Any(predicate:(value) => true);

        //--------------------------------------------------------------------------------------------------------------
        public bool Any (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            foreach (var element in this) {
                if (predicate(arg:element)) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<AppendEnumerator<TEnumerator, TSource>, TSource> Append (TSource element) => 
            AppendEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, element:element);

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult> Cast<TResult> () 
            where TResult : IConvertible =>
            // ReSharper disable once HeapView.BoxingAllocation
            Cast(resultSelector:(value) => (TResult)Convert.ChangeType(value:value, conversionType:typeof(TResult)));

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult>
            Cast<TResult> (Func<TSource, TResult> resultSelector) =>
            CastEnumerator<TEnumerator, TSource, TResult>.GetEnumerable(
                enumerator:m_enumerator, 
                resultSelector:resultSelector
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<ConcatEnumerable<TEnumerator, TSecondEnumerator, TSource>, TSource> 
            Concat<TSecondEnumerator> (in EnumerableAdapter<TSecondEnumerator, TSource> second)
            where TSecondEnumerator : IAdaptedEnumerator<TSource>
            => 
            ConcatEnumerable<TEnumerator, TSecondEnumerator, TSource>.GetEnumerable(
                first:m_enumerator, 
                second:second.m_enumerator
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public bool Contains (TSource value) => Contains(value:value, comparer:null);

        //--------------------------------------------------------------------------------------------------------------
        public bool Contains (TSource value, IEqualityComparer<TSource> comparer) {
            comparer = comparer ?? EqualityComparer<TSource>.Default;
            foreach (var element in this) {
                if (comparer.Equals(x:element, y:value)) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<DefaultIfEmptyEnumerator<TEnumerator, TSource>, TSource> DefaultIfEmpty () =>
            DefaultIfEmptyEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public TSource ElementAt (int index) {
            if (index < 0) {
                throw new ArgumentOutOfRangeException(paramName:nameof(index));
            }
            foreach (var element in this) {
                if (index-- == 0) {
                    return element;
                }
            }
            throw new ArgumentOutOfRangeException(paramName:nameof(index));
        }

        //--------------------------------------------------------------------------------------------------------------
        public TSource ElementAtOrDefault (int index) {
            if (index < 0) {
                throw new ArgumentOutOfRangeException(paramName:nameof(index));
            }
            foreach (var element in this) {
                if (index-- == 0) {
                    return element;
                }
            }
            return default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource First () => First(predicate:(value) => true);

        //--------------------------------------------------------------------------------------------------------------
        public TSource First (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            foreach (var element in this) {
                if (predicate(arg:element)) {
                    return element;
                }
            }
            throw new InvalidOperationException();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource FirstOrDefault () => FirstOrDefault(predicate:(value) => true);

        //--------------------------------------------------------------------------------------------------------------
        public TSource FirstOrDefault (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            foreach (var element in this) {
                if (predicate(arg:element)) {
                    return element;
                }
            }
            return default;
        }

        //--------------------------------------------------------------------------------------------------------------
        public EnumeratorAdapter<TEnumerator, TSource> GetEnumerator () => m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        public TSource Last () => Last(predicate:(value) => true);

        //--------------------------------------------------------------------------------------------------------------
        public TSource Last (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            var found = false;
            var last = default(TSource);
            foreach (var element in this) {
                if (!predicate(arg:element)) {
                    continue;
                }
                found = true;
                last = element;
            }
            return found ? last : throw new InvalidOperationException();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource LastOrDefault () => LastOrDefault(predicate:(value) => true);

        //--------------------------------------------------------------------------------------------------------------
        public TSource LastOrDefault (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            var found = false;
            var last = default(TSource);
            foreach (var element in this) {
                if (!predicate(arg:element)) {
                    continue;
                }
                found = true;
                last = element;
            }
            return found ? last : default;
        }

        //--------------------------------------------------------------------------------------------------------------
        public TSource Max () => Max(selector:(value) => value);

        //--------------------------------------------------------------------------------------------------------------
        public TResult Max<TResult> (Func<TSource, TResult> selector) {
            var comparer = Comparer<TResult>.Default;
            var equalityComparer = EqualityComparer<TResult>.Default;
            var result = default(TResult);

            if (!typeof(TResult).IsClass && !typeof(TResult).IsNullable()) {
                var hasResult = false;
                foreach (var element in this) {
                    var selected = selector(arg:element);
                    if (!hasResult || comparer.Compare(x:selected, y:result) > 0) {
                        hasResult = true;
                        result = selected;
                    }
                }
                return hasResult ? result : throw new InvalidOperationException();
            }
            
            foreach (var element in this) {
                var selected = selector(arg:element);
                if (!equalityComparer.Equals(x:selected, y:default) && 
                    (equalityComparer.Equals(x:result, y:default) || comparer.Compare(x:selected, y:result) > 0)
                ) {
                    result = selected;
                }
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public TSource Min () => Min(selector:(value) => value);

        //--------------------------------------------------------------------------------------------------------------
        public TResult Min<TResult> (Func<TSource, TResult> selector) {
            var comparer = Comparer<TResult>.Default;
            var equalityComparer = EqualityComparer<TResult>.Default;
            var result = default(TResult);

            if (!typeof(TResult).IsClass && !typeof(TResult).IsNullable()) {
                var hasResult = false;
                foreach (var element in this) {
                    var selected = selector(arg:element);
                    if (!hasResult || comparer.Compare(x:selected, y:result) < 0) {
                        hasResult = true;
                        result = selected;
                    }
                }
                return hasResult ? result : throw new InvalidOperationException();
            }
            
            foreach (var element in this) {
                var selected = selector(arg:element);
                if (!equalityComparer.Equals(x:selected, y:default) && 
                    (equalityComparer.Equals(x:result, y:default) || comparer.Compare(x:selected, y:result) < 0)
                ) {
                    result = selected;
                }
            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int? Mismatch<TSecondEnumerator> (in EnumerableAdapter<TSecondEnumerator, TSource> second)
            where TSecondEnumerator : IAdaptedEnumerator<TSource>
            => 
            Mismatch(second:second, comparer:null)
        ; 

        //--------------------------------------------------------------------------------------------------------------
        public int? Mismatch<TSecondEnumerator> (
            in EnumerableAdapter<TSecondEnumerator, TSource> second,
            IEqualityComparer<TSource> comparer
        )
            where TSecondEnumerator : IAdaptedEnumerator<TSource>
        {
            comparer = comparer ?? EqualityComparer<TSource>.Default;

            using (var lhsEnumerator = GetEnumerator())
            using (var rhsEnumerator = second.GetEnumerator()) {
                for (var i = 0; ; ++i) {
                    var lhsResult = lhsEnumerator.MoveNext();
                    var rhsResult = rhsEnumerator.MoveNext();
                    if (lhsResult != rhsResult) {
                        return i;
                    }
                    if (!lhsResult) {
                        return default;
                    }
                    if (!comparer.Equals(x:lhsEnumerator.Current, y:rhsEnumerator.Current)) {
                        return i;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool None () => None(predicate:(value) => true);

        //--------------------------------------------------------------------------------------------------------------
        public bool None (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            foreach (var element in this) {
                if (predicate(arg:element)) {
                    return false;
                }
            }
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult> OfType<TResult> ()
            where TResult : class, TSource =>
            OfTypeEnumerator<TEnumerator, TSource, TResult>.GetEnumerable(enumerator:m_enumerator);

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource> Prepend (TSource element) => 
            PrependEnumerator<TEnumerator, TSource>.GetEnumerable(element:element, enumerator:m_enumerator);

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource>
            Replace (TSource what, TSource with, IEqualityComparer<TSource> equalityComparer) =>
            ReplaceEnumerator<TEnumerator, TSource>.GetEnumerable(
                enumerator:m_enumerator, 
                what:what, 
                with:with, 
                equalityComparer:equalityComparer
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SelectEnumerator<TEnumerator, TSource, TResult>, TResult>
            Select<TResult> (Func<TSource, TResult> selector) =>
            SelectEnumerator<TEnumerator, TSource, TResult>.GetEnumerable(enumerator:m_enumerator, selector:selector)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult> 
            Select<TResult> (Func<TSource, int, TResult> selector) =>
            SelectIndexedEnumerator<TEnumerator, TSource, TResult>.GetEnumerable(
                enumerator:m_enumerator,
                selector:selector
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SelectManyEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>, TResult>
            SelectMany<TSourceEnumerator, TResult> (
                Func<TSource, EnumerableAdapter<TSourceEnumerator, TResult>> selector
            )
            where TSourceEnumerator : IAdaptedEnumerator<TResult>
            =>
            SelectManyEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>.GetEnumerable(
                enumerator:m_enumerator, 
                selector:selector
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<
            SelectManyIndexedEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>,
            TResult
        >
            SelectMany<TSourceEnumerator, TResult> (
                Func<TSource, int, EnumerableAdapter<TSourceEnumerator, TResult>> selector
            )
            where TSourceEnumerator : IAdaptedEnumerator<TResult>
            =>
            SelectManyIndexedEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>.GetEnumerable(
                enumerator:m_enumerator, 
                selector:selector
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<
            SelectManyIndirectEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect, 
                TResult
            >,
            TResult
        >
            SelectMany<TSourceEnumerator, TIndirect, TResult> (
                Func<TSource, EnumerableAdapter<TSourceEnumerator, TIndirect>> collectionSelector,
                Func<TSource, TIndirect, TResult> resultSelector
            )
            where TSourceEnumerator : IAdaptedEnumerator<TIndirect>
            =>
            SelectManyIndirectEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect, 
                TResult
            >.GetEnumerable(
                enumerator:m_enumerator, 
                collectionSelector:collectionSelector,
                resultSelector:resultSelector
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<
            SelectManyIndirectIndexedEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect, 
                TResult
            >,
            TResult
        >
            SelectMany<TSourceEnumerator, TIndirect, TResult> (
                Func<
                    TSource, 
                    int, 
                    EnumerableAdapter<TSourceEnumerator, TIndirect>
                > collectionSelector,
                Func<TSource, TIndirect, TResult> resultSelector
            )
            where TSourceEnumerator : IAdaptedEnumerator<TIndirect>
            =>
            SelectManyIndirectIndexedEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect, 
                TResult
            >.GetEnumerable(
                enumerator:m_enumerator, 
                collectionSelector:collectionSelector,
                resultSelector:resultSelector
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public bool SequenceEqual<TSecondEnumerator> (in EnumerableAdapter<TSecondEnumerator, TSource> second)
            where TSecondEnumerator : IAdaptedEnumerator<TSource>
            => 
            SequenceEqual(second:second, comparer:null)
        ; 

        //--------------------------------------------------------------------------------------------------------------
        public bool SequenceEqual<TSecondEnumerator> (
            in EnumerableAdapter<TSecondEnumerator, TSource> second,
            IEqualityComparer<TSource> comparer
        )
            where TSecondEnumerator : IAdaptedEnumerator<TSource>
        {
            comparer = comparer ?? EqualityComparer<TSource>.Default;
            using (var lhsEnumerator = GetEnumerator())
            using (var rhsEnumerator = second.GetEnumerator()) {
                for (;;) {
                    var lhsResult = lhsEnumerator.MoveNext();
                    var rhsResult = rhsEnumerator.MoveNext();
                    if (lhsResult != rhsResult) {
                        return false;
                    }
                    if (!lhsResult) {
                        return true;
                    }
                    if (!comparer.Equals(x:lhsEnumerator.Current, y:rhsEnumerator.Current)) {
                        return false;
                    }
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource Single () => Single(predicate:(value) => true);
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource Single (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            var found = false;
            var single = default(TSource);
            foreach (var element in this) {
                if (!predicate(arg:element)) {
                    continue;
                }
                if (found) {
                    throw new InvalidOperationException();
                }
                found = true;
                single = element;
            }
            return found ? single : throw new InvalidOperationException();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource SingleOrDefault () => SingleOrDefault(predicate:(value) => true);
        
        //--------------------------------------------------------------------------------------------------------------
        public TSource SingleOrDefault (Func<TSource, bool> predicate) {
            predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            var found = false;
            var single = default(TSource);
            foreach (var element in this) {
                if (!predicate(arg:element)) {
                    continue;
                }
                if (found) {
                    throw new InvalidOperationException();
                }
                found = true;
                single = element;
            }
            return found ? single : default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SkipEnumerator<TEnumerator, TSource>, TSource> Skip (int count) =>
            SkipEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, count:count)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SkipWhileEnumerator<TEnumerator, TSource>, TSource> SkipWhile (
            Func<TSource, bool> predicate
        ) => SkipWhileEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, predicate:predicate);

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SkipWhileIndexedEnumerator<TEnumerator, TSource>, TSource> SkipWhile (
            Func<TSource, int, bool> predicate
        ) => SkipWhileIndexedEnumerator<TEnumerator, TSource>.GetEnumerable(
                enumerator:m_enumerator, 
                predicate:predicate
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<TakeEnumerator<TEnumerator, TSource>, TSource> Take (int count) =>
            TakeEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, count:count)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<TakeWhileEnumerator<TEnumerator, TSource>, TSource> TakeWhile (
            Func<TSource, bool> predicate
        ) => TakeWhileEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, predicate:predicate);
        
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource> TakeWhile (
            Func<TSource, int, bool> predicate
        ) =>
            TakeWhileIndexedEnumerator<TEnumerator, TSource>.GetEnumerable(
                enumerator:m_enumerator, 
                predicate:predicate
            ) 
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public List<TSource> ToList () {
            var results = new List<TSource>();
            ToList(results:results);
            return results;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ToList (List<TSource> results) {
            results = results ?? throw new ArgumentNullException(paramName:nameof(results));
            results.Clear();
            foreach (var element in this) {
                results.Add(item:element);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<WhereEnumerator<TEnumerator, TSource>, TSource> Where (
            Func<TSource, bool> predicate
        ) => WhereEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, predicate:predicate);

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource> Where (
            Func<TSource, int, bool> predicate
        ) => WhereIndexedEnumerator<TEnumerator, TSource>.GetEnumerable(enumerator:m_enumerator, predicate:predicate);
        
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<
            ZipEnumerator<TEnumerator, TSource, TSecondEnumerator, TSecondSource, TResult>,
            TResult
        >
            Zip<TSecondEnumerator, TSecondSource, TResult> (
                in EnumerableAdapter<TSecondEnumerator, TSecondSource> second,
                Func<TSource, TSecondSource, TResult> resultSelector
            )
            where TSecondEnumerator : IAdaptedEnumerator<TSecondSource>
            =>
            ZipEnumerator<TEnumerator, TSource, TSecondEnumerator, TSecondSource, TResult>.GetEnumerable(
                first:m_enumerator, 
                second:second.m_enumerator, 
                resultSelector:resultSelector
            )
        ;
    }

    public static class EnumerableAdapterExtensions {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static int Count<TEnumerator, TSource> (this EnumerableAdapter<TEnumerator, TSource> enumerable)
            where TEnumerator : IAdaptedEnumerator<TSource>
            =>
            enumerable.Count(predicate:(value) => true)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public static int Count<TEnumerator, TSource> (
            this EnumerableAdapter<TEnumerator, TSource> enumerable,
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptedEnumerator<TSource>
        {
            if (predicate == null) {
                throw new ArgumentNullException(paramName:nameof(predicate));
            }
            var count = 0;
            foreach (var element in enumerable) {
                if (predicate(arg:element)) {
                    ++count;
                }
            }
            return count;
        }
    }

}