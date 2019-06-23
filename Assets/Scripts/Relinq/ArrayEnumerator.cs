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

    public struct ArrayEnumerator<TSource> :
        IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<ArrayEnumerator<TSource>, TSource> Description { get; } =  
            new EnumeratorDescription<ArrayEnumerator<TSource>, TSource>(
                current:(ref ArrayEnumerator<TSource> enumerator) => enumerator.Current,
                dispose:(ref ArrayEnumerator<TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref ArrayEnumerator<TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref ArrayEnumerator<TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current => 
            m_index > 0 && m_index <= m_array.Length ? m_array[m_index - 1] : throw new InvalidOperationException()
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly TSource[] m_array;
        private int m_index;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<ArrayEnumerator<TSource>, TSource> GetEnumerable (TSource[] array) => 
            new EnumerableAdapter<ArrayEnumerator<TSource>, TSource>(
                enumerator:new EnumeratorAdapter<ArrayEnumerator<TSource>, TSource>(
                    description:Description,
                    enumerator:new ArrayEnumerator<TSource>(array:array)
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private ArrayEnumerator (TSource[] array) {
            m_array = array;
            m_index = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () { }

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            if (m_index >= m_array.Length) {
                return false;
            }

            ++m_index;
            return true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_index = 0;
    }
    
}