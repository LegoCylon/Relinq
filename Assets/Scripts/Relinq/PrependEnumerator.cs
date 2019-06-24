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

    public struct PrependEnumerator<TEnumerator, TSource> : 
        IAdaptableEnumerator<TSource> 
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current => m_index > 1 ? m_enumerator.Current : m_element;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private int m_index;
        private readonly TSource m_element;
        private TEnumerator m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            TSource element,
            in TEnumerator enumerator
        ) => 
            new EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new PrependEnumerator<TEnumerator, TSource>(element:element, enumerator:enumerator)
            )
        ; 

        //--------------------------------------------------------------------------------------------------------------
        private PrependEnumerator (TSource element, in TEnumerator enumerator) {
            m_index = 0;
            m_element = element;
            m_enumerator = enumerator;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            if (m_index > 0 && !m_enumerator.MoveNext()) {
                return false;
            }
            ++m_index;
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_enumerator.Reset();
            m_index = 0;
        }
    }
    
}