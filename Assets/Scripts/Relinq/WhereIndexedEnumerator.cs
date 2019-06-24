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

    public struct WhereIndexedEnumerator<TEnumerator, TSource> : 
        IAdaptableEnumerator<TSource>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public TSource Current => m_enumerator.Current;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly Func<TSource, int, bool> m_predicate;
        private int m_visited;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in TEnumerator enumerator,
            Func<TSource, int, bool> predicate
        ) =>
            new EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new WhereIndexedEnumerator<TEnumerator, TSource>(enumerator:enumerator, predicate:predicate) 
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private WhereIndexedEnumerator (
            in TEnumerator enumerator, 
            Func<TSource, int, bool> predicate
        ) {
            m_enumerator = enumerator;
            m_predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            m_visited = 0;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            while (m_enumerator.MoveNext()) {
                if (m_predicate(arg1:m_enumerator.Current, arg2:m_visited++)) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_enumerator.Reset();
            m_visited = 0;
        }
    }
    
}