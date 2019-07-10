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
    > : IAdaptableEnumerator<TResult>
        where TEnumerator : IAdaptableEnumerator<TSource>
        where TSourceEnumerator : IAdaptableEnumerator<TIndirect>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => throw new NotSupportedException();
        public TResult Current => 
            m_resultSelector(arg1:m_enumerator.Current, arg2:m_collectionEnumerator.Current)
        ;
        public bool HasCount => false;
        public bool HasIndexer => false;
        public TResult this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly Func<
            TSource, 
            EnumerableAdapter<TSourceEnumerator, TIndirect>
        > m_collectionSelector;
        private readonly Func<TSource, TIndirect, TResult> m_resultSelector;
        private bool m_hasEnumerator;
        private TSourceEnumerator m_collectionEnumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public SelectManyIndirectEnumerator (
            in TEnumerator enumerator,
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
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
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
        public void Reset () {
            m_enumerator.Reset();
            m_hasEnumerator = false;
            m_collectionEnumerator = default;
        }
    }
    
}