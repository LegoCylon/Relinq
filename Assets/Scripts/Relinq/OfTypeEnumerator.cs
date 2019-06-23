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

    public struct OfTypeEnumerator<TEnumerator, TSource, TResult> :
        IAdaptedEnumerator<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult>
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
        where TResult : class, TSource
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult> 
            Description { get; } = new EnumeratorDescription<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult>(
                current:(ref OfTypeEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Current,
                dispose:(ref OfTypeEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Dispose(),
                moveNext:(ref OfTypeEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.MoveNext(),
                reset:(ref OfTypeEnumerator<TEnumerator, TSource, TResult> enumerator) => enumerator.Reset() 
            )
        ;
        private TResult Current => m_enumerator.Current as TResult;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator
        ) => 
            new EnumerableAdapter<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult>(
                enumerator:new EnumeratorAdapter<OfTypeEnumerator<TEnumerator, TSource, TResult>, TResult>(
                    description:Description,
                    enumerator:new OfTypeEnumerator<TEnumerator, TSource, TResult>(enumerator:enumerator)
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private OfTypeEnumerator (in EnumeratorAdapter<TEnumerator, TSource> enumerator) {
            m_enumerator = enumerator;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            while (m_enumerator.MoveNext()) {
                if (m_enumerator.Current is TResult) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_enumerator.Reset();
    }
    
}