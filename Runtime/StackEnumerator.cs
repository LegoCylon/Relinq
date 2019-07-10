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
using System.Collections.Generic;

namespace Relinq {

    public struct StackEnumerator<TSource> : IAdaptableEnumerator<TSource> {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => m_stack.Count;
        public TSource Current => m_enumerator.Current;
        public bool HasCount => true;
        public bool HasIndexer => false;
        public TSource this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly Stack<TSource> m_stack;
        private Stack<TSource>.Enumerator m_enumerator;
        
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public StackEnumerator (Stack<TSource> stack) {
            m_stack = stack ?? throw new ArgumentNullException(paramName:nameof(stack));
            m_enumerator = stack.GetEnumerator();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_enumerator.MoveNext();
        
        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator = m_stack.GetEnumerator();
    }
    
}