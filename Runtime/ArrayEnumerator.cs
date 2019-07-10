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
        public int Count => m_array.Length;
        public TSource Current => this[index:m_index];
        public bool HasCount => true;
        public bool HasIndexer => true;
        public TSource this [int index] => m_array[index];

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly TSource[] m_array;
        private int m_index;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public ArrayEnumerator (TSource[] array) {
            m_array = array;
            m_index = -1;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () { }

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            if (m_index >= (m_array.Length - 1)) {
                return false;
            }

            ++m_index;
            return true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_index = -1;
    }
    
}