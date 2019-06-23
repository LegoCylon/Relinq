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

    public struct SelectEnumerator<TEnumerator, TSource, TResult> :
        IAdaptedEnumerator<TResult>
        where TEnumerator : IAdaptedEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<SelectEnumerator<TEnumerator, TSource, TResult>, TResult> 
            Description { get; } = new EnumeratorDescription<SelectEnumerator<TEnumerator, TSource, TResult>, TResult>(
                current:(ref SelectEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Current,
                dispose:(ref SelectEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Dispose(),
                moveNext:(ref SelectEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.MoveNext(),
                reset:(ref SelectEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Reset() 
            )
        ;
        private TResult Current => m_selector(arg:m_enumerator.Current);

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<TSource, TResult> m_selector;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<SelectEnumerator<TEnumerator, TSource, TResult>, TResult> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, TResult> selector
        ) =>
            new EnumerableAdapter<SelectEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new EnumeratorAdapter<SelectEnumerator<TEnumerator, TSource, TResult>, TResult>(
                    description:Description,
                    enumerator:new SelectEnumerator<TEnumerator, TSource, TResult>(
                        enumerator:enumerator, 
                        selector:selector
                    )
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private SelectEnumerator (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            Func<TSource, TResult> selector
        ) {
            m_enumerator = enumerator;
            m_selector = selector ?? throw new ArgumentNullException(paramName:nameof(selector));
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () => m_enumerator.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_enumerator.Reset();
    }
    
}