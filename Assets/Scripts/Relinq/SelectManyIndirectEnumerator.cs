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

namespace Relinq {

    public struct SelectManyIndirectEnumerator<
        TEnumerator, 
        TSource, 
        TSourceEnumerator, 
        TIndirect,
        TResult
    > : IAdaptedEnumerator<
            SelectManyIndirectEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect,
                TResult
            >,
            TResult
        >
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
        where TSourceEnumerator : IAdaptedEnumerator<TSourceEnumerator, TIndirect>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<
            SelectManyIndirectEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect,
                TResult
            >,
            TResult
        >
            Description { get; } = 
            new EnumeratorDescription<
                SelectManyIndirectEnumerator<TEnumerator, TSource, TSourceEnumerator, TIndirect, TResult>, 
                TResult
            >(
                current:(
                    ref SelectManyIndirectEnumerator<
                        TEnumerator, 
                        TSource, 
                        TSourceEnumerator, 
                        TIndirect, 
                        TResult
                    > enumerator
                ) => enumerator.Current,
                dispose:(
                    ref SelectManyIndirectEnumerator<
                        TEnumerator, 
                        TSource, 
                        TSourceEnumerator, 
                        TIndirect, 
                        TResult
                    > enumerator
                ) => enumerator.Dispose(),
                moveNext:(
                    ref SelectManyIndirectEnumerator<
                        TEnumerator, 
                        TSource, 
                        TSourceEnumerator, 
                        TIndirect, 
                        TResult
                    > enumerator
                ) => enumerator.MoveNext(),
                reset:(
                    ref SelectManyIndirectEnumerator<
                        TEnumerator, 
                        TSource, 
                        TSourceEnumerator, 
                        TIndirect, 
                        TResult
                    > enumerator
                ) => enumerator.Reset() 
            )
        ;
        private TResult Current => m_resultSelector(arg1:m_enumerator.Current, arg2:m_collectionEnumerator.Current);

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<
            TSource, 
            EnumerableAdapter<TSourceEnumerator, TIndirect>
        > m_collectionSelector;
        private readonly Func<TSource, TIndirect, TResult> m_resultSelector;
        private bool m_hasEnumerator;
        private EnumeratorAdapter<TSourceEnumerator, TIndirect> m_collectionEnumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<
            SelectManyIndirectEnumerator<
                TEnumerator, 
                TSource, 
                TSourceEnumerator, 
                TIndirect, 
                TResult
            >,
            TResult
        >
            GetEnumerable (
                in EnumeratorAdapter<TEnumerator, TSource> enumerator,
                Func<TSource, EnumerableAdapter<TSourceEnumerator, TIndirect>> collectionSelector,
                Func<TSource, TIndirect, TResult> resultSelector
            ) =>
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
                enumerator:new EnumeratorAdapter<
                    SelectManyIndirectEnumerator<
                        TEnumerator, 
                        TSource, 
                        TSourceEnumerator, 
                        TIndirect, 
                        TResult
                    >,
                    TResult
                >(
                    description:Description,
                    enumerator:new SelectManyIndirectEnumerator<
                        TEnumerator, 
                        TSource, 
                        TSourceEnumerator, 
                        TIndirect, 
                        TResult
                    >(
                        enumerator:enumerator, 
                        collectionSelector:collectionSelector,
                        resultSelector:resultSelector
                    )
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private SelectManyIndirectEnumerator (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, EnumerableAdapter<TSourceEnumerator, TIndirect>> collectionSelector,
            Func<TSource, TIndirect, TResult> resultSelector
        ) {
            m_enumerator = enumerator;
            m_collectionSelector =
                collectionSelector ?? throw new ArgumentNullException(paramName:nameof(collectionSelector))
            ;
            m_resultSelector = resultSelector ?? throw new ArgumentNullException(paramName:nameof(resultSelector));
            m_hasEnumerator = false;
            m_collectionEnumerator = default;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            for (;;) {
                if (m_hasEnumerator && m_collectionEnumerator.MoveNext()) {
                    return true;
                }

                if (!m_enumerator.MoveNext()) {
                    return false;
                }

                m_hasEnumerator = true;
                m_collectionEnumerator = m_collectionSelector(arg:m_enumerator.Current).GetEnumerator();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            m_hasEnumerator = false;
            m_collectionEnumerator = default;
        }
    }
    
}