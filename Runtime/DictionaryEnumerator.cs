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

    public struct DictionaryEnumerator<TKey, TValue> : IAdaptableEnumerator<KeyValuePair<TKey, TValue>> {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => m_dictionary.Count;
        public KeyValuePair<TKey, TValue> Current => m_enumerator.Current;
        public bool HasCount => true;
        public bool HasIndexer => false;
        public KeyValuePair<TKey, TValue> this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private readonly Dictionary<TKey, TValue> m_dictionary;
        private Dictionary<TKey, TValue>.Enumerator m_enumerator;
        
        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public DictionaryEnumerator (Dictionary<TKey, TValue> dictionary) {
            m_dictionary = dictionary ?? throw new ArgumentNullException(paramName:nameof(dictionary));
            m_enumerator = dictionary.GetEnumerator();
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () => m_enumerator.MoveNext();
        
        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator = m_dictionary.GetEnumerator();
    }
    
}