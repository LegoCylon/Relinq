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

    public struct TakeEnumerator<TEnumerator, TSource> :
        IAdaptableEnumerator<TSource>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => Math.Min(m_enumerator.Count, m_count);
        public TSource Current => m_enumerator.Current;
        public bool HasCount => m_enumerator.HasCount;
        public bool HasIndexer => m_enumerator.HasIndexer;
        public TSource this [int index] =>
            index < m_count ? m_enumerator[index:index] : throw new InvalidOperationException()
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly int m_count;
        private int m_taken;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public TakeEnumerator (in TEnumerator enumerator, int count) {
            m_enumerator = enumerator;
            m_count = count;
            m_taken = 0;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            if (m_taken >= m_count) {
                return false;
            }

            ++m_taken;
            return m_enumerator.MoveNext();
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_enumerator.Reset();
            m_taken = 0;
        }
    }
    
}