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
    
    public struct CountEnumerator<TSource> :
        IAdaptedEnumerator<CountEnumerator<TSource>, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<CountEnumerator<TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<CountEnumerator<TSource>, TSource>(
                current:(ref CountEnumerator<TSource> enumerator) => enumerator.Current,
                dispose:(ref CountEnumerator<TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref CountEnumerator<TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref CountEnumerator<TSource> enumerator) => enumerator.Reset()
            )
        ;
        private TSource Current => 
            m_index > 0 && m_index <= m_count ? 
                m_generator(arg1:m_start, arg2:m_index - 1) : 
                throw new InvalidOperationException()
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly TSource m_start;
        private readonly int m_count;
        private readonly Func<TSource, int, TSource> m_generator;
        private int m_index;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<CountEnumerator<TSource>, TSource> GetEnumerable (
            TSource start, 
            int count, 
            Func<TSource, int, TSource> generator
        ) =>
            new EnumerableAdapter<CountEnumerator<TSource>, TSource>(
                enumerator:new EnumeratorAdapter<CountEnumerator<TSource>, TSource>(
                    description:Description,
                    enumerator:new CountEnumerator<TSource>(start:start, count:count, generator:generator)
                )
            )
        ;

        //--------------------------------------------------------------------------------------------------------------
        private CountEnumerator (
            TSource start, 
            int count, 
            Func<TSource, int, TSource> generator
        ) {
            m_start = start;
            m_count = count;
            m_generator = generator ?? throw new ArgumentNullException(paramName:nameof(generator));
            m_index = 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () { }
        
        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () {
            if (m_index >= m_count) {
                return false;
            }

            ++m_index;
            return true;
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () => m_index = 0;
    }
    
}