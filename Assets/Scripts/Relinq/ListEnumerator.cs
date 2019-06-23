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

    public struct ListEnumerator<TSource> : IAdaptableEnumerator<TSource> {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<ListEnumerator<TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<ListEnumerator<TSource>, TSource>(
                current:(ref ListEnumerator<TSource> enumerator) => enumerator.Current,
                dispose:(ref ListEnumerator<TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref ListEnumerator<TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref ListEnumerator<TSource> enumerator) => enumerator.Reset()
            )
        ;
        private TSource Current => m_enumerator.Current;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly List<TSource> m_list;
        private List<TSource>.Enumerator m_enumerator;
        
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<ListEnumerator<TSource>, TSource> GetEnumerable (List<TSource> list) =>
            new EnumerableAdapter<ListEnumerator<TSource>, TSource>(
                enumerator:new EnumeratorAdapter<ListEnumerator<TSource>, TSource>(
                    description:Description,
                    enumerator:new ListEnumerator<TSource>(list:list)
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private ListEnumerator (List<TSource> list) {
            m_list = list ?? throw new ArgumentNullException(paramName:nameof(list));
            m_enumerator = list.GetEnumerator();
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () => m_enumerator.MoveNext();
        
        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_enumerator = m_list.GetEnumerator();
    }
    
}