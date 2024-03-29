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

    public struct SelectManyEnumerator<
        TEnumerator, 
        TSource, 
        TSourceEnumerator, 
        TResult
    > : IAdaptableEnumerator<TResult>
        where TEnumerator : IAdaptableEnumerator<TSource>
        where TSourceEnumerator : IAdaptableEnumerator<TResult>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => throw new NotSupportedException();
        public TResult Current => m_resultEnumerator.Current;
        public bool HasCount => false;
        public bool HasIndexer => false;
        public TResult this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly Func<
            TSource, 
            EnumerableAdapter<TSourceEnumerator, TResult>
        > m_selector;
        private bool m_hasEnumerator;
        private TSourceEnumerator m_resultEnumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public SelectManyEnumerator (
            in TEnumerator enumerator, 
            Func<TSource, EnumerableAdapter<TSourceEnumerator, TResult>> selector
        ) {
            m_enumerator = enumerator;
            m_selector = selector ?? throw new ArgumentNullException(paramName:nameof(selector));
            m_hasEnumerator = false;
            m_resultEnumerator = default;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            for (;;) {
                if (m_hasEnumerator && m_resultEnumerator.MoveNext()) {
                    return true;
                }

                if (!m_enumerator.MoveNext()) {
                    return false;
                }

                m_hasEnumerator = true;
                m_resultEnumerator = m_selector(arg:m_enumerator.Current).GetEnumerator();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_enumerator.Reset();
            m_hasEnumerator = false;
            m_resultEnumerator = default;
        }
    }
    
}