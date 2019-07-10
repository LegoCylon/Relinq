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

    public struct QueueEnumerator<TSource> : IAdaptableEnumerator<TSource> {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => m_queue.Count;
        public TSource Current => m_enumerator.Current;
        public bool HasCount => true;
        public bool HasIndexer => false;
        public TSource this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly Queue<TSource> m_queue;
        private Queue<TSource>.Enumerator m_enumerator;
        
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public QueueEnumerator (Queue<TSource> queue) {
            m_queue = queue ?? throw new ArgumentNullException(paramName:nameof(queue));
            m_enumerator = queue.GetEnumerator();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_enumerator.MoveNext();
        
        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator = m_queue.GetEnumerator();
    }
    
}