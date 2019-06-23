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

    public struct RepeatEnumerator<TSource> : IAdaptedEnumerator<RepeatEnumerator<TSource>, TSource> {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<RepeatEnumerator<TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<RepeatEnumerator<TSource>, TSource>(
                current:(ref RepeatEnumerator<TSource> enumerator) => enumerator.Current,
                dispose:(ref RepeatEnumerator<TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref RepeatEnumerator<TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref RepeatEnumerator<TSource> enumerator) => enumerator.Reset()
            )
        ;
        private TSource Current => m_index > 0 && m_index <= m_count ? m_element : throw new InvalidOperationException();

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
                enumerator:new EnumeratorAdapter<RepeatEnumerator<TSource>, TSource>(
                    description:Description,
                    enumerator:new RepeatEnumerator<TSource>(element:element, count:count)
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        private RepeatEnumerator (TSource element, int count) {
            m_element = element;
            m_count = count;
            m_index = 0;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () { }

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            if (m_index >= m_count) {
                return false;
            }

            ++m_index;
            return true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_index = 0;
    }
    
}