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
        private static EnumeratorDescription<WhereIndexedEnumerator<TEnumerator, TSource>, TSource> 
            Description { get; } = 
            new EnumeratorDescription<WhereIndexedEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref WhereIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref WhereIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref WhereIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref WhereIndexedEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current => m_enumerator.Current;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<TSource, int, bool> m_predicate;
        private int m_visited;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, int, bool> predicate
        ) =>
            new EnumerableAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<WhereIndexedEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new WhereIndexedEnumerator<TEnumerator, TSource>(
                        enumerator:enumerator, 
                        predicate:predicate
                    ) 
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private WhereIndexedEnumerator (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            Func<TSource, int, bool> predicate
        ) {
            m_enumerator = enumerator;
            m_predicate = predicate ?? throw new ArgumentNullException(paramName:nameof(predicate));
            m_visited = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            while (m_enumerator.MoveNext()) {
                if (m_predicate(arg1:Current, arg2:m_visited++)) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            m_visited = 0;
        }
    }
    
}