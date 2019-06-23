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

    public struct TakeEnumerator<TEnumerator, TSource> :
        IAdaptedEnumerator<TakeEnumerator<TEnumerator, TSource>, TSource>
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<TakeEnumerator<TEnumerator, TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<TakeEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref TakeEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref TakeEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref TakeEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref TakeEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current => m_enumerator.Current;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly int m_count;
        private int m_taken;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<TakeEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            int count
        ) =>
            new EnumerableAdapter<TakeEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<TakeEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new TakeEnumerator<TEnumerator, TSource>(enumerator:enumerator, count:count)
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private TakeEnumerator (in EnumeratorAdapter<TEnumerator, TSource> enumerator, int count) {
            m_enumerator = enumerator;
            m_count = count;
            m_taken = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            if (m_taken >= m_count) {
                return false;
            }

            ++m_taken;
            return m_enumerator.MoveNext();
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            m_taken = 0;
        }
    }
    
}