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
        IAdaptableEnumerator<TResult>
        where TEnumerator : IAdaptableEnumerator<TSource>
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => throw new NotSupportedException();
        public TResult Current => m_selector(arg:m_enumerator.Current);
        public bool HasCount => false;
        public bool HasIndexer => false;
        public TResult this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;
        private readonly Func<TSource, TResult> m_selector;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public SelectEnumerator (in TEnumerator enumerator, Func<TSource, TResult> selector) {
            m_enumerator = enumerator;
            m_selector = selector ?? throw new ArgumentNullException(paramName:nameof(selector));
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_enumerator.MoveNext();

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator.Reset();
    }
    
}