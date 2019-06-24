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

    public struct SkipEnumerator<TEnumerator, TSource> : 
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
        private readonly int m_count;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public static EnumerableAdapter<SkipEnumerator<TEnumerator, TSource>, TSource> GetEnumerable (
            in TEnumerator enumerator,
            int count
        ) =>
            new EnumerableAdapter<SkipEnumerator<TEnumerator, TSource>, TSource>(
                enumerator:new SkipEnumerator<TEnumerator, TSource>(enumerator:enumerator, count:count)
            )
        ;
        
        //--------------------------------------------------------------------------------------------------------------
        private SkipEnumerator (in TEnumerator enumerator, int count) {
            m_enumerator = enumerator;
            m_count = count;
            Setup();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        private void Setup () {
            for (var i = 0; i < m_count && m_enumerator.MoveNext(); ++i) { }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_enumerator.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () {
            m_enumerator.Reset();
            Setup();
        }
    }
    
}