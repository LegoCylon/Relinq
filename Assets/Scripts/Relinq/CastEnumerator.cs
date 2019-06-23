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

    public struct CastEnumerator<TEnumerator, TSource, TResult> : 
        IAdaptedEnumerator<CastEnumerator<TEnumerator, TSource, TResult>, TResult>
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<CastEnumerator<TEnumerator, TSource, TResult>, TResult> 
            Description { get; } = 
            new EnumeratorDescription<CastEnumerator<TEnumerator, TSource, TResult>, TResult>(
                current:(ref CastEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Current,
                dispose:(ref CastEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Dispose(),
                moveNext:(ref CastEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.MoveNext(),
                reset:(ref CastEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Reset() 
            )
        ;
        private TResult Current => m_resultSelector(arg:m_enumerator.Current);

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly Func<TSource, TResult> m_resultSelector;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            Func<TSource, TResult> resultSelector
        ) =>
            new EnumerableAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new EnumeratorAdapter<CastEnumerator<TEnumerator, TSource, TResult>, TResult>(
                    description:Description,
                    enumerator:new CastEnumerator<TEnumerator, TSource, TResult>(
                        enumerator:enumerator,
                        resultSelector:resultSelector
                    )
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private CastEnumerator (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator, 
            Func<TSource, TResult> resultSelector
        ) {
            m_enumerator = enumerator;
            m_resultSelector = resultSelector ?? throw new ArgumentNullException(paramName:nameof(resultSelector));
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () => m_enumerator.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_enumerator.Reset();
    }
    
}