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

namespace Relinq {

    public struct PrependEnumerator<TEnumerator, TSource> : 
        IAdaptedEnumerator<PrependEnumerator<TEnumerator, TSource>, TSource> 
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<PrependEnumerator<TEnumerator, TSource>, TSource> Description { get; } =
            new EnumeratorDescription<PrependEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref PrependEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref PrependEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref PrependEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref PrependEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current => m_index > 1 ? m_enumerator.Current : m_element;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private int m_index;
        private readonly TSource m_element;
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            TSource element,
            in EnumeratorAdapter<TEnumerator, TSource> enumerator
        ) => 
            new EnumerableAdapter<PrependEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<PrependEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new PrependEnumerator<TEnumerator, TSource>(element:element, enumerator:enumerator)
                )
            )
        ; 

        //--------------------------------------------------------------------------------------------------------------
        private PrependEnumerator (TSource element, in EnumeratorAdapter<TEnumerator, TSource> enumerator) {
            m_index = 0;
            m_element = element;
            m_enumerator = enumerator;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            if (m_index > 0 && !m_enumerator.MoveNext()) {
                return false;
            }
            ++m_index;
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            m_index = 0;
        }
    }
    
}