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
using System.Collections.Generic;

namespace Relinq {

    public struct IReadOnlyListEnumerator<TSource> :
        IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current => 
            m_index > 0 && m_index <= m_list.Count ? m_list[m_index - 1] : throw new InvalidOperationException()
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly IReadOnlyList<TSource> m_list;
        private int m_index;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public IReadOnlyListEnumerator (IReadOnlyList<TSource> list) {
            m_list = list ?? throw new ArgumentNullException(paramName:nameof(list));
            m_index = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () { }

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            if (m_index >= m_list.Count) {
                return false;
            }

            ++m_index;
            return true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_index = 0;
    }
    
}