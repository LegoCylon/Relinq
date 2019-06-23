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

    public struct SelectIndexedEnumerator<TEnumerator, TSource, TResult> :
        IAdaptedEnumerator<TResult>
        where TEnumerator : IAdaptedEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult> 
            Description { get; } = 
            new EnumeratorDescription<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult>(
                current:(ref SelectIndexedEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Current,
                dispose:(ref SelectIndexedEnumerator<TEnumerator, TSource, TResult> enumerator) => 
                    enumerator.Dispose(),
                moveNext:(ref SelectIndexedEnumerator<TEnumerator, TSource, TResult> enumerator) => 
                    enumerator.MoveNext(),
                reset:(ref SelectIndexedEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Reset() 
            )
        ;
        private TResult Current => m_selector(arg1:m_enumerator.Current, arg2:m_visited - 1);

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<TSource, int, TResult> m_selector;
        private int m_visited;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, int, TResult> selector
        ) =>
            new EnumerableAdapter<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new EnumeratorAdapter<SelectIndexedEnumerator<TEnumerator, TSource, TResult>, TResult>(
                    description:Description,
                    enumerator:new SelectIndexedEnumerator<TEnumerator, TSource, TResult>(
                        enumerator:enumerator,
                        selector:selector
                    )
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private SelectIndexedEnumerator (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            Func<TSource, int, TResult> selector
        ) {
            m_enumerator = enumerator;
            m_selector = selector ?? throw new ArgumentNullException(paramName:nameof(selector));
            m_visited = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            if (!m_enumerator.MoveNext()) {
                return false;
            }

            ++m_visited;
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            m_visited = 0;
        }
    }
    
}