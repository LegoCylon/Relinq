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
        public int Count => Math.Max(m_enumerator.Count - m_count, 0);
        public TSource Current => m_enumerator.Current;
        public bool HasCount => m_enumerator.HasCount;
        public bool HasIndexer => m_enumerator.HasIndexer && m_enumerator.HasCount;
        public TSource this [int index] => index >= 0 && index < Count
            ? m_enumerator[index:index - m_count]
            : throw new InvalidOperationException()
        ;

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly int m_count;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public SkipEnumerator (in TEnumerator enumerator, int count) {
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