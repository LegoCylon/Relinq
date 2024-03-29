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

    public struct OfTypeEnumerator<TEnumerator, TSource, TResult> :
        IAdaptableEnumerator<TResult>
        where TEnumerator : IAdaptableEnumerator<TSource>
        where TResult : class, TSource
    {
        //--------------------------------------------------------------------------------------------------------------
        //  Properties
        //--------------------------------------------------------------------------------------------------------------
        public int Count => throw new NotSupportedException();
        public TResult Current => m_enumerator.Current as TResult;
        public bool HasCount => false;
        public bool HasIndexer => false;
        public TResult this [int index] => throw new NotSupportedException();

        //--------------------------------------------------------------------------------------------------------------
        //  Variables
        //--------------------------------------------------------------------------------------------------------------
        private TEnumerator m_enumerator;

        //--------------------------------------------------------------------------------------------------------------
        //  Methods
        //--------------------------------------------------------------------------------------------------------------
        public OfTypeEnumerator (in TEnumerator enumerator) {
            m_enumerator = enumerator;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public void Dispose () => m_enumerator.Dispose();

        //--------------------------------------------------------------------------------------------------------------
        public bool MoveNext () {
            while (m_enumerator.MoveNext()) {
                if (m_enumerator.Current is TResult) {
                    return true;
                }
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Reset () => m_enumerator.Reset();
    }
    
}