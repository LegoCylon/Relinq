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
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly TEnumerator m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter (in TEnumerator enumerator) {
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
            new EnumerableAdapter<AppendEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new AppendEnumerator<TEnumerator, TSource>(enumerator:m_enumerator, element:element)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult> Cast<TResult> () 
            where TResult : IConvertible =>
            // ReSharper disable once HeapView.BoxingAllocation
            Cast(resultSelector:(value) => (TResult)Convert.ChangeType(value:value, conversionType:typeof(TResult)));

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult>
            Cast<TResult> (Func<TSource, TResult> resultSelector) =>
            new EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new CastEnumerator<TEnumerator, TSource, TResult>(
                    enumerator:m_enumerator, 
                    resultSelector:resultSelector
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<ConcatEnumerable<TEnumerator, TSecondEnumerator, TSource>, TSource> 
            Concat<TSecondEnumerator> (in EnumerableAdapter<TSecondEnumerator, TSource> second)
            where TSecondEnumerator : IAdaptableEnumerator<TSource>
            => 
            new EnumerableAdapter<ConcatEnumerable<TEnumerator, TSecondEnumerator, TSource>, TSource>(
                enumerator:new ConcatEnumerable<TEnumerator, TSecondEnumerator, TSource>(
                    first:m_enumerator,
                    second:second.m_enumerator
                )
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
        public TEnumerator GetEnumerator () => m_enumerator;

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
            where TSecondEnumerator : IAdaptableEnumerator<TSource>
            => 
            Mismatch(second:second, comparer:null)
        ; 

        //--------------------------------------------------------------------------------------------------------------
        public int? Mismatch<TSecondEnumerator> (
            in EnumerableAdapter<TSecondEnumerator, TSource> second,
            IEqualityComparer<TSource> comparer
        )
            where TSecondEnumerator : IAdaptableEnumerator<TSource>
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
            new EnumerableAdapter<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new OfTypeEnumerator<TEnumerator, TSource, TResult>(enumerator:m_enumerator)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource> Prepend (TSource element) =>
            new EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new PrependEnumerator<TEnumerator, TSource>(element:element, enumerator:m_enumerator)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource> Replace (TSource what, TSource with)
            => Replace(what:what, with:with, equalityComparer:null)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource>
            Replace (TSource what, TSource with, IEqualityComparer<TSource> equalityComparer) =>
            new EnumerableAdapter<ReplaceEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new ReplaceEnumerator<TEnumerator, TSource>(
                    enumerator:m_enumerator, 
                    replaceWhat:what, 
                    replaceWith:with, 
                    equalityComparer:equalityComparer
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SelectEnumerator<TEnumerator, TSource, TResult>, TResult>
            Select<TResult> (Func<TSource, TResult> selector) =>
            new EnumerableAdapter<SelectEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new SelectEnumerator<TEnumerator, TSource, TResult>(
                    enumerator:m_enumerator, 
                    selector:selector
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult> 
            Select<TResult> (Func<TSource, int, TResult> selector) =>
            new EnumerableAdapter<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new SelectIndexedEnumerator<TEnumerator, TSource, TResult>(
                    enumerator:m_enumerator,
                    selector:selector
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SelectManyEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>, TResult>
            SelectMany<TSourceEnumerator, TResult> (
                Func<TSource, EnumerableAdapter<TSourceEnumerator, TResult>> selector
            )
            where TSourceEnumerator : IAdaptableEnumerator<TResult>
            =>
            new EnumerableAdapter<SelectManyEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>, TResult>(
                    enumerator:new SelectManyEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>(
                    enumerator:m_enumerator, 
                    selector:selector
                )
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
            where TSourceEnumerator : IAdaptableEnumerator<TResult>
            =>
            new EnumerableAdapter<
                SelectManyIndexedEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>, 
                TResult
            >(
                enumerator:new SelectManyIndexedEnumerator<TEnumerator, TSource, TSourceEnumerator, TResult>(
                    enumerator:m_enumerator, 
                    selector:selector
                )
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
            where TSourceEnumerator : IAdaptableEnumerator<TIndirect>
            =>
            new EnumerableAdapter<
                SelectManyIndirectEnumerator<
                    TEnumerator, 
                    TSource, 
                    TSourceEnumerator, 
                    TIndirect, 
                    TResult
                >,
                TResult
            >(
                enumerator:new SelectManyIndirectEnumerator<
                    TEnumerator, 
                    TSource, 
                    TSourceEnumerator, 
                    TIndirect, 
                    TResult
                >(enumerator:m_enumerator, collectionSelector:collectionSelector, resultSelector:resultSelector)
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
                Func<TSource, int,EnumerableAdapter<TSourceEnumerator, TIndirect>> collectionSelector,
                Func<TSource, TIndirect, TResult> resultSelector
            )
            where TSourceEnumerator : IAdaptableEnumerator<TIndirect>
            =>
            new EnumerableAdapter<
                SelectManyIndirectIndexedEnumerator<
                    TEnumerator, 
                    TSource, 
                    TSourceEnumerator, 
                    TIndirect, 
                    TResult
                >,
                TResult
            >(
                enumerator:new SelectManyIndirectIndexedEnumerator<
                    TEnumerator, 
                    TSource, 
                    TSourceEnumerator, 
                    TIndirect, 
                    TResult
                >(
                    enumerator:m_enumerator, 
                    collectionSelector:collectionSelector,
                    resultSelector:resultSelector
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public bool SequenceEqual<TSecondEnumerator> (in EnumerableAdapter<TSecondEnumerator, TSource> second)
            where TSecondEnumerator : IAdaptableEnumerator<TSource>
            => 
            SequenceEqual(second:second, comparer:null)
        ; 

        //--------------------------------------------------------------------------------------------------------------
        public bool SequenceEqual<TSecondEnumerator> (
            in EnumerableAdapter<TSecondEnumerator, TSource> second,
            IEqualityComparer<TSource> comparer
        )
            where TSecondEnumerator : IAdaptableEnumerator<TSource>
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
            new EnumerableAdapter<SkipEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new SkipEnumerator<TEnumerator, TSource>(enumerator:m_enumerator, count:count)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SkipWhileEnumerator<TEnumerator, TSource>, TSource> SkipWhile (
            Func<TSource, bool> predicate
        ) =>
            new EnumerableAdapter<SkipWhileEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new SkipWhileEnumerator<TEnumerator, TSource>(enumerator:m_enumerator, predicate:predicate)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<SkipWhileIndexedEnumerator<TEnumerator, TSource>, TSource> SkipWhile (
            Func<TSource, int, bool> predicate
        ) => 
            new EnumerableAdapter<SkipWhileIndexedEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new SkipWhileIndexedEnumerator<TEnumerator, TSource>(
                    enumerator:m_enumerator, 
                    predicate:predicate
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<TakeEnumerator<TEnumerator, TSource>, TSource> Take (int count) =>
            new EnumerableAdapter<TakeEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new TakeEnumerator<TEnumerator, TSource>(enumerator:m_enumerator, count:count)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<TakeWhileEnumerator<TEnumerator, TSource>, TSource> TakeWhile (
            Func<TSource, bool> predicate
        ) =>
            new EnumerableAdapter<TakeWhileEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new TakeWhileEnumerator<TEnumerator, TSource>(enumerator:m_enumerator, predicate:predicate)
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource> TakeWhile (
            Func<TSource, int, bool> predicate
        ) =>
            new EnumerableAdapter<TakeWhileIndexedEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new TakeWhileIndexedEnumerator<TEnumerator, TSource>(
                    enumerator:m_enumerator, 
                    predicate:predicate
                )
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
        public EnumerableAdapter<WhereEnumerator<TEnumerator, TSource>, TSource> Where (Func<TSource, bool> predicate) 
            => new EnumerableAdapter<WhereEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new WhereEnumerator<TEnumerator, TSource>(enumerator:m_enumerator, predicate:predicate)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource> Where (
            Func<TSource, int, bool> predicate
        ) =>
            new EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new WhereIndexedEnumerator<TEnumerator, TSource>(
                    enumerator:m_enumerator, 
                    predicate:predicate
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        public EnumerableAdapter<
            ZipEnumerator<TEnumerator, TSource, TSecondEnumerator, TSecondSource, TResult>,
            TResult
        >
            Zip<TSecondEnumerator, TSecondSource, TResult> (
                in EnumerableAdapter<TSecondEnumerator, TSecondSource> second,
                Func<TSource, TSecondSource, TResult> resultSelector
            )
            where TSecondEnumerator : IAdaptableEnumerator<TSecondSource>
            =>
            new EnumerableAdapter<
                ZipEnumerator<TEnumerator, TSource, TSecondEnumerator, TSecondSource, TResult>, 
                TResult
            >(
                enumerator:new ZipEnumerator<
                    TEnumerator, 
                    TSource, 
                    TSecondEnumerator, 
                    TSecondSource, 
                    TResult
                >(first:m_enumerator, second:second.m_enumerator, resultSelector:resultSelector)
            )
        ;
    }

    public static class EnumerableAdapterExtensions {
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static int Count<TEnumerator, TSource> (this EnumerableAdapter<TEnumerator, TSource> enumerable)
            where TEnumerator : IAdaptableEnumerator<TSource>
            =>
            enumerable.Count(predicate:(value) => true)
        ;

        //--------------------------------------------------------------------------------------------------------------
        public static int Count<TEnumerator, TSource> (
            this EnumerableAdapter<TEnumerator, TSource> enumerable,
            Func<TSource, bool> predicate
        )
            where TEnumerator : IAdaptableEnumerator<TSource>
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