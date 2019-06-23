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

    public struct SkipEnumerator<TEnumerator, TSource> : 
        IAdaptedEnumerator<SkipEnumerator<TEnumerator, TSource>, TSource>
        where TEnumerator : IAdaptedEnumerator<TEnumerator, TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        private static EnumeratorDescription<SkipEnumerator<TEnumerator, TSource>, TSource> Description { get; } = 
            new EnumeratorDescription<SkipEnumerator<TEnumerator, TSource>, TSource>(
                current:(ref SkipEnumerator<TEnumerator, TSource> enumerator) => enumerator.Current,
                dispose:(ref SkipEnumerator<TEnumerator, TSource> enumerator) => enumerator.Dispose(),
                moveNext:(ref SkipEnumerator<TEnumerator, TSource> enumerator) => enumerator.MoveNext(),
                reset:(ref SkipEnumerator<TEnumerator, TSource> enumerator) => enumerator.Reset() 
            )
        ;
        private TSource Current => m_enumerator.Current;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private EnumeratorAdapter<TEnumerator, TSource> m_enumerator;
        private readonly int m_count;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<SkipEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in EnumeratorAdapter<TEnumerator, TSource> enumerator,
            int count
        ) =>
            new EnumerableAdapter<SkipEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new EnumeratorAdapter<SkipEnumerator<TEnumerator, TSource>, TSource>(
                    description:Description,
                    enumerator:new SkipEnumerator<TEnumerator, TSource>(enumerator:enumerator, count:count)
                )
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private SkipEnumerator (in EnumeratorAdapter<TEnumerator, TSource> enumerator, int count) {
            m_enumerator = enumerator;
            m_count = count;
            Setup();
        }

        //--------------------------------------------------------------------------------------------------------------
        private void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        private bool MoveNext () => m_enumerator.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        private void Reset () {
            m_enumerator.Reset();
            Setup();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Setup () {
            for (var i = 0; i < m_count && m_enumerator.MoveNext(); ++i) { }
        }
    }
    
}