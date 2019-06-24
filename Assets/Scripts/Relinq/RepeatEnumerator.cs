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

    public struct RepeatEnumerator<TSource> : IAdaptableEnumerator<TSource> {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current => 
            m_index > 0 && m_index <= m_count ? m_element : throw new InvalidOperationException()
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly TSource m_element;
        private readonly int m_count;
        private int m_index;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<RepeatEnumerator<TSource>, TSource> GetEnumerable (
            TSource element, 
            int count
        ) => 
            new EnumerableAdapter<RepeatEnumerator<TSource>, TSource>(
                enumerator:new RepeatEnumerator<TSource>(element:element, count:count)
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        private RepeatEnumerator (TSource element, int count) {
            m_element = element;
            m_count = count;
            m_index = 0;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () { }

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            if (m_index >= m_count) {
                return false;
            }

            ++m_index;
            return true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_index = 0;
    }
    
}